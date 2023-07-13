
using BlazorAppVideoPlayer.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BlazorAppVideoPlayer.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    [HttpGet("Get")]
    public IActionResult Get(int id)
    {
        if (id == 0 || id >= VideosData.Videos.Length) return BadRequest("Not valid video id.");
        return Ok(VideosData.Videos[id]);
    }

    [HttpGet("GetPoster")]
    public IActionResult GetPoster(int id)
    {
        string[] videos = new string[]
        {
            "",
            "Nature.poster.jpg"
        };

        string videofilename = videos[id];
        string path = Path.Combine("TestVideos/") + videofilename;
        Response.ContentType = new MediaTypeHeaderValue("image/jpeg").ToString();// Content type
        FileStream filestream = System.IO.File.OpenRead(path);
        return File(filestream, contentType: "image/jpeg", fileDownloadName: videofilename, enableRangeProcessing: true);
    }

    [HttpGet("GetVideo")]
    public IActionResult GetVideo(int id)
    {
        string[] videos = new string[]
        {
            "",
            "Nature-360p.mp4",
            "Nature-480p.mp4"
        };

        string videofilename = videos[id];
        string path = Path.Combine("TestVideos/") + videofilename;
        Response.ContentType = new MediaTypeHeaderValue("video/mp4").ToString();// Content type
        FileStream filestream = System.IO.File.OpenRead(path);
        return File(filestream, contentType: "video/mp4", fileDownloadName: videofilename, enableRangeProcessing: true);
    }

    [HttpGet("GetSubtitle")]
    public IActionResult GetSubtitle(int id)
    {
        string[] subtitles = new string[]
        {
            "",
            "Nature.subtitle.en.vtt",
            "Nature.subtitle.tr.vtt"
        };

        string subtitlefilename = subtitles[id];
        string path = Path.Combine("TestVideos/") + subtitlefilename;
        Response.ContentType = new MediaTypeHeaderValue("text/vtt").ToString();// Content type
        FileStream filestream = System.IO.File.OpenRead(path);
        return File(filestream, contentType: "text/vtt", fileDownloadName: subtitlefilename, enableRangeProcessing: true);
    }

}
