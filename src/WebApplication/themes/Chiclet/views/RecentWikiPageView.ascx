<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.WikiPageView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
 <script runat="server" language="C#">

    void Page_Load()
    {
        if (!string.IsNullOrEmpty(WikiPage.Keywords))
        {
            ltKeywords.Visible = true;
            ltKeywords.Text = string.Format("<div class='ViewItemWikiKeywordCat'><span>Keywords: </span>{0}</div>", WikiPage.Keywords.Replace(",",", "));
        }
        if (!string.IsNullOrEmpty(WikiPage.Categories))
        {
            string comma = ", ";
            string catlinks = string.Empty;
            string[] cats = WikiPage.Categories.Split('|');
            for (int i = 0; i < cats.Length; i++)
            {
                if (i == cats.Length - 1)
                    comma = string.Empty;
                
                if (!string.IsNullOrEmpty(cats[i]))
                    catlinks += string.Format("<a href='/wiki/allpages.aspx?cat={0}'>{0}</a>{1}", cats[i], comma);
            }
            ltCategories.Visible = true;
            ltCategories.Text = string.Format("<div class='ViewItemWikiKeywordCat'><span>Categories: </span>{0}</div>", catlinks);
        }     
        string wikiAbstract = WikiPage.Abstract;
        if (!string.IsNullOrEmpty(wikiAbstract))
        {
            ltDescription.Visible = true;
            ltDescription.Text = string.Format("<div class='ViewItemDescription'>{0}</div>", DataHelper.TruncateText(wikiAbstract, 150));
        }
            
    }
    
</script>

<div id="ViewItem" class="ViewItem">
  <div class="ViewItemTitle"><a href="<%= WikiPage.Permalink %>"><%= WikiPage.PageTitle %></a></div>
  <asp:Literal ID="ltDescription" runat="server" Visible="false" />
  <asp:Literal ID="ltKeywords" runat="server" Visible="false" />
    <asp:Literal ID="ltCategories" runat="server" Visible="false" />
  <div class="ViewItemAuthorDate">By <a href="/members/profile.aspx?u=<%= WikiPage.UserID %>"><%= WikiPage.DisplayName%></a> on <%= WikiPage.DateTimeModified.ToLongDateString()%></div>
</div>