// --------------------------------------------------------------------------
// <copyright file="EmailService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.29">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Services.Impl
{
    using System.Configuration;
    using System.Text.RegularExpressions;
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;

    /// <summary>
    /// Email Service.
    /// </summary>
    public static class EmailService
    {
        /// <summary>
        /// Function to send mail.
        /// </summary>
        /// <param name="mailTo">Input Mail to.</param>
        /// <param name="mailCc">Input Mail Cc.</param>
        /// <param name="mailBcc">Input Mail Bcc.</param>
        /// <param name="subject">Subject of mail.</param>
        /// <param name="body">Body of mail.</param>
        public static void SendEmail(string mailTo, string mailCc, string mailBcc, string subject, string body)
        {
            if (!IsValidEmailList(mailTo, true) || !IsValidEmailList(mailCc) || !IsValidEmailList(mailBcc))
            {
                return;
            }

            var mailItem = new MailRecord
            {
                MailTo = mailTo,
                MailCc = mailCc,
                MailBcc = mailBcc,
                Subject = subject,
                Title = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Email.SenderName"]) 
                ? string.Empty 
                : ConfigurationManager.AppSettings["Email.SenderName"],
                Body = body
            };

            using (var client = new MailDeliver())
            {
                client.SendMail(mailItem);
            }
        }

        /// <summary>
        /// Function to check valid email list.
        /// </summary>
        /// <param name="emailList">Email list.</param>
        /// <param name="hasCheckEmpty">Check empty or no.</param>
        /// <returns>True or False.</returns>
        public static bool IsValidEmailList(string emailList, bool hasCheckEmpty = false)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(emailList))
            {
                if (hasCheckEmpty)
                {
                    isValid = false;
                }
            }
            else
            {
                var arrEmail = emailList.Split(';');
                foreach (var email in arrEmail)
                {
                    if (!IsValidEmail(email))
                    {
                        return false;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Function to check valid a email.
        /// </summary>
        /// <param name="email">Email need check.</param>
        /// <returns>True or False.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email);
        }
    }
}
