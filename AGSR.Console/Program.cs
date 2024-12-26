using System.Net.Http.Json;
using AGSR.TestTask.ViewModels;
using AutoFixture;

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

    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

        var fixture = new Fixture();
        fixture.Customize<PatientViewModel>(x => x.With(prop => prop.Gender, () => GenderValues[new Random().Next() % 4]));
        var patientVMs = Enumerable.Range(1, 100).Select(x => fixture.Create<PatientViewModel>()).ToList();

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync("http://localhost:5285/patient", patientVMs);
        if (response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("OK");
        }
    }
}