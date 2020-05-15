using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Models
{
    public class Coupon
    {
        [Key]
        public int code { get; set; }
        public int discountPercent { get; set; }
    }
}
