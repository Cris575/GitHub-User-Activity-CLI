using GitHub_User_Activity;
using System.Runtime.InteropServices.Swift;
using System.Text.Json;
using System.Text.RegularExpressions;

static class Program
{
    static readonly HttpClient client = new HttpClient()
    {
        BaseAddress = new Uri("https://api.github.com/users/"),
        DefaultRequestHeaders = {
            { "Accept", "application/vnd.github+json" },
            { "X-GitHub-Api-Version", "2022-11-28" },
            { "User-Agent", "Awesome-Octocat-App" },
        },

    };

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== GitHub Activity CLI ===");
        Console.WriteLine("Usage: github-activity <username> (or type 'exit' to exit)\n");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            Match match = Regex.Match(input, @"^github-activity\s+(.+)$", RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                Console.WriteLine("Unrecognized command. Correct format: github-activity <username>\n");
                continue;
            }

            string username = match.Groups[1].Value.Trim();

            List<Model> events = await GetInfo(username);
            PrintInformation(events);
            Console.WriteLine();
        }
    }

    static async Task<List<Model>> GetInfo(string username)
    {
        var items = new List<Model>();

        using HttpResponseMessage response = await client.GetAsync($"{username}/events");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return items;
        }

        var content = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(content);

        if (document.RootElement.ValueKind != JsonValueKind.Array) return items;

        foreach (var repo in document.RootElement.EnumerateArray())
        {
            string type = repo.GetProperty("type").GetString() ?? string.Empty;

            string name = repo.TryGetProperty("repo", out var repoElem) && repoElem.TryGetProperty("name", out var nameElem)
                ? nameElem.GetString() ?? string.Empty
                : string.Empty;

            string action = repo.TryGetProperty("payload", out var payloadElem) && payloadElem.TryGetProperty("action", out var actionElem)
                ? actionElem.GetString() ?? string.Empty
                : string.Empty;

            AddInformation(type, name, action, items);

        }

        return items;
    }

    static void AddInformation(string type,  string? name, string? action, List<Model> items)
    {
        items.Add(new Model { type = type, action = action, name = name });
    }

    static void PrintInformation(List<Model> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("No recent events were found for this user.");
            return;
        }

        var newItem = items.GroupBy(x => x.name).Select(g => new Model
        {
            name = g.First().name,
            total = g.Count(),
            action = g.First().action,
            type = g.First().type
        });

        foreach (var item in newItem)
        {

            switch(item.type)
            {
                case "PushEvent":
                    Console.WriteLine($"Pushed {item.total} commits to {item.name}"); 
                break;   
                case "IssuesEvent":
                    Console.WriteLine($"Opened a new issues in {item.name}"); 
                    break;
                case "WatchEvent":
                    Console.WriteLine($"Starred {item.name}"); 
                    break;
                case "ForkEvent":
                    Console.WriteLine($"Forked {item.name}"); 
                    break;
                case "CreateEvent":
                    Console.WriteLine($"Created {item.name}"); 
                    break;
                default:
                    Console.WriteLine($"Performed {item.total} {item.type} actions in {item.name}");
                    break;
            }
        }
    }

}