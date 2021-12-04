using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VulnerableAppForWebinar.Dto.Account;
using VulnerableAppForWebinar.Dto.Others;
using VulnerableAppForWebinar.Entity.Account;
using VulnerableAppForWebinar.Repository.Account;
using VulnerableAppForWebinar.Utility.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly IMapper _mapper;

        private readonly AccountRepository _accountRepository;
        private readonly JWTAuthManager _jwtAuthManager;

        public AccountController(AccountRepository accountRepository , JWTAuthManager jwtAuthManager)
        {
            _accountRepository = accountRepository;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var account = await _accountRepository.GetAccountByEmailPassword(request.Email, request.Password);
            if (account is null)
            {
                return BadRequest( new ErrorResponse() { Message = "There is and error while login process. Please control your email or password"} );
            }

            var token = _jwtAuthManager.GenerateTokens(account);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] AccountEntity request)
        {

            var account = await _accountRepository.GetAccountByEmail(request.Email);
            if (account != null)
            {
                return BadRequest(new ErrorResponse() { Message = "The email address which you provided is using another user." });
            }

            if (request.Role == "") { request.Role = "User"; }

            await _accountRepository.CreateAccount(request);

            AccountResponse accres = new AccountResponse
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
            };

            return Ok(accres);
        }


        [Authorize(Roles ="Admin,User")]
        [HttpGet]
        public async Task<ActionResult> GetAccount([FromQuery] string email)
        {
            //email adresindeki kullanıcıları getir.
            var account = await _accountRepository.GetAccountByEmail(email);
            if (account is null)
            {
                return BadRequest(new ErrorResponse { Message = "There is no profile using email address whhich provided" });
            }
            return Ok(new AccountResponse { Name = account.Name , Surname = account.Surname , Email = account.Email});
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<ActionResult> UpdateNameSurnameAccount([FromBody] UpdateNameSurnameRequest request)
        {
            var email = _jwtAuthManager.TakeEmailFromJWT(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var account = await _accountRepository.GetAccountByEmail(email);
            if (account is null)
            {
                return BadRequest(new ErrorResponse { Message = "There is an error while getting Acccount information" });
            }

            var updated = await _accountRepository.UpdateNameSurname(account.Id, new AccountEntity { 
                Id = account.Id,
                Name = request.Name, 
                Surname = request.Surname ,
                Email = account.Email,
                Password = account.Password,
                CreatedAt = account.CreatedAt,
                Role = account.Role
            });
            return Ok(new SuccessResponse { Message = "Account's Name and Surname are updated"});
        }


        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ActionResult> GetAccounts()
        {
            var account = await _accountRepository.GetAccounts();
            if (account is null)
            {
                return BadRequest(new ErrorResponse { Message = "There is no profile" });
            }
            List<AccountResponse> accountResponses = new List<AccountResponse>();
            foreach (var item in account)
            {
                accountResponses.Add(new AccountResponse { Name = item.Name , Surname = item.Surname , Email = item.Email});
            }
            return Ok(accountResponses);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAccount([FromQuery] string email)
        {
            var account = await _accountRepository.GetAccountByEmail(email);
            if(account is null)
            {
                return BadRequest(new ErrorResponse { Message = "There is no account with start email address which you entered" });
            }
            var result = _accountRepository.DeleteAccount(email);
            return Ok(new SuccessResponse { Message = "User which you provided is deleted" });
        }

    }
}
