﻿namespace BuildSchool.MvcSolution.OnlineStore.Models.Models
{
    public class OrderDetails
    {
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}