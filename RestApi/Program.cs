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
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;
        await GetWydawinctwoAsync();
        bool _running = true;
    //     while (_running)
    //     {
    //         Console.WriteLine("Wybierz polecenie:");
    //         Console.WriteLine("1. Dodaj browar");
    //         Console.WriteLine("2. Pobierz listę browarów");
    //         Console.WriteLine("3. Pobierz szczegóły browaru");
    //         Console.WriteLine("4. Aktualizuj dane browaru");
    //         Console.WriteLine("5. Usuń browar");
    //         Console.WriteLine("0. Wyjdź");

    //         var input = Console.ReadLine();

    //         switch (input)
    //         {
    //             case "1":
    //                 PostBrowar();
    //                 Console.WriteLine();
    //                 break;
    //             case "2":
    //                 await GetWydawinctwoAsync();
    //                 Console.WriteLine();
    //                 break;
    //             case "3":
    //                 await GetSzczegolyBrowaru();
    //                 Console.WriteLine();
    //                 break;
    //             case "4":
    //                 await AktualizujBrowar();
    //                 Console.WriteLine();
    //                 break;
    //             case "5":
    //                 await UsunBrowar();
    //                 Console.WriteLine();
    //                 break;
    //             case "0":
    //                 _running = false;
    //                 break;
    //             default:
    //                 Console.WriteLine("Nieprawidłowe polecenie. Spróbuj ponownie.");
    //                 break;
    //         }
    //     }
    }
    public static void PostBrowar(){
        Console.Write("Podaj nazwę browaru:");
        var BreweryNamePost = Console.ReadLine();
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
                            BreweryName = BreweryNamePost,
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
                Console.WriteLine("Nie znaleziono browaru o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas dodawania browaru.");
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
                Console.WriteLine("Nie znaleziono browaru o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas pobierania browaru.");
            }
        }

    }
    public static async Task GetSzczegolyBrowaru()
    {
        Console.Write("Podaj nazwę browaru, którego szczegóły chcesz wyświetlić: ");
        string BreweryName = Console.ReadLine();
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5151/api/BrowarModelControllerApi/{BreweryName}");
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
                Console.WriteLine("Nie znaleziono browaru o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas pobierania browaru.");
            }
        }
    }
    public static async Task AktualizujBrowar()
    {
        Console.Write("Podaj nazwę browaru:");
        var BreweryNamePut = Console.ReadLine();
        Console.Write("Podaj miasto:");
        var CityPut = Console.ReadLine();
        Console.Write("Podaj kraj:");
        var CountryPut = Console.ReadLine();
        Console.Write("Podaj rok założenia:");
        var FoundedPut = Console.ReadLine();
        Console.Write("Podaj opis:");
        var DescriptionPost = Console.ReadLine();
        
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5151/api/BrowarModelControllerApi/{BreweryNamePut}");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "PUT";

        httpWebRequest.Headers["Authorization"] = accessToken;
        httpWebRequest.Headers["Email"] = email;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = JsonSerializer.Serialize(new
            {
                BreweryName = BreweryNamePut,
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
                    Console.WriteLine("Browar został zaktualizowany pomyślnie.");
                }
                else
                {
                    Console.WriteLine($"Nie udało się zaktualizować browaru. Kod odpowiedzi: {httpResponse.StatusCode}");
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono browaru o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas aktualizacji browaru.");
            }
        }
    }
    public static async Task UsunBrowar()
    {
        Console.Write("Podaj nazwę browaru, który chcesz usunąć: ");
        var BreweryName = Console.ReadLine();
        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://localhost:5151/api/BrowarModelControllerApi/{BreweryName}");
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
                    Console.WriteLine("Browar został pomyślnie usunięty.");
                }
                else
                {
                    Console.WriteLine($"Nie udało się usunąć browaru. Kod odpowiedzi: {httpResponse.StatusCode}");
                }
            }
        }
        catch (WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("Nie znaleziono browaru o podanym identyfikatorze.");
            }
            else
            {
                Console.WriteLine("Wystąpił błąd podczas usuwania browaru.");
            }
        }
    }

}

