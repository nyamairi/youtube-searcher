using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeSearcher
{
    internal static class VideoSearcher
    {
        public static async Task<IEnumerable<VideoSummary>> SearchSummaries(string apiKey, string keyword)
        {
            var youtube = new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey });
            var identifiers = await FetchIdentifiers(keyword, youtube);

            var videos = await FetchVideos(youtube, identifiers);
            return videos.Select(BuildVideoSummary);
        }

        private static async Task<IList<Video>> FetchVideos(YouTubeService youtube, IEnumerable<string> identifiers)
        {
            var listRequest = youtube.Videos.List("snippet,statistics");
            listRequest.Id = new Repeatable<string>(identifiers);
            var listResponse = await listRequest.ExecuteAsync();
            return listResponse.Items;
        }

        private static async Task<IEnumerable<string>> FetchIdentifiers(string keyword, YouTubeService youtube)
        {
            var req = youtube.Search.List("id");
            req.Q = keyword;
            req.Type = "video";
            req.Order = SearchResource.ListRequest.OrderEnum.Relevance;

            var searchResponse = await req.ExecuteAsync();
            return searchResponse.Items.Select(i => i.Id.VideoId);
        }

        private static VideoSummary BuildVideoSummary(Video video)
        {
            var snippet = video.Snippet;
            var videoStatistics = video.Statistics;
            return new VideoSummary(
                video.Id,
                snippet.Title,
                snippet.ChannelTitle,
                snippet.PublishedAt,
                new VideoViewCount(videoStatistics.ViewCount),
                new VideoLikeCount(videoStatistics.LikeCount)
            );
        }
    }
}