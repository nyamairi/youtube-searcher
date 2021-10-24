using System;
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
        public static IAsyncEnumerable<VideoSummary> SearchSummaries(string apiKey, string keyword)
        {
            return SearchSummaries(apiKey, keyword, new MaxResults(5));
        }

        public static async IAsyncEnumerable<VideoSummary> SearchSummaries(
            string apiKey,
            string keyword,
            MaxResults maxResults
        )
        {
            using var youtube = new YouTubeService(new BaseClientService.Initializer { ApiKey = apiKey });
            var searcher = new Searcher(youtube);

            await foreach (var video in searcher.Search(keyword, maxResults))
            {
                yield return BuildSummary(video);
            }
        }

        private static VideoSummary BuildSummary(Video video)
        {
            var snippet = video.Snippet;
            var statistics = video.Statistics;
            return new VideoSummary(
                video.Id,
                snippet.Title,
                snippet.ChannelTitle,
                snippet.PublishedAt,
                new VideoViewCount(statistics.ViewCount),
                new VideoLikeCount(statistics.LikeCount)
            );
        }

        private class Searcher
        {
            private const int ApiMaxResults = 50;

            private readonly YouTubeService _youtube;

            public Searcher(YouTubeService youtube)
            {
                _youtube = youtube;
            }

            public async IAsyncEnumerable<Video> Search(string keyword, MaxResults maxResults)
            {
                await foreach (var videos in SearchChunks(keyword, maxResults))
                foreach (var video in videos)
                {
                    yield return video;
                }
            }

            private async IAsyncEnumerable<IEnumerable<Video>> SearchChunks(string keyword, MaxResults maxResults)
            {
                await foreach (var identifiers in SearchIdentifiersChunks(keyword, maxResults))
                {
                    yield return await FetchVideos(identifiers);
                }
            }

            private async Task<IEnumerable<Video>> FetchVideos(IEnumerable<string> identifiers)
            {
                var req = _youtube.Videos.List("snippet,statistics");
                req.Id = new Repeatable<string>(identifiers);
                var res = await req.ExecuteAsync();
                return res.Items;
            }

            private async IAsyncEnumerable<IEnumerable<string>> SearchIdentifiersChunks(
                string keyword,
                MaxResults maxResults)
            {
                var response = await RequestIdentifiers(keyword, maxResults);
                var identifiers = ToVideoIdentifiers(response).ToArray();
                if (identifiers.Length == 0)
                {
                    yield break;
                }

                yield return identifiers;

                for (
                    var remaining = maxResults - identifiers.Length;
                    remaining > 0 && HasNextPage(response);
                    remaining -= identifiers.Length
                )
                {
                    response = await RequestIdentifiers(keyword, remaining, response.NextPageToken);
                    identifiers = ToVideoIdentifiers(response).ToArray();
                    yield return identifiers;
                }
            }

            private static IEnumerable<string> ToVideoIdentifiers(SearchListResponse res)
            {
                return res.Items.Select(i => i.Id.VideoId);
            }

            private static bool HasNextPage(SearchListResponse response)
            {
                return !string.IsNullOrEmpty(response.NextPageToken);
            }

            private async Task<SearchListResponse> RequestIdentifiers(
                string keyword,
                int maxResults,
                string pageToken = null)
            {
                var req = _youtube.Search.List("id");
                req.Q = keyword;
                req.Type = "video";
                req.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                req.MaxResults = maxResults < ApiMaxResults ? maxResults : ApiMaxResults;
                req.PageToken = pageToken;
                return await req.ExecuteAsync();
            }
        }

        public readonly struct MaxResults
        {
            public static readonly MaxResults Max = new(MaxValue);

            private const int MinValue = 1;
            private const int MaxValue = 100;

            private readonly int _value;

            public MaxResults(int value)
            {
                if (value is < MinValue or > MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"> {MinValue} and <= {MaxValue}");
                }

                _value = value;
            }

            public static implicit operator int(MaxResults r) => r._value;
        }
    }
}