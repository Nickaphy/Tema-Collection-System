namespace WatchWorld.Api.Requests.ImageRequests
{
    public record CreateImageRequest(
        string url,
        int height,
        int width

    )
    {

    }
}
