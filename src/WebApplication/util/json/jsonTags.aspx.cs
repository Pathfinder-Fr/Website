using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Sueetie.Core;




public partial class jsonTags : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> sueetieTags = GetSueetieTags();
        Response.Write(JsonConvert.SerializeObject(sueetieTags));
    }

    public List<string> GetSueetieTags()
    {
        List<SueetieTag> _sueetieTags = SueetieTags.GetSueetieTagList(false);

        string q = Request.QueryString["tag"].ToLower();

        List<string> _tagStrings = new List<string>();
        foreach (SueetieTag _sueetieTag in _sueetieTags)
        {
            string tag = _sueetieTag.Tag;
            if (tag.ToLower().IndexOf(q) > 0 || tag.ToLower().StartsWith(q))
                _tagStrings.Add(tag);
        }
        return _tagStrings;
    }
}

