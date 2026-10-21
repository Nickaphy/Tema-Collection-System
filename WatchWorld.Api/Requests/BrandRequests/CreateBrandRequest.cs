namespace WatchWorld.Api.Requests.BrandRequests
{
    public record CreateBrandRequest (
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
