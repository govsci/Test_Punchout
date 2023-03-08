using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Punchout.Classes
{
    public class User
    {
        public User(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}