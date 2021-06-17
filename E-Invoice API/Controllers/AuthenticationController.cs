using AutoMapper;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.DTO.Response;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Validators;
using E_Invoice_API.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Generates and returns JWT Token for authentication.
        /// </summary>
        /// <param name="request">Request with credentials email and password</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>LoginResponse</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var validator = new LoginRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var _privateKey = _configuration.GetSection("AppSettings:PrivateKey").Value;
            if (String.IsNullOrEmpty(_privateKey))
            {
                throw new ControllerException(ErrorCodes.PrivateKeyNotFound, "Controller couldn't retrieve private key");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    var key = TokenHelper.BuildRsaSigningKey(_privateKey);
                    var token = TokenHelper.GenerateToken(user.Id, key, _dateTimeProvider);
                    var loggedUser = _mapper.Map<LoginResponse>(user);
                    loggedUser.Token = token;

                    return Ok(loggedUser);
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
