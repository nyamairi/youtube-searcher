using System;

namespace YouTubeSearcher
{
    internal record VideoSummary(string Id, string Title, DateTime? PublishedAt, VideoViewCount ViewCount,
        VideoLikeCount LikeCount)
    {
        public Uri Url => new($"https://youtube.com/watch?v={Id}");

        public void Deconstruct(
            out string id,
            out string title,
            out DateTime? publishedAt,
            out VideoViewCount viewCount,
            out VideoLikeCount likeCount,
            out Uri url
        )
        {
            id = Id;
            title = Title;
            publishedAt = PublishedAt;
            viewCount = ViewCount;
            likeCount = LikeCount;
            url = Url;
        }
    }
}