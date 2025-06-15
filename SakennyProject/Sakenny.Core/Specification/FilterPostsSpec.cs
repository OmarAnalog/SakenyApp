using Sakenny.Core.Specification.SpecParam;

namespace Sakenny.Core.Specification
{
    public class FilterPostsSpec:BaseSpecParam
    {
        public string? UnitType { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public string? RentFrequency { get; set; }
        public bool? Furnished { get; set; }
        public int? Floor { get; set; }
        public int? Beds { get; set; }
        public int? Baths { get; set; }
        public int? Rooms { get; set; }
        public string? Gender { get; set; }
        public string? Purpose { get; set; }
/*        int? beds;
        int? baths;
        int? rooms;
        String? gender; // male - female - any
        String? purpose; // buy - rent - any*/
    }
}

