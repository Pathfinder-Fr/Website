using System.Web.UI;
namespace Sueetie.Web
{
    public class SueetieForm : System.Web.UI.HtmlControls.HtmlForm
    {
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            writer.WriteAttribute("name", this.Name);
            base.Attributes.Remove("name");

            writer.WriteAttribute("method", this.Method);
            base.Attributes.Remove("method");

            this.Attributes.Render(writer);

            base.Attributes.Remove("action");

            if (base.ID != null)
                writer.WriteAttribute("id", base.ClientID);
        }
    }
}
