using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Views.Account
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

    }
}