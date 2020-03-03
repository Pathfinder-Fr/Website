using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Sueetie.Core;




public partial class admin_TokenUsers : System.Web.UI.Page
{

    public class TokenUser
    {
        public int id;
        public string name;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        List<TokenUser> TokenUsers = GetTokenUsers();
       Response.Write(JsonConvert.SerializeObject(TokenUsers));
    }

    public List<TokenUser> GetTokenUsers()
    {
        List<SueetieUser> _sueetieUsers = SueetieUsers.GetSueetieUserList(SueetieUserType.RegisteredUser, false);
        string q = Request.QueryString["q"];

        List<TokenUser> tokenUsers = new List<TokenUser>();
        foreach (SueetieUser _sueetieUser in _sueetieUsers)
        {
            string uname = _sueetieUser.DisplayName + " (" + _sueetieUser.UserName + ")";
            if (uname.IndexOf(q) == 0 || uname.IndexOf(q) > 0)
                tokenUsers.Add(new TokenUser { id = _sueetieUser.UserID, name = uname });
        }
        return tokenUsers;
    }
}

