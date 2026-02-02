using System.Net.Http;
using IpConsoleApp;

var filePath = Path.Combine("Data", "ips.txt");

var ips = File.ReadAllLines(filePath)
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .ToList();

using var client = new HttpClient();

var ipDataList = new List<IpData>();

foreach (var ip in ips)
{
    var data = await IpInfoService.GetIpDataAsync(client, ip);
    if (data != null)
        ipDataList.Add(data);
}

var countryStats = ipDataList
    .Where(x => !string.IsNullOrWhiteSpace(x.Country))
    .GroupBy(x => x.Country)
    .Select(g => new
    {
        Country = g.Key,
        Count = g.Count()
    })
    .OrderByDescending(x => x.Count)
    .ToList();

Console.WriteLine("IP count by country:");

foreach (var item in countryStats)
    Console.WriteLine($"{item.Country} - {item.Count}");

var topCountry = countryStats.First();

var cities = ipDataList
    .Where(x => x.Country == topCountry.Country)
    .Select(x => x.City)
    .Distinct()
    .ToList();

Console.WriteLine();
Console.WriteLine($"Country with most IPs: {topCountry.Country}");
Console.WriteLine("Cities:");

foreach (var city in cities)
    Console.WriteLine($"- {city}");
