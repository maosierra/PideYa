using System;
namespace PideYa.Shared.Entities
{
    public class Dish
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public DishCategory? Category { get; set; }
    }
}

