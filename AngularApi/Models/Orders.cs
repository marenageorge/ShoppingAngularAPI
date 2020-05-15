using System;
using System.Collections.Generic;

namespace AngularApi.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public int Status { get; set; }
      
        public DateTime OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool Isdeleted { get; set; } = false;
        public string UserId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
