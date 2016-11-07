using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.WebUI.Models
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string City { get; set; }

        public int Age { get; set; }

        public string Hobby { get; set; }
    }
}