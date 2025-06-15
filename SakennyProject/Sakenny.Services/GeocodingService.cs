using System;
using System.Collections.Generic;
using System.Linq; // هتضيف دي عشان تستخدم متدات LINQ زي .Select() و .ToArray()
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // تأكد إن الحزمة دي متثبتة من NuGet

public class GeocodingService
{
    private readonly string _apiKey;
    private static readonly HttpClient _client = new HttpClient(); // استخدام HttpClient واحد لكل عمر التطبيق

    public GeocodingService(string apiKey)
    {
        _apiKey = apiKey;
    }

    private async Task<string> GetGovernorateForSingleCoordinate(double latitude, double longitude)
    {
        string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&language=ar&key={_apiKey}";

        try
        {
            var response = await _client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode(); 
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());

            var status = json["status"]?.ToString();
            if (status != "OK")
            {
                return $"API Error ({status}) for ({latitude},{longitude})";
            }

            var results = json["results"] as JArray;
            if (results?.Any() != true) // .Any() بيتحقق لو المصفوفة مش فاضية
            {
                return $"No results for ({latitude},{longitude})";
            }

            // بنبحث عن administrative_area_level_1 (المحافظة)
            var governorateComponent = results[0]["address_components"]?
                .Children<JObject>()
                .FirstOrDefault(c => c["types"]?.Children<JToken>().Any(t => t.ToString() == "administrative_area_level_1") == true);

            if (governorateComponent != null)
            {
                return governorateComponent["long_name"]?.ToString();
            }
            return "";
        }
        catch (HttpRequestException ex)
        {
            return $"HTTP Request Failed ({latitude},{longitude}): {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"An unexpected error occurred ({latitude},{longitude}): {ex.Message}";
        }
    }
    public async Task<Dictionary<string, int>> GetGovernoratesForCoordinatesList(List<(double Latitude, double Longitude)> coordinatesList)
    {
        var tasks = coordinatesList.Select(async coord =>
        {
            var governorate = await GetGovernorateForSingleCoordinate(coord.Latitude, coord.Longitude);
            return ($"{coord.Latitude},{coord.Longitude}", governorate); // بنرجع الإحداثي والمحافظة
        }).ToList();

        var results = await Task.WhenAll(tasks);
       var data = new Dictionary<string,int>();
        List<(string, string)> a = new();
        foreach(var temp in results)
        {
            string aa= temp.ToTuple().Item1;
            string bb = temp.ToTuple().Item2;
            a.Add((aa, bb));

        }
        return data;
    }
}