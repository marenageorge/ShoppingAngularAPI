using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AngularApi.Models
{
    public  class Users:IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
