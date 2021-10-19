using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeSearcher
{
    internal static class VideoSearcher
    {
        public static async Task<IEnumerable<VideoSummary>> SearchSummaries(string apiKey, string keyword)
        {
            var youtube = new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey });
            var req = youtube.Search.List("snippet");
            req.Q = keyword;
            req.Type = "video";
            req.Order = SearchResource.ListRequest.OrderEnum.Relevance;

            var searchResponse = await req.ExecuteAsync();
            return searchResponse.Items.Select(BuildVideoSummary);
        }

        private static VideoSummary BuildVideoSummary(SearchResult result)
        {
            return new VideoSummary(result.Id.VideoId, result.Snippet.Title, result.Snippet.PublishedAt);
        }
    }
}