using Sakenny.Core.Models;

namespace Sakenny.Core.Specification
{
    public class PostWithUnitAndUserWithFiltersSpec:BaseSpecification<Post>
    {
        public PostWithUnitAndUserWithFiltersSpec(FilterPostsSpec model): base(p => !p.ISDeleted &&
        (model.UnitType == null || (model.UnitType.ToLower() == "house" && ((int)p.Unit.UnitType == 2 || (int)p.Unit.UnitType == 5)) ||
        (model.UnitType.ToLower() == "room" && ((int)p.Unit.UnitType == 0)) || (model.UnitType.ToLower() == "apartment" && ((int)p.Unit.UnitType == 1 || (int)p.Unit.UnitType == 4)) ||
        (model.UnitType.ToLower() == "studentapartment" && ((int)p.Unit.UnitType == 3))
        ) &&
        (model.Latitude == null || (Math.Abs((decimal)(model.Longitude??0)-(decimal)p.Unit.Location.Longitude)<= (decimal)0.045 && (Math.Abs((decimal)model.Latitude - (decimal)p.Unit.Location.Latitude) <= (decimal)0.045)) ) &&
        ((model.MinPrice ?? 0) <= p.Unit.Price && (model.MaxPrice ?? (decimal)2e15) >= p.Unit.Price) &&
        ((model.MinArea ?? 0) <= p.Unit.UnitArea && (model.MaxArea ?? (decimal)2e15) >= p.Unit.UnitArea) &&
        (model.RentFrequency == null || (model.RentFrequency.ToLower() == "any") ||
        (model.RentFrequency.ToLower() == "daily" && (int)p.Unit.RentalFrequency == 0) ||
        (model.RentFrequency.ToLower() == "weekly" && (int)p.Unit.RentalFrequency == 1) ||
        (model.RentFrequency.ToLower() == "monthly" && (int)p.Unit.RentalFrequency == 2) ||
        (model.RentFrequency.ToLower() == "yearly" && (int)p.Unit.RentalFrequency == 3))

        &&
        (model.Beds == null || (model.Beds == 5 && model.Beds <= p.Unit.BedRoomCount) || (model.Beds == p.Unit.BedRoomCount)) &&
        (model.Baths == null || (model.Baths == 5 && model.Baths <= p.Unit.BathRoomCount) || (model.Baths == p.Unit.BathRoomCount)) &&
        (model.Floor == null || (model.Floor == 5 && model.Floor <= p.Unit.Floor) || (model.Floor == p.Unit.Floor)) &&
        (model.Gender == null || model.Gender.ToLower() == "any" || (model.Gender.ToLower() == "male" && (int)p.Unit.GenderType == 0) || (model.Gender.ToLower() == "female" && (int)p.Unit.GenderType == 1)) &&
        (model.Rooms == null || (model.Rooms == 5 && model.Rooms <= p.Unit.RoomsCount) || (model.Rooms == p.Unit.RoomsCount)) &&
        (model.Purpose == null || (model.Purpose.ToLower() == "any") || (model.Purpose.ToLower() == "rent" && ((int)p.Unit.UnitType == 0 || (int)p.Unit.UnitType == 1 || (int)p.Unit.UnitType == 2 || (int)p.Unit.UnitType == 3)) || (model.Purpose.ToLower() == "buy" && ((int)p.Unit.UnitType == 4 || (int)p.Unit.UnitType == 5))))
        { 
            Includes.Add(p => p.User);
            Includes.Add(p=>p.Unit);
            Includes.Add(P => P.Unit.PicutresUrl);
            ApplyPagination(model.PageSize * (model.PageIndex - 1), model.PageSize);
        }
    }
}
