using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCEmergency.Common;

namespace WCEmergency.DataLayer
{
    public class DataToPocoMapper
    {
        public IList<Common.Toilet> Map(IList<Toilet> list)
        {
            IList<Common.Toilet> result = new List<Common.Toilet>();
            foreach (var toilet in list)
            {
                result.Add(new Common.Toilet() {Id = toilet.Id, Coordinate = new Coordinate(){X = toilet.CoordinateX, Y = toilet.CoordinateY},
                    Description = toilet.Description, Name = toilet.Name, Picture = toilet.Picture, Rate = toilet.Rate, Sex = (Sex)toilet.Sex
                }); 
            }
            return result;
        }

    }
}
