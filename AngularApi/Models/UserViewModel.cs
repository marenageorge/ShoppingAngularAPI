using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        public string Firstname { get; set; }
        public string UserName { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Url { get; set; }
        public string password { get; set; }
        public string email { get; set; }

    }
}
