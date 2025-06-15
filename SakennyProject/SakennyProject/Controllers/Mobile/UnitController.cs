using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Repository;
using SakennyProject.DTO;
using System.Security.Claims;
using Sakenny.Core.Specification.SpecParam;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IGenericRepository<Unit> unitRepository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly RatingRepository ratingRepository;

        public UnitController(IGenericRepository<Unit> unitRepository,IMapper mapper
            ,UserManager<User> userManager
            ,RatingRepository ratingRepository)
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.ratingRepository = ratingRepository;
        }
        [HttpGet("unit/{id}")]
        public async Task<ActionResult<UnitDto>> GetUnit(int id)
        {
            var unitSpec = new UnitWithProperites(id);
            var unit = await unitRepository.GetByIdWithSpecAsync(unitSpec);
            if (unit == null) return BadRequest("No Unit with that Id");
            var mappedUnit = mapper.Map<UnitDto>(unit);
            return Ok(mappedUnit);
        }
        [HttpPost("addrate")]
        [Authorize]
        public async Task<ActionResult<UnitDto>> AddRate(EditRateDto model)
        {
            var ratingUserId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (ratingUserId== null) return Unauthorized("User Not Found");
            var unit = await unitRepository.GetByIdAsync(model.UnitId);
            if (unit == null) return BadRequest("No Unit with that Id");
            var RateSpec=new RatingWithRatedUserAndUnitSpec(new RatingSpec
            {
                UnitId = model.UnitId,
                RatingId = ratingUserId
            });
            var rating = await ratingRepository.GetByIdWithSpecAsync(RateSpec);
            var ratedUser = await userManager.FindByIdAsync(unit.UserId);
            if (ratedUser == null || unit.UserId != model.RatedUserId) return BadRequest("No User with that Id");
            if (rating is null)
            { // first time he rates
                var rate = new Rating
                {
                    UnitId = model.UnitId,
                    Rate = model.Rate,
                    RatedUserId = model.RatedUserId,
                    RatingUserId = ratingUserId
                };
                if (await ratingRepository.AddRating(rate,unit,ratedUser) is null)
                    return BadRequest("Couldn't Add Rate");
                return Ok("Rate was added"); // return the added rate
            }
            else
            {
                // rating exist
                if (await ratingRepository.UpdateRating(rating,model.Rate) is null)
                    return BadRequest("Couldn't Update Rate");
                return Ok("Rate was Updated"); // return the added rate
            }
        }
    }
}
