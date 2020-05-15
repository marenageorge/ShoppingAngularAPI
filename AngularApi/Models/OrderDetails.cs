using System;
using System.Collections.Generic;

namespace AngularApi.Models
{
    public partial class OrderDetails
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public short Quantity { get; set; }
       
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}
