using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PideYa.Shared.Entities
{
    public class DishImage
    {
        public int id { get; set; }
        public Dish Dish { get; set; }
        public string url { get; set; }
    }
}
