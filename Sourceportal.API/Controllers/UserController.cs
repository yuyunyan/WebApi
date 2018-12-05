using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Sourceportal.Domain.Models.API.Requests;
using SourcePortal.Services.User;
using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB;

namespace Sourceportal.API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("api/user/getroleList")]
        [Authorize]
        [HttpGet]
        public Response<List<UserRoleResponse>> GetRoleList(string searchString)
        {
            List<Sourceportal.Domain.Models.DB.UserRole> dbrolelist = DB.User.UserRepository.GetRoleList(searchString);
            return UserService.ApiUserRoleList(dbrolelist);
        }

        [Authorize]
        [Route("api/user/userList")]
        [HttpGet]
        public List<UserDetailsResponse> UserList(bool isenabled, int startrow, int endrow,string searchString)
        {
            List<Sourceportal.Domain.Models.DB.User> dbuserlist = DB.User.UserRepository.GetUserList(isenabled, startrow, endrow,searchString);
            return UserService.APIUserList(dbuserlist);
        }

        [Authorize]
        [Route("api/user/buyerList")]
        [HttpGet]
        public List<UserDetailsResponse> GetBuyerList(bool sortByFirstName)
        {
            List<Sourceportal.Domain.Models.DB.User> dbBuyerlist = DB.User.UserRepository.GetBuyerList(sortByFirstName);
            return UserService.APIUserList(dbBuyerlist);
        }

        [Authorize]
        [Route("api/user/updateAccountSatus")]
        [HttpPost] 
        public bool SetAccountStatus(UserSetStatusRequet setStatusRequet)
        {
           bool success = _userService.SetAccountStatus(setStatusRequet.userId, setStatusRequet.isEnabled);
            return success;
        }

        [Authorize]
        [Route("api/user/getuser")]
        [HttpGet]
        public UserDetailsResponse GetUser(int userId)
        {    
            var result = _userService.GetUser(userId);
            return _userService.GetUser(userId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/create")]
        public UserDetailsResponse Create(UserUpdateRequest userUpdateRequest)
        {
            var apiUser= _userService.UpdateUser(userUpdateRequest);
            return apiUser;
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/update")]
        public UserDetailsResponse Update(UserUpdateRequest userUpdateRequest)
        {
            var apiUser = _userService.UpdateUser(userUpdateRequest);
            return apiUser;
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/validate")]
        public bool ValidatePassword(ValidatePasswordRequest request)
        {
            var valid = _userService.ValidatePassword(request.emailaddress, request.password);
            return valid;
        }


        #region examples
        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToString());
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }
        #endregion
    }
}
