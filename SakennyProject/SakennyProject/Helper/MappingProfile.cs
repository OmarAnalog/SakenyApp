using AutoMapper;
using Sakenny.Core.Models;
using SakennyProject.DTO;

namespace SakennyProject.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region post
            CreateMap<Post, PostDto>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.User.FirstName+" "+ src.User.LastName)) // or .FullName, depending on your User model
            .ForMember(dest=>dest.OwnerPicture,opt=>opt.MapFrom(src=>src.User.Picture))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Unit.UnitArea))
            .ForMember(dest => dest.UserRate, opt => opt.MapFrom(src => src.User.Rate))
            .ForMember(dest => dest.UserCountRate, opt => opt.MapFrom(src => src.User.CountRated))
            .ForMember(dest => dest.IsLiked, opt => opt.Ignore())
            .ForMember(dest => dest.IsFavourite, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Unit.Id = src.Unit.Id;
                dest.Unit.Address = src.Unit.Address;
                // mapping dest.unit properties
                dest.Unit.Title = src.Unit.Title;
                dest.Unit.Description = src.Unit.Description;
                dest.Unit.Floor = src.Unit.Floor;
                dest.Unit.RoomsCount = src.Unit.RoomsCount;
                dest.Unit.FrontPicture = src.Unit.FrontPicture;
                dest.Unit.BathRoomCount = src.Unit.BathRoomCount;
                dest.Unit.Price = src.Unit.Price;
                dest.Unit.UnitArea = src.Unit.UnitArea;
                dest.Unit.Rate = src.Unit.Rate;
                dest.Unit.CountRated = src.Unit.CountRated;
                dest.Unit.IsRented = src.Unit.IsRented;
                dest.Unit.IsFurnished = src.Unit.IsFurnished;
                dest.Unit.Location = src.Unit.Location;
                dest.Unit.RentalFrequency = src.Unit.RentalFrequency?.ToString();
                dest.Unit.NearbyServices = src.Unit.NearbyServices;
                dest.Unit.UnitServices = src.Unit.UnitServices;
                dest.Unit.GenderType = src.Unit.GenderType.ToString();
                dest.Unit.UnitType = src.Unit.UnitType.ToString();
                dest.Unit.BedRoomCount = src.Unit.BedRoomCount;
                dest.Unit.PicutresUrl = src.Unit.PicutresUrl.Select(p => p.Url).ToList();
            });
            // Calculated fields below might require additional logic during mapping
            #endregion
            CreateMap<User, UserProfileReturnDto>();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment,ReturnedComment>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.Picture))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest=>dest.Content,opt=>opt.MapFrom(src=>src.Content))
                .ForMember(dest => dest.UserRate, opt => opt.MapFrom(src => src.User.Rate));
            CreateMap<Unit,UnitDto>()
                .ForMember(dest=>dest.PicutresUrl, opt => opt.MapFrom(src => src.PicutresUrl.Select(p => p.Url).ToList()))
                .AfterMap((src, dest) =>
                {
                    dest.OwnerId = src.UserId;
                    dest.OwnerName = src.User.FirstName + " " + src.User.LastName;
                    dest.UserRate = src.User.Rate;
                    dest.UserCountRated=src.User.CountRated;
                    dest.UserPicture = src.User.Picture;
                    dest.UserLongitude = src.User.Longitude;
                    dest.UserLatitude = src.User.Latitude;
                    dest.UserAddress = src.User.Address;
                });
        }
    }
}
