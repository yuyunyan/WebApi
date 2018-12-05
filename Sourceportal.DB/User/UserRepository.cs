using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Sourceportal.Domain.Models.API.Requests;
using System.Collections.Generic;
using Sourceportal.Utilities;

namespace Sourceportal.DB.User
{
    public class UserRepository : IUserRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public static List<Domain.Models.DB.User> GetUserList(bool isenabled, int startrow, int endrow, string searchString)
        {
            List<Domain.Models.DB.User> userlist = new List<Domain.Models.DB.User>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@RowOffset", startrow);
                param.Add("@RowLimit", endrow);
                param.Add("@IsEnabled", isenabled? (bool?)isenabled: null);
                param.Add("@SearchString", searchString);
                //result is list of CustomTest
                userlist = con.Query<Domain.Models.DB.User>("uspUserListGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return userlist;
        }

        public static List<Domain.Models.DB.User> GetBuyerList(bool sortByFirstName)
        {
            var buyerList = new List<Domain.Models.DB.User>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SortByFirstName", sortByFirstName);
                buyerList = con.Query<Domain.Models.DB.User>("uspBuyersGet",param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return buyerList;
        }

        public bool SetAccountStatus(int userId, bool isEnabled)
        {
            bool success = false;
            int status = 0;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", userId);
                param.Add("@isenabled", isEnabled);
                param.Add("@ModifiedBy", 1);
                param.Add("@ret", status, direction: ParameterDirection.ReturnValue);
                //result is list of CustomTest
                con.Query("uspUserSet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                success = (status == 0);
                con.Close();
            }

            return success;
        }
        
        public static Domain.Models.DB.User Login(string emailaddress, string password)
        {
            Sourceportal.Domain.Models.DB.User usr = new Sourceportal.Domain.Models.DB.User();
            //hash password
            string PasswordHash = GetSHA1HashData(password);

            //Attempt Login
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmailAddress", emailaddress);
                param.Add("@PasswordHash", PasswordHash);
                param.Add("@ret", usr.Status, direction: ParameterDirection.ReturnValue);
                //result is list of CustomTest
                usr = con.Query<Sourceportal.Domain.Models.DB.User>("uspUserLogin", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }

            if (usr != null)
            {
                switch (usr.Status)
                {
                    //Invalid email address
                    case -1:
                        usr.Error = "Invalid email address";
                        break;

                    //Invalid Password
                    case -2:
                        usr.Error = "Invalid password";
                        break;

                    //User disabled
                    case -3:
                        usr.Error = "User disabled";
                        break;
                    default:
                        usr.Error = string.Empty;
                        break;
                }
            }
            return usr;
        }

        public bool ValidatePassword(string emailaddress, string password)
        {
            var userId = false;
            //hash password
            string PasswordHash = GetSHA1HashData(password);

            //Attempt Login
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@EmailAddress", emailaddress);
                param.Add("@PasswordHash", PasswordHash);
                //result is list of CustomTest
                var result = con.Query<int>("SELECT UserID FROM Users WHERE EmailAddress=@EmailAddress AND PasswordHash = @PasswordHash", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    userId = true;
                }
                else
                {
                    userId = false;
                }
                con.Close();

            }
            return userId;
        }

        public Domain.Models.DB.User Update(UserUpdateRequest user)
        {
            
            //hash password
            string PasswordHash = GetSHA1HashData(user.password);
            var modifiedBy = UserHelper.GetUserId();
            int Status = 0;
            Sourceportal.Domain.Models.DB.User returnedUser;
            //attempt creation
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", user.userid, direction: ParameterDirection.InputOutput);
                param.Add("@FirstName", user.firstname);
                param.Add("@LastName", user.lastname);
                param.Add("@PhoneNumber", user.phoneNumber);
                param.Add("@PasswordHash", PasswordHash);
                param.Add("@EmailAddress", user.emailaddress);
                param.Add("@OrganizationID", user.organizationid);
                param.Add("@TimezoneName", user.timezonename);
                param.Add("@isenabled", user.isenabled);
                param.Add("@ModifiedBy", modifiedBy);
                param.Add("@ret", Status, direction: ParameterDirection.ReturnValue);
                //result is list of CustomTest
                returnedUser = con
                    .Query<Domain.Models.DB.User>("uspUserSet", param, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
                con.Close();

                if (returnedUser == null)
                    returnedUser = new Domain.Models.DB.User();

                //Fail
                switch (param.Get<int>("@ret"))
                {
                    case -1:
                        returnedUser.Error = "New user creation failed";
                        break;

                    case -2:
                        returnedUser.Error = "User update failed";
                        break;

                    case -3:
                        returnedUser.Error = "Email address already used";
                        break;

                }
            }

            return returnedUser;
        }

        private static string GetSHA1HashData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }

        public  Domain.Models.DB.User GetUserData(int userId)
        {
            Sourceportal.Domain.Models.DB.User usr = new Sourceportal.Domain.Models.DB.User();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", userId);
                //result is list of CustomTest
                usr = con.Query<Sourceportal.Domain.Models.DB.User>("uspUserGet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return usr;
        }

        public static List<Domain.Models.DB.UserRole> GetRoleList(string searchString)
        {
            List<Domain.Models.DB.UserRole> roleList = new List<Domain.Models.DB.UserRole>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchString);
                roleList = con.Query<Domain.Models.DB.UserRole>("uspRolesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return roleList;
        }

        public int GetUserIdFromExternal(string externalId)
        {
            int userId = 0;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", externalId);

                var result = con.Query<int>("SELECT UserID FROM Users WHERE ExternalID=@ExternalID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    userId = result.First();
                }

                con.Close();
            }

            return userId;
        }

        public Dictionary<int, string> GetUserNamesForIds(List<int> userIds)
        {
            var userData = new  Dictionary<int, string>();

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (var userId in userIds)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    var result = con.Query<string>("SELECT FirstName+ ' ' + LastName FROM Users WHERE UserId=@UserId", param, commandType: null);
                    if (result != null && result.Count() > 0)
                    {
                        userData.Add(userId, result.First());
                    }
                }

                con.Close();
            }

            return userData;
        }
    }
}
