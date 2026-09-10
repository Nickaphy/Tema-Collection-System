namespace WatchWorld.Application.Commands.ImageCommands
{
    public record CreateImageCommand(
        string url,
        int height,
        int width

    )
    { }
}
