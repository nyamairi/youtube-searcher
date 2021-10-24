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

            await foreach (var summary in VideoSearcher.SearchSummaries(ApiKey, keyword, VideoSearcher.MaxResults.Max))
            {
                await Console.Out.WriteLineAsync(ToString(summary));
            }
        }

        private static string ToString(VideoSummary summary)
        {
            var (_, title, channelTitle, publishedAt, viewCount, likeCount, url) = summary;
            return $"[{channelTitle}] {title}{publishedAt?.ToString(" (yyyy/MM/dd)")} {viewCount} views {likeCount} likes {url}";
        }
    }
}