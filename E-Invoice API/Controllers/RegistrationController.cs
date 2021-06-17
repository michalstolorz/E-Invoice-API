using AutoMapper;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Validators;
using E_Invoice_API.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegistrationController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegistrationController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Register new user to the system
        /// </summary>
        /// <param name="request">Request with new user email, password, first name, last name and phone number</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Created user id</returns>
        [HttpPost("registerUser")]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            var validator = new RegisterRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var userToCreate = _mapper.Map<User>(request);

            var creatingUserResult = await _userManager.CreateAsync(userToCreate, request.Password);
            if (!creatingUserResult.Succeeded)
            {
                return BadRequest(creatingUserResult.Errors);
            }

            return Ok(userToCreate.Id);
        }
    }
}
