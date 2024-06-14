using System.Net;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Text;

namespace RestApiEndPoints;
public class RestApiEndPoints
{
    
    private static readonly string email = "admin@libraryman.com";
    private static readonly string accessToken = "QenEmNl4jq6QK8fBcEJxbJlxl/s9PYSF+44y3pKLEF8=";
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        bool _running = true;
        while (_running)
        {
            Console.WriteLine("Wybierz polecenie:");
            Console.WriteLine("1. Dodaj wydawnictwo");
            Console.WriteLine("2. Pobierz listę wydawnictw");
            Console.WriteLine("3. Pobierz szczegóły wydawnictwa");
            Console.WriteLine("4. Aktualizuj dane wydawnictwa");
            Console.WriteLine("5. Usuń wydawnictwo");
            Console.WriteLine("0. Wyjdź");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PostWydawnictwo();
                    Console.WriteLine();
                    break;
                case "2":
                    await GetWydawinctwoAsync();
                    Console.WriteLine();
                    break;
                case "3":
                    await GetSzczegolyWydawnictwa();
                    Console.WriteLine();
                    break;
                case "4":
                    await AktualizujWydawnictwo();
                    Console.WriteLine();
                    break;
                case "5":
                    await UsunWydawnictwo();
                    Console.WriteLine();
                    break;
                case "0":
                    _running = false;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowe polecenie. Spróbuj ponownie.");
                    break;
            }
        }
    }
    public static void PostWydawnictwo(){
        Console.Write("Podaj nazwę wydawnictwa:");
        var PublisherNamePost = Console.ReadLine();
        Console.Write("Podaj miasto:");
        var CityPost = Console.ReadLine();
        Console.Write("Podaj kraj:");
        var CountryPost = Console.ReadLine();
        Console.Write("Podaj rok założenia:");
        var FoundedPost = Console.ReadLine();
        Console.Write("Podaj opis:");
        var DescriptionPost = Console.ReadLine();
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5052/api/WydawnictwoControllerApi");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        httpWebRequest.Headers["Authorization"]= accessToken;
        httpWebRequest.Headers["Email"]= email;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = JsonSerializer.Serialize(new
                        {
                            PublisherName = PublisherNamePost,
                            City = CityPost,
                            Country = CountryPost,
                            Founded = FoundedPost,
                            Description = DescriptionPost
                        });
            streamWriter.Write(json);
        }
        try
        {
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono wydawnictwa o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas dodawania wydawnictwa.");
            }
        }
    }
    public static async Task GetWydawinctwoAsync()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5052/api/WydawnictwoControllerApi");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";

        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Email"] = email;
        try{
            using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = await streamReader.ReadToEndAsync();
                    Console.WriteLine(result);
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono wydawnictwa o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas pobierania wydawnictwa.");
            }
        }

    }
    public static async Task GetSzczegolyWydawnictwa()
    {
        Console.Write("Podaj nazwę wydawnictwa, którego szczegóły chcesz wyświetlić: ");
        string PublisherName = Console.ReadLine();
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5052/api/WydawnictwoControllerApi/{PublisherName}");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";

        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Email"] = email;

        try
        {
            using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = await streamReader.ReadToEndAsync();
                    Console.WriteLine(result);
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono wydawnictwa o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas pobierania wydawnictwa.");
            }
        }
    }
    public static async Task AktualizujWydawnictwo()
    {
        Console.Write("Podaj nazwę wydawnictwa:");
        var PublisherNamePut = Console.ReadLine();
        Console.Write("Podaj miasto:");
        var CityPut = Console.ReadLine();
        Console.Write("Podaj kraj:");
        var CountryPut = Console.ReadLine();
        Console.Write("Podaj rok założenia:");
        var FoundedPut = Console.ReadLine();
        Console.Write("Podaj opis:");
        var DescriptionPost = Console.ReadLine();
        
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5052/api/WydawnictwoControllerApi/{PublisherNamePut}");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";

        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Email"] = email;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = JsonSerializer.Serialize(new
            {
                PublisherName = PublisherNamePut,
                City = CityPut,
                Country = CountryPut,
                Founded = FoundedPut,
                Description = DescriptionPost
            });
            streamWriter.Write(json);
        }

        try
        {
            using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
            {
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Wydawnictwo został zaktualizowany pomyślnie.");
                }
                else
                {
                    Console.WriteLine($"Nie udało się zaktualizować wydawnictwa. Kod odpowiedzi: {httpResponse.StatusCode}");
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono wydawnictwa o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas aktualizacji wydawnictwa.");
            }
        }
    }
    public static async Task UsunWydawnictwo()
    {
        Console.Write("Podaj nazwę wydawnictwa, który chcesz usunąć: ");
        var PublisherName = Console.ReadLine();
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5052/api/WydawnictwoControllerApi/{PublisherName}");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "DELETE";

        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Email"] = email;

        try
        {
            using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
            {
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Wydawnictwo został pomyślnie usunięty.");
                }
                else
                {
                    Console.WriteLine($"Nie udało się usunąć wydawnictwa. Kod odpowiedzi: {httpResponse.StatusCode}");
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono wydawnictwa o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas usuwania wydawnictwa.");
            }
        }
    }

}

