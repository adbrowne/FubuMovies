using System;
using System.Web.Security;

namespace FubuMovies.Infrastructure
{
    public class MembershipProvider : System.Web.Security.MembershipProvider
    {
        public MembershipProvider()
        {
            
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.UserRejected;
            return null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            return "";
        }

        public override void UpdateUser(MembershipUser user)
        {
            
        }

        public override bool ValidateUser(string username, string password)
        {
            return true;
        }

        public override bool UnlockUser(string userName)
        {
            return true;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return new MembershipUser("FubuRoleProvider", null, null, null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return new MembershipUser("FubuRoleProvider", null, null, null, null, null, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public override string GetUserNameByEmail(string email)
        {
            return "admin";
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 1; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 100; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Encrypted; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 8; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return ""; }
        }
    }
    public class RoleProvider : System.Web.Security.RoleProvider
    {
        public RoleProvider()
        {
            
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            return new string[0];
        }

        public override void CreateRole(string roleName)
        {
            
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return true;
        }

        public override bool RoleExists(string roleName)
        {
            return true;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
        }

        public override string[] GetUsersInRole(string roleName)
        {return new string[0];
        }

        public override string[] GetAllRoles()
        {
            return new string[0];
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return new string[0];
        }


        public override string ApplicationName { get; set; }
    }
}