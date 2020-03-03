using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Sueetie.Web.admin
{
    /// <summary>
    /// Summary description for Migrate
    /// </summary>
    public class Migrate : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            HashAllPasswords();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        void HashAllPasswords()
        {
            var clearProvider = Membership.Providers["SqlMembershipProvider"];
            var hashedProvider = Membership.Providers["SqlMembershipProvider_Hash"];
            int dontCare;
            if (clearProvider == null || hashedProvider == null) return;
            var passwords = clearProvider.GetAllUsers(0, int.MaxValue, out dontCare)
                .Cast<MembershipUser>().Where(u => u.IsLockedOut == false && u.IsApproved).ToDictionary(u => u.UserName, u => u.GetPassword());

            using (var conn = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["SueetieConnectionString"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                       "UPDATE [aspnet_Membership] SET [PasswordFormat]=1", conn))
                    cmd.ExecuteNonQuery();
            }

            foreach (var entry in passwords)
            {
                var resetPassword = hashedProvider.ResetPassword(entry.Key, null);
                hashedProvider.ChangePassword(entry.Key, resetPassword, entry.Value);
            }
        }
    }
}