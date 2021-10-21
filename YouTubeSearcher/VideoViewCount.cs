namespace YouTubeSearcher
{
    internal class VideoViewCount
    {
        private readonly ulong? _value;

        public VideoViewCount(ulong? value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value?.ToString("N0") ?? "?";
        }
    }
}