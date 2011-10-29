using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using GpsEmulator.Utilities;

namespace GpsEmulator.MapControl
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private List<List<MapTile>> tiles = new List<List<MapTile>>();
        private TranslateTransform translateTransform = new TranslateTransform();
        private MapMarker selectionMarker;
        private List<MapMarker> Markers = new List<MapMarker>();
        private Path routePath;

        #region DependencyProperties

        public static readonly DependencyProperty MapTypeProperty = DependencyProperty.Register("MapType", typeof(MapType), typeof(MapControl), new UIPropertyMetadata(MapType.Road, new PropertyChangedCallback(MapTypePropertyChanged)));

        /// <summary>
        /// type of map
        /// </summary>
        [Category("MapControl")]
        public MapType MapType
        {
            get
            {
                return (MapType)(GetValue(MapTypeProperty));
            }
            set
            {
                SetValue(MapTypeProperty, value);
                ResetView();
            }
        }

        private static void MapTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapControl map = (MapControl)d;
            if (map != null)
            {
                map.MapType = (MapType)e.NewValue;
            }
        }
        #endregion

        #region properties
        private Point mapCenter;
        public Point MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;

                int centerXpixel, centerYpixel;
                MapUtils.LatLongToPixelXY(mapCenter.X, mapCenter.Y, zoomLevel, out centerXpixel, out centerYpixel);

                translateTransform.X = (this.ActualWidth / 2)- centerXpixel;
                translateTransform.Y = (this.ActualHeight / 2) -centerYpixel;
                InvalidateVisual();
            }
        }

        private int zoomLevel = 10;
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (value < 1 || value > 21) return;
                if (zoomLevel != value)
                {
                    zoomLevel = value;
                    ResetView();
                }
            }
        }

        IMapTileFactory mapTileFactory;
        public IMapTileFactory MapTileFactory
        {
            get { return mapTileFactory; }
            set { mapTileFactory = value; }
        }

        public Brush PathColor = Brushes.Red;

        #endregion

        public MapControl() : this(47.65, -122.24)
        {
        }

        public MapControl(double lat, double lng)
        {
            InitializeComponent();
            MapLayer.RenderTransformOrigin = new Point(0.5, 0.5);
            MapLayer.RenderTransform = translateTransform;
            MarkerLayer.RenderTransformOrigin = new Point(0.5, 0.5);
            MarkerLayer.RenderTransform = translateTransform;
            MapCenter = new Point(lat, lng);
            MapLayer.SetValue(Canvas.ZIndexProperty, 100);
            MarkerLayer.SetValue(Canvas.ZIndexProperty, 200);
            selectionMarker = AddMarker(new Markers.SelectionMarker(),lat, lng);
        }

        /// <summary>
        /// Moves the center by the specified amount of pixels
        /// </summary>
        /// <param name="dX">Pixels to move horizontally</param>
        /// <param name="dY">Pixels to move vertically</param>
        public void MoveCenter(double dX, double dY)
        {
            Point transformPoint = translateTransform.Inverse.Transform(new System.Windows.Point(dX, dY));
            int centerXpixel, centerYpixel;
            centerXpixel = (int)(transformPoint.X + ActualWidth / 2 + dX);
            centerYpixel = (int)(transformPoint.Y + ActualHeight / 2 + dY);
            double lat, lng;
            MapUtils.PixelXYToLatLong(centerXpixel, centerYpixel, zoomLevel, out lat, out lng);
            MapCenter = new Point(lat, lng);
        }

        public Point LatLongFromLocal(double imageX, double imageY)
        {
            Point localPoint = translateTransform.Inverse.Transform(new System.Windows.Point(imageX, imageY));
            double lat, lng;
            MapUtils.PixelXYToLatLong((int)(localPoint.X), (int)(localPoint.Y), zoomLevel, out lat, out lng);
            return new Point(lat, lng);
        }

        #region Markers
        public MapMarker AddMarker(UserControl marker, double lat, double lng)
        {
            if (!MapLayer.Children.Contains(marker))
            {
                MapMarker m = new MapMarker(new Point(lat, lng), marker);
                m.SetVisualLocation(zoomLevel);
                Markers.Add(m);
                MarkerLayer.Children.Add(marker);
                return m;
            }
            return null;
        }

        public bool RemoveMarker(MapMarker marker)
        {
            if (!MapLayer.Children.Contains(marker.MarkerControl))
            {
                foreach (MapMarker mapMarker in Markers)
                {
                    if (mapMarker == marker)
                    {
                        Markers.Remove(mapMarker);
                        break;
                    }
                }
                MarkerLayer.Children.Remove(marker.MarkerControl);
                return true;
            }
            return false;
        }

        public MapMarker ReplaceMarker(MapMarker oldMarker, UserControl newMarker, double lat, double lng)
        {
            MapMarker m = new MapMarker(new Point(lat, lng), newMarker);
            m.SetVisualLocation(zoomLevel);
            int i = 0;
            for (; i<Markers.Count; i++)
            {
                if (Markers[i] == oldMarker)
                {
                    Markers[i]=m;
                    break;
                }
            }
            if (i==Markers.Count) return null;

            for (i = 0; i < MarkerLayer.Children.Count; i++)
            {
                if (MarkerLayer.Children[i] == oldMarker.MarkerControl)
                {
                    MarkerLayer.Children.RemoveAt(i);
                    MarkerLayer.Children.Insert(i, newMarker);
                    break;
                }
            }
            return m;
        }


        public void RemoveAllMarkers()
        {
            Markers.Clear();
            MarkerLayer.Children.Clear();
            Markers.Add(selectionMarker);
            MarkerLayer.Children.Add(selectionMarker.MarkerControl);
            RegenerateRouteShape();
        }

        public void SetSelectedPoint(double lat, double lng)
        {
            if (selectionMarker == null)
            {
                selectionMarker = AddMarker(new Markers.SelectionMarker(), lat, lng);
            }
            else
            {
                selectionMarker.Location = new Point(lat, lng);
                selectionMarker.SetVisualLocation(zoomLevel);
            }
        }

        public void RegenerateRouteShape()
        {
            const int MAX_DISTANCE_BEFORE_DISTORTION = 5000; // 5km
            var visualPath = new List<System.Windows.Point>();
            Point? previousPoint = null;
            int x, y;

            // TODO (Shy, 9/14/2010): Need to make sure that the markers are sorted according to time. This means that I'll have to store the time there, or switch to TimedPosition.
            foreach (var marker in Markers)
            {
                if (!(marker.MarkerControl is Markers.WaypointMarker)) continue;

                int lengthInSegments = 0;
                if (previousPoint.HasValue)
                {
                    double distanceInMeters = MapUtils.GetDistance(marker.Location, previousPoint.Value);
                    lengthInSegments = (int)(distanceInMeters / MAX_DISTANCE_BEFORE_DISTORTION);
                    if (lengthInSegments > 0)
                    {
                        double dX = (marker.Location.X - previousPoint.Value.X) / (lengthInSegments + 1);
                        double dY = (marker.Location.Y - previousPoint.Value.Y) / (lengthInSegments + 1);
                        double currX = previousPoint.Value.X;
                        double currY = previousPoint.Value.Y;
                        Point currentPoint = previousPoint.Value;
                        for (int i = 0; i < lengthInSegments; i++)
                        {
                            currX += dX;
                            currY += dY;
                            MapUtils.LatLongToPixelXY(currX, currY, zoomLevel, out x, out y);
                            visualPath.Add(new System.Windows.Point(x, y));
                        }
                    }
                }

                MapUtils.LatLongToPixelXY(marker.Location.X, marker.Location.Y, zoomLevel, out x, out y);
                visualPath.Add(new Point(x, y));

                previousPoint = marker.Location;
            }

            MarkerLayer.Children.Remove(routePath);
            if (visualPath.Count < 2) return;

            // Create a StreamGeometry to use to specify myPath.
            StreamGeometry geometry = new StreamGeometry();

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(visualPath[0], false, false);
                ctx.PolyLineTo(visualPath, true, true);
            }

            // Freeze the geometry for performance reasons
            geometry.Freeze();

            routePath = new Path()
            {
                Data = geometry,
                Effect = new BlurEffect()
                {
                    KernelType = KernelType.Gaussian,
                    Radius = 2.0,
                    RenderingBias = RenderingBias.Quality
                },
                Stroke = PathColor,
                StrokeThickness = 3,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Opacity = 0.6,
                IsHitTestVisible = false,
            };
            MarkerLayer.Children.Add(routePath);
        }

        #endregion

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                tbMapInfo.Text = "Bing Maps Control";
                return;
            }

            if (mapTileFactory == null) throw new Exception("MapControl cannot render without a map tile factory");

            int centerTileX, centerTileY;
            MapUtils.LatLongToPixelXY(mapCenter.X, mapCenter.Y, zoomLevel, out centerTileX, out centerTileY);
            centerTileX /= 256;
            centerTileY /= 256;
            int widthInTiles = (int)(ActualWidth / 256) +1;
            int heightInTiles = (int)(ActualHeight / 256) +1 ;
            int minHorizontalTile = (int) MapUtils.Clip(centerTileX - widthInTiles, 0, MapUtils.MapSize(zoomLevel)/256);
            int maxHorizontalTile = (int)MapUtils.Clip(centerTileX + widthInTiles, 0, MapUtils.MapSize(zoomLevel) / 256);
            int minVerticalTile = (int)MapUtils.Clip(centerTileY - heightInTiles, 0, MapUtils.MapSize(zoomLevel) / 256);
            int maxVerticalTile = (int)MapUtils.Clip(centerTileY + heightInTiles, 0, MapUtils.MapSize(zoomLevel) / 256);

            // TODO (Shy, 9/14/2010): Cancel previous tile requests (on the tile factory) to make zooming go faster (i.e. stop getting tiles for a view that we're not going to present)
            MapLayer.Children.Clear();
            for (int x = minHorizontalTile; x <= maxHorizontalTile; x++)
            {
                for (int y = minVerticalTile; y <= maxVerticalTile; y++)
                {
                    MapTile tile = mapTileFactory.GetTile(zoomLevel, x, y, MapType);

                    if (!MapLayer.Children.Contains(tile))
                    {
                        // Position tile in the map area
                        tile.Margin = new Thickness(x * 256, y * 256, 0, 0);
                        MapLayer.Children.Add(tile);
                    }
                }
            }
        }

        private void ResetView()
        {
            MapLayer.Children.Clear();
            MapCenter = mapCenter;
            MarkerLayer.Children.Clear();
            foreach (MapMarker marker in Markers)
            {
                marker.SetVisualLocation(zoomLevel);
                MarkerLayer.Children.Add(marker.MarkerControl);
            }
            RegenerateRouteShape();
            InvalidateVisual();
        }
    }
}
