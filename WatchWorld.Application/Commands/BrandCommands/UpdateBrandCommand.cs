namespace WatchWorld.Application.Commands.BrandCommands
{
    public record UpdateBrandCommand(
        Guid id,
        string? parentGroup,
        string logoUrl,
        string websiteUrl,
        string founderDescriptionNote,
        string? originDescriptionNote,
        string brandDescriptionNote,
        string watchStyleDescriptionNote,
        bool isActive
    )
    {
    }
}
