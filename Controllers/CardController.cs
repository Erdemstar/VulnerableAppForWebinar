using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VulnerableAppForWebinar.Dto.Card;
using VulnerableAppForWebinar.Dto.Others;
using VulnerableAppForWebinar.Entity.Card;
using VulnerableAppForWebinar.Repository.Card;
using VulnerableAppForWebinar.Utility.JWT;

namespace VulnerableAppForWebinar.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly CardRepository _cardRepository;
        private readonly JWTAuthManager _jwtAuthManager;

        public CardController(CardRepository cardRepository , JWTAuthManager jWTAuthManager)
        {
            _cardRepository = cardRepository;
            _jwtAuthManager = jWTAuthManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> GetCard()
        {
            var UserId = _jwtAuthManager.TakeUserIdFromJWT(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            var card = await _cardRepository.GetCard(UserId);
            if (card is null)
            {
                return Ok(new SuccessResponse { Message = "There is no card" });
            }
            return Ok(card);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> CreateCard(CardCreateRequest request)
        {
            var UserId = _jwtAuthManager.TakeUserIdFromJWT(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            CardEntity entity = new CardEntity
            {
                UserID = UserId,
                Nickname = request.Nickname,
                Number = request.Number,
                ExpireDate = request.ExpireDate,
                Cve = request.Cve,
                Password = request.Password
            };

            await _cardRepository.CreateCard(entity);
            
            return Ok(new SuccessResponse { Message = "Card is created" });
        }

    }
}
