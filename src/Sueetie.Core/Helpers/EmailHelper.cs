// -----------------------------------------------------------------------
// <copyright file="EmailHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;

    public static class EmailHelper
    {
        public static void AsyncSendEmail(MailMessage message)
        {
            ThreadPool.QueueUserWorkItem(delegate { TrySendMessage(message); });
        }

        public static void AsyncSendEmail(string recipient, string sender, string subject, string body, bool html)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                var message = new MailMessage(sender, recipient, subject, body);
                message.IsBodyHtml = html;
                TrySendMessage(message);
            });
        }

        public static string[] GetRecipients(List<SueetieUser> users)
        {
            if (users == null) return new string[0];

            var result = new string[users.Count];
            var i = 0;
            foreach (var user in users)
            {
                result[i] = user.Email;
            }

            return result;
        }

        public static void AsyncSendMassEmail(string[] recipients, string sender, string subject, string body, bool html)
        {
            if (recipients.Length == 0) return;

            ThreadPool.QueueUserWorkItem(delegate
            {
                var message = new MailMessage(new MailAddress(sender), new MailAddress(sender));
                message.Subject = subject;
                message.Body = body;
                for (var i = 0; i < recipients.Length; i++)
                {
                    message.Bcc.Add(new MailAddress(recipients[i]));
                }
                message.IsBodyHtml = html;
                TrySendMessage(message);
            });
        }

        public static void NotifyError(Exception ex, string url)
        {
            try
            {
                var recipients = SiteSettings.Instance.ErrorEmails.Replace(" ", string.Empty).Split(',');

                if (recipients.Length > 0)
                {
                    AsyncSendMassEmail(recipients, SiteSettings.Instance.FromEmail, "Error Notification", "An error occurred at " +
                                                                                                          SiteSettings.Instance.SiteName + " on " + DateTime.Now.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss") + " (server time)" +
                                                                                                          " - server stack trace follows.\r\n\r\n" +
                                                                                                          (!string.IsNullOrEmpty(url) ? url + "\r\n\r\n" : "") +
                                                                                                          ex, false);
                }
            }
            catch
            {
            }
        }

        private static void TrySendMessage(MailMessage message)
        {
            try
            {
                GenerateSmtpClient().Send(message);
            }
            catch (Exception ex)
            {
                if (ex is SmtpException)
                {
                    SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.EmailException, "SMTP ERROR: " + ex.Message);
                }
                else SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.EmailException, "EMAIL ERROR: " + ex.Message);
            }
        }

        private static SmtpClient GenerateSmtpClient()
        {
            var client = new SmtpClient(SiteSettings.Instance.SmtpServer);
            if (!string.IsNullOrEmpty(SiteSettings.Instance.SmtpUserName))
            {
                client.Credentials = new NetworkCredential(SiteSettings.Instance.SmtpUserName, SiteSettings.Instance.SmtpPassword);
            }
            client.EnableSsl = SiteSettings.Instance.EnableSSL;
            if (!string.IsNullOrEmpty(SiteSettings.Instance.SmtpServerPort)) client.Port = int.Parse(SiteSettings.Instance.SmtpServerPort);
            else if (SiteSettings.Instance.EnableSSL) client.Port = 465;
            return client;
        }
    }
}