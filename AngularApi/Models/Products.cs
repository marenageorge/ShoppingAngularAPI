using System;
using System.Collections.Generic;

namespace AngularApi.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Quantity { get; set; }
        public bool Isdeleted { get; set; } = false;
        public decimal? Price { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
