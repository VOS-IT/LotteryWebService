using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryWebService
{
    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string Nationality { get; set; }

        public string IDType { get; set; }

        public string IdNo { get; set; }

        public string Address { get; set; }

        public string State { get; set; }
        public string City { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }
        public string Error { get; set; }
    }
}