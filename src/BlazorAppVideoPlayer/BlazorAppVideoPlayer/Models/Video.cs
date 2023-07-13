namespace BlazorAppVideoPlayer.Models;

public class Video
{
    public int Id { get; set; }
    public string PosterLink { get; set; } = string.Empty;
    public VideoLink[]? Links { get; set; }
    public Subtitle[]? Subtitles { get; set; }

    public class VideoLink
    {
        public VideoLink(string type, int size, string link)
        {
            this.Type = type;
            this.Size = size;
            this.Link = link;
        }
        public string Type { get; set; } = string.Empty;
        public int Size { get; set; }
        public string Link { get; set; } = string.Empty;
    }

    public class Subtitle
    {
        public Subtitle(string label, string lang, string link)
        {
            this.Label = label;
            this.Lang = lang;
            this.Link = link;
        }
        public string Label { get; set; } = string.Empty;
        public string Lang { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }

}
