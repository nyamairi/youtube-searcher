using System;
using System.Linq;
using System.Threading.Tasks;

namespace YouTubeSearcher
{
    public static class Program
    {
        private const string EncryptedApiKey = "S5CdhRaeKK1lIjn6VSRBM3ueIEtaBMNW1p2M0JZ7vR68NdoyI++5PQ==";

        private static string ApiKey => Decryptor.Decrypt(EncryptedApiKey);

        public static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                await Console.Error.WriteLineAsync($"usage: {Environment.GetCommandLineArgs()[0]} keyword");
                return;
            }

            var keyword = args.First();

            var summaries = await VideoSearcher.SearchSummaries(ApiKey, keyword);
            foreach (var s in summaries.Select(ToString))
            {
                await Console.Out.WriteLineAsync(s);
            }
        }

        private static string ToString(VideoSummary summary)
        {
            var (_, title, publishedAt, viewCount, likeCount, url) = summary;
            return $"{url} {title}{publishedAt?.ToString(" (yyyy/MM/dd)")} 視聴数({viewCount}) +{likeCount}";
        }
    }
}