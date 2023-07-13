namespace BlazorAppVideoPlayer.Data;

public static class VideosData
{
    public static Models.Video[] Videos = new Models.Video[]{
        new Models.Video{
            Id = 1,
            PosterLink = "/api/Video/GetPoster?id=1",
            Links = new Models.Video.VideoLink[]{
                new Models.Video.VideoLink("video/mp4", 360, "/api/Video/GetVideo?id=1"),
                new Models.Video.VideoLink("video/mp4", 480, "/api/Video/GetVideo?id=2")
            },
            Subtitles = new Models.Video.Subtitle[]{
                new Models.Video.Subtitle("English", "en", "/api/Video/GetSubtitle?id=1"),
                new Models.Video.Subtitle("Turkish", "tr", "/api/Video/GetSubtitle?id=2"),
            }
        }
    };

}
