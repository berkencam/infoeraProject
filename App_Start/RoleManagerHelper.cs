using dal.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

public class RoleManagerHelper
{
    public static void InitializeRoles()
    {
        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));

        string[] roleNames = { "Admin", "Member", "User" };
        foreach (var roleName in roleNames)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
        }
    }
}
