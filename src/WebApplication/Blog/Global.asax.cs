﻿// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace BlogEngine
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Core;

    public class Global : HttpApplication
    {
        /// <summary>
        /// Application Error handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // Remove any special filtering especially GZip filtering
            this.Response.Filter = null;

            HttpContext context = ((HttpApplication)sender).Context;
            Exception ex = context.Server.GetLastError();
            if (ex == null || !(ex is HttpException) || (ex as HttpException).GetHttpCode() == 404)
            {
                return;
            }
            var sb = new StringBuilder();

            try
            {
                sb.AppendLine("Url : " + context.Request.Url);
                sb.AppendLine("Raw Url : " + context.Request.RawUrl);

                while (ex != null)
                {
                    sb.AppendLine("Message : " + ex.Message);
                    sb.AppendLine("Source : " + ex.Source);
                    sb.AppendLine("StackTrace : " + ex.StackTrace);
                    sb.AppendLine("TargetSite : " + ex.TargetSite);
                    ex = ex.InnerException;
                }
            }
            catch (Exception ex2)
            {
                sb.AppendLine("Error logging error : " + ex2.Message);
            }

            if (BlogSettings.Instance.EnableErrorLogging)
            {
                Utils.Log(sb.ToString());
            }
            context.Items["LastErrorDetails"] = sb.ToString();
            context.Response.StatusCode = 500;

            // Custom errors section defined in the Web.config, will rewrite (not redirect)
            // this 500 error request to error.aspx.
        }

        /// <summary>
        /// Hooks up the available extensions located in the BlogEngine folder.
        /// An extension must be decorated with the ExtensionAttribute to work.
        /// <example>
        ///  <code>
        /// [Extension("Description of the SomeExtension class")]
        /// public class SomeExtension
        /// {
        ///   //There must be a parameterless default constructor.
        ///   public SomeExtension()
        ///   {
        ///     //Hook up to the BlogEngine.NET events.
        ///   }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            Utils.LoadExtensions();
        }

        /// <summary>
        /// Sets the culture based on the language selection in the settings.
        /// </summary>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var culture = BlogSettings.Instance.Culture;
            if (!string.IsNullOrEmpty(culture) && !culture.Equals("Auto"))
            {
                CultureInfo defaultCulture = Utils.GetDefaultCulture();
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
                Thread.CurrentThread.CurrentCulture = defaultCulture;
            }
        }
    }
}