using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.WebUI.Areas.Admin.Models
{
    public class MemberViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }
    }
}