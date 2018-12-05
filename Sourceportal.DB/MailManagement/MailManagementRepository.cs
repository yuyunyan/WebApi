using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.DB.MailManagementService;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.DB.ErrorManagement;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.MailManagementService
{
    public class MailManagementRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public static int LogEmail(string fromEmail, string fromName, string toEmail, string subject, string body, string[] ccEmails = null, string[] bccEmails = null, string attachmentFilePath = null, bool isBodyHtml = true)
        {
            int ret = 0;
            string ccEmailList = ccEmails != null? String.Join(",", ccEmails) : null;
            string bccEmailList = bccEmails != null ? String.Join(",", bccEmails) : null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@EmailFrom", fromEmail);
                param.Add("@FromName", fromName);
                param.Add("@EmailTo", toEmail);
                param.Add("@CC", ccEmailList);
                param.Add("@BCC", bccEmailList);
                param.Add("@MailSubject", subject);
                param.Add("@MailBody", body);
                param.Add("@AttachmentFilePath", attachmentFilePath);

                ret = con.Query<int>("uspLogMailSentSet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                
                con.Close();
                return ret;
            }
        }

        public static int LogEmailSent(int LogID, bool success, string errorMessage, int? errorCode)
        {
            int ret = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@LogID", LogID);
                param.Add("@Success", success);
                param.Add("@errorMessage", errorMessage);
                param.Add("@StatusCode", errorCode);

                ret = con.Query<int>("uspLogMailSentSet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
                return ret;
            }
        }


    }
}
