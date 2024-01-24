using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static HttpClient client = new HttpClient();

        public static int calculateNumDraws(string content)
        {
            int drawCount = 0;
            using (JsonDocument doc = JsonDocument.Parse(content))
            {
                JsonElement root = doc.RootElement;
                JsonElement data = root.GetProperty("data");

                foreach (JsonElement match in data.EnumerateArray())
                {
                    int team1Goals = Int32.Parse(match.GetProperty("team1goals").ToString());
                    int team2Goals = Int32.Parse(match.GetProperty("team2goals").ToString());
                    if (team1Goals == team2Goals)
                    {
                        drawCount++;
                    }
                }
            }
            return drawCount;
        }

        public static async Task<int> getNumDrawsAsync(int year)
        {
            int drawCount = 0;
            string initialUrl = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&page=1";
            var initialResponse = await client.GetAsync(initialUrl);
            var initialContent = await initialResponse.Content.ReadAsStringAsync();
            var initialJson = JsonDocument.Parse(initialContent);
            int totalPages = initialJson.RootElement.GetProperty("total_pages").GetInt32();

            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 1; i <= totalPages; i++)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&page={i}";
                tasks.Add(client.GetAsync(url));
            }

            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
            {
                if (!response.IsSuccessStatusCode) continue;
                var content = await response.Content.ReadAsStringAsync();
                drawCount += calculateNumDraws(content);
            }

            return drawCount;
        }

        public static int getNumDraws(int year)
        {
            return getNumDrawsAsync(year).GetAwaiter().GetResult();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(getNumDraws(2011).ToString());
        }
    }
}