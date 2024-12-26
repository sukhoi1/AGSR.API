using System.Net.Http.Json;
using AGSR.TestTask.ViewModels;
using AutoFixture;
using Microsoft.Extensions.Configuration;

namespace AGSR.Console;

internal class Program
{
    public static List<string> GenderValues = new List<string>
    {
        "male",
        "female",
        "other",
        "unknown"
    };

    public class AppSettings
    {
        public string ApiUrl { get; set; }
    }

    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();
        var settings = config.Get<AppSettings>();

        var fixture = new Fixture();
        fixture.Customize<PatientViewModel>(x => x.With(prop => prop.Gender, () => GenderValues[new Random().Next() % 4]));
        var patientVMs = Enumerable.Range(1, 100).Select(x => fixture.Create<PatientViewModel>()).ToList();

        using var client = new HttpClient();
        System.Console.WriteLine(settings.ApiUrl);
        var response = await client.PostAsJsonAsync(settings.ApiUrl, patientVMs);
        if (response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("OK");
        }
    }
}