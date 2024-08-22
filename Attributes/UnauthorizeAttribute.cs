using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserIdentity.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UnauthorizeAttribute : Attribute
    {
        public string[] Roles { get; }

        public UnauthorizeAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}