using RestSharp;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Random_Song_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var word = GetNewWord();
            Console.WriteLine(word);
        }

        static string GetNewWord()
        {
            var client = new RestClient("https://random-words-api.vercel.app");
            var request = new RestRequest("/word");
            var responseA = client.GetAsync(request);

            var response = responseA.GetAwaiter().GetResult();

            // using range indexes to trim array wrapper from Json file
            var jsonString = response.Content[1..^1];

            var data = JsonSerializer.Deserialize<WordClass>(jsonString,
                       new JsonSerializerOptions() { WriteIndented = true });

            return data.word;
        }
    }
    class WordClass
    {
        public string word { get; set; }
        public string definition { get; set; }
        public string pronunciation { get; set; }
    }
}
