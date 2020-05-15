using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Models
{
    public class AdminOrdersViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Status { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }

    }
}
