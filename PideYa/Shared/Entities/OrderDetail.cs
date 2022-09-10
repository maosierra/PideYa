using System;
namespace PideYa.Shared.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        public float SubTotal { get; set; }
    }
}

