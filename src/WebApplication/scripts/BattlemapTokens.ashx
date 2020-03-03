<%@ WebHandler Language="C#" Class="Pathfinder.JsList" %>
namespace Pathfinder
{
    using System.IO;
    using System.Web;
    
    public class JsList : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            
            var folder = context.Server.MapPath("/Wiki/public/Upload/Battlemaps");

            WriteLine(context.Response, "var tokens = {");

            foreach (var directory in Directory.GetDirectories(folder))
            {
                var dirInfo = new DirectoryInfo(directory);
                WriteLine(context.Response, string.Format("\t\"{0}\": [", dirInfo.Name));

                foreach (var file in dirInfo.GetFiles("*.png"))
                {
                    WriteLine(context.Response, string.Format("\t\t\"{0}\",", Path.GetFileNameWithoutExtension(file.Name)));
                }
                
                WriteLine(context.Response, string.Format("\t],", dirInfo.Name));
            }

            WriteLine(context.Response, "};");
        }
        
        private static void WriteLine(HttpResponse response, string text)
        {
            response.Write(text);
            response.Write('\n');
        }
    }
}