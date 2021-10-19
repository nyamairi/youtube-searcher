using System;

namespace YouTubeSearcher
{
    internal record VideoSummary(string Id, string Title, DateTime? PublishedAt)
    {
        public Uri Url => new($"https://youtube.com/watch?v={Id}");

        public void Deconstruct(out string id, out string title, out DateTime? publishedAt, out Uri url)
        {
            id = Id;
            title = Title;
            publishedAt = PublishedAt;
            url = Url;
        }
    }
}