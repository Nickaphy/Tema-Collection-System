namespace WatchWorld.Application.Commands.BrandCommands
{
    public record CreateBrandCommand(
        string name,
        int foundingYear,
        string countryOfOrigin,
        string? parentGroup,
        string logoUrl,
        string websiteUrl,
        string founderName,
        string founderDescriptionNote,
        string? originDescriptionNote,
        string brandDescriptionNote,
        string watchStyleDescriptionNote,
        bool isActive
    )
    {
    }
}
