<%@ WebHandler Language="C#" Class="Sueetie.Web.Download" %>

using System;
using System.Web;
using Sueetie.Core;
using System.IO;

namespace Sueetie.Web
{
    public class Download : IHttpHandler
    {
   
        public void ProcessRequest(HttpContext context)
        {
            HttpContext ctx = HttpContext.Current;
            try
            {
                System.IO.FileStream oFile = new System.IO.FileStream("/markeplace/files/1/dummy.zip", System.IO.FileMode.Open);
                ctx.Response.Charset = "";
                ctx.Response.ContentType = "application/zip";
                BinaryReader br = new BinaryReader(oFile);
                for (long l = 0; l < oFile.Length; l++)
                {
                    ctx.Response.OutputStream.WriteByte(br.ReadByte());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                ctx.Response.StatusCode = 404;
            }


        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

    }
}