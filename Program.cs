using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Random_Song_Generator
{
    class Program
    {
        const int MIN_WORDS = 5;
        const int MAX_WORDS = 20;
        static void Main(string[] args)
        {
            int n;
            do
            {
                Console.Write($"How many records to generate? (between {MIN_WORDS} and {MAX_WORDS}): ");
                n = int.Parse(Console.ReadLine());
            } while (n < MIN_WORDS || n > MAX_WORDS);

            var data = GenerateData(n);

            PrintData(data);
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

        static IRelease GetSong(string word)
        {
            var q = new Query("RandomSongGenerator", "1.0", "https://github.com/Dejniel01/Random-Song-Generator");

            var songs = q.FindReleases($"release:{word}");

            var s = songs.Results.GetEnumerator();

            return s.MoveNext() ? s.Current.Item : null;
        }

        static List<(string word, IRelease song)> GenerateData(int n)
        {
            Console.WriteLine("Generating data...");
            var data = new List<(string word, IRelease song)>();
            var words = new HashSet<string>();

            for (int i = 0; i < n; i++)
            {
                string word;
                do
                {
                    word = GetNewWord();
                } while (words.Contains(word));
                var song = GetSong(word);
                data.Add((word, song));
                words.Add(word);
            }

            data.Sort(((string word, IRelease song) t1, (string word, IRelease song) t2) 
                => t1.word.CompareTo(t2.word));

            return data;
        }


        static void PrintData(List<(string word, IRelease song)> data)
        {
            foreach (var (word, song) in data)
            {
                Console.WriteLine(word);
                if (song == null)
                {
                    Console.WriteLine("\tNo recording found!");
                }
                else
                {
                    Console.WriteLine($"\tSong title:\n\t\t{song.Title}");
                    Console.WriteLine($"\tArtists:");
                    foreach (var a in song.ArtistCredit)
                        Console.WriteLine($"\t\t{a.Name}");
                    Console.WriteLine($"\tAlbum:\n\t\t{song.ReleaseGroup}");
                }
            }
        }
    }
    class WordClass
    {
        public string word { get; set; }
        public string definition { get; set; }
        public string pronunciation { get; set; }
    }
    
}
