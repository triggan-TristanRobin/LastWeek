using System;

namespace LastWeek.Model
{
    public static class UserRole
    {
        public const string Basic = "Basic";
        public const string Manager = "Manager";
        public const string Admin = "Admin";

        public static bool IsRoleHigher(string higherRole, string otherRole, bool strictlyHigher = true)
        {
            if (Enum.TryParse<Role>(higherRole, out var higherRoleEnum))
            {
                if (!Enum.TryParse<Role>(otherRole, out var otherRoleEnum))
                {
                    otherRoleEnum = Role.Basic;
                }

                return higherRoleEnum > otherRoleEnum || !strictlyHigher && higherRoleEnum == otherRoleEnum;
            }
            throw new ArgumentException($"The role provided doesn't translate to existing role ({higherRole})");
        }
    }

    public enum Role
    {
        Basic = 0,
        Manager,
        Admin
    }
}