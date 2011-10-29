using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCEmergency.Common;

namespace WCEmergency.DataLayer
{
    public class ToiletMapper
    {
        public static IList<Common.Toilet> Map(IList<Toilet> list)
        {
            return list.Select(toilet => new Common.Toilet()
                                             {
                                                 Id = toilet.Id, Coordinate = new Coordinate()
                                                                                  {
                                                                                      X = toilet.CoordinateX, Y = toilet.CoordinateY
                                                                                  }, Description = toilet.Description, Name = toilet.Name, Picture = toilet.Picture, Rate = toilet.Rate, Sex = (Sex) toilet.Sex
                                             }).ToList();
        }

        public static Toilet Map(Common.Toilet toilet)
        {
            return new Toilet()
            {
                Id = toilet.Id,
                CoordinateX = toilet.Coordinate.X,
                CoordinateY = toilet.Coordinate.Y,
                Description = toilet.Description,
                Name = toilet.Name,
                Picture = toilet.Picture,
                Rate = toilet.Rate,
                Sex = (int)toilet.Sex,
                DateAdded = DateTime.Now
            };
        }
    }
}
