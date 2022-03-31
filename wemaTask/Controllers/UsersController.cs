using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;
using WebApi.Services;

namespace wemaTask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private string ddlLength;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("api/Login")]
        public IActionResult Authenticate(Login model)
        {
            var response = _userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("api/Register")]
        public IActionResult Register(Register model)
        {
            _userService.Register(model);

            // gen 4 digit random number from 0-9
            //string numbers = "1234567890";

            //string characters = numbers;
            //if (ddlLength == "1")
            //{
            //    characters += numbers;
            //}
            //int length = int.Parse(ddlLength.ToString());
            string otp = string.Empty;
            //for (int i = 0; i < length; i++)
            //{
            //    string character = string.Empty;
            //    do
            //    {
            //        int index = new Random().Next(0, characters.Length);
            //        character = characters.ToCharArray()[index].ToString();
            //    } while (otp.IndexOf(character) != -1);
            //    otp += character;
            //}

            //send theb otp has respone
            return Ok(new { iSuccess=true, message = "Registration is successful kindly enter otp send to your number or email for:"+otp+"to complete process" });
        }

        [HttpGet("api/users")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("api/{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("api/{id}")]
        public IActionResult Update(int id, UpdateUsers model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("api/{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
