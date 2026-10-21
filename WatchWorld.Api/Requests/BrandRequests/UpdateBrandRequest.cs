namespace WatchWorld.Api.Requests.BrandRequests
{
    public record UpdateBrandRequest (
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
