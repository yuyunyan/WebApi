using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Mail.MailTemplates
{
    public class Quote
    {
        public static string QuoteAttachment(string recipientName, string senderName, string messageBody, bool showSignatureLogo = true)
        {
            return @"<body><p>Dear " + recipientName + @",</p>"
                + "<p>" + messageBody + "</p>" //@"<p>Please see the attached PDF quote from Sourceability.</p>
                 + @"<p>Warm Regards,</p>
                  <p>" + senderName + "</p>"
                  + (showSignatureLogo ? "<p><img src='https://sourceability.com/wp-content/uploads/2018/01/primary-logo-tricolor-text-transparent-background-1-5-300x142.png' alt='Sourceability' width='167' height='79'></p></body>" : "");
        }
    }
}