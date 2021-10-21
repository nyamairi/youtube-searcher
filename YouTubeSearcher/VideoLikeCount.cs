namespace YouTubeSearcher
{
    internal class VideoLikeCount
    {
        private readonly ulong? _value;

        public VideoLikeCount(ulong? value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value?.ToString("N0") ?? "?";
        }
    }
}