using Sakenny.Core.Models;
using Sakenny.Repository.Migrations;
using SakennyProject.DTO;

namespace SakennyProject
{
    public static class MappingModels
    {
        public static Unit MapUnit(AddPostDto model,string userId)
        {
            var unit = new Unit()
            {
                Title = model.Title,
                Description = model.Description,
                Floor = model.Floor,
                RoomsCount = model.RoomsCount,
                BedRoomCount = model.BedRoomCount,
                BathRoomCount = model.BathRoomCount,
                UnitArea = model.Area,
                IsFurnished = model.IsFurnished,
                Price = model.Price,
                RentalFrequency = model.RentalFrequency,
                NearbyServices = model.NearbyServices,
                UnitType = model.UnitType,
                UnitServices = model.UnitServices,
                UserId = userId,
                Location = model.Location,
                Address=model.Address,
                GenderType=model.GenderType
            };
            return unit;
        }
        public static Post MapPost(AddPostDto model,int unitId, string userId)
        {
            var post = new Post()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow,
                LikesCount = 0,
                CommentsCount = 0,
                UserId = userId,
                UnitId = unitId,
            };
            return post;
        }
    }
}
