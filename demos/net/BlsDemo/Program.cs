using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

const string url = "https://yourbls.avionworx.aero";
const string env = "YourEnd";
const string blueprint = "master";
const string apiKey = "your api key";
const string email = "your@email.com";

var authEndpoint = $"/{env}/login?email={email}";

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(url)
};

// Get JWT token
var authRequest = new HttpRequestMessage(
    HttpMethod.Post,
    authEndpoint);

authRequest.Headers.Add("X-API-KEY", apiKey); 

Console.WriteLine("Requesting JWT token...");

var authResponse = await httpClient.SendAsync(authRequest);

authResponse.EnsureSuccessStatusCode();

var jwtToken = await authResponse.Content.ReadAsStringAsync();

Console.WriteLine($"Auth response: {jwtToken}"); 

if (string.IsNullOrWhiteSpace(jwtToken))
{
    throw new Exception("JWT token not found.");
}

// Get some payload data from a file (data is in Gna Model format)
var payloadJson = await File.ReadAllTextAsync("payload.json"); 

// Define what to calculate with BLS (FDP and Duty in this example)
var dataEndpoint = $"/{env}/{blueprint}/evaluate?label=FDP&label=Duty&label=FDPlimit"; 

// Send authenticated request
var dataRequest = new HttpRequestMessage(
    HttpMethod.Post,
    dataEndpoint);

dataRequest.Headers.Authorization =
    new AuthenticationHeaderValue("Bearer", jwtToken);

dataRequest.Content = new StringContent(
    payloadJson,
    Encoding.UTF8,
    "application/json");

Console.WriteLine("Sending payload...");

var dataResponse = await httpClient.SendAsync(dataRequest);

var responseBody = await dataResponse.Content.ReadAsStringAsync();

var doc = System.Text.Json.JsonSerializer.Deserialize(responseBody,typeof(object));

var formatted = System.Text.Json.JsonSerializer.Serialize(
    doc,
    new JsonSerializerOptions
    {
        WriteIndented = true
    });

Console.WriteLine($"Status: {dataResponse.StatusCode}");
Console.WriteLine("Response:");
Console.WriteLine(formatted);