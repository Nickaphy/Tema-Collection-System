using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using static System.Net.WebRequestMethods;

namespace WatchWorld.Infrastructure.Database.Seed
{
    public class SeedBrands
    {
        public static readonly IList<Brand> brands = new List<Brand>
        {

        };
        public static async Task SeedBrandsAsync(AppDbContext context)
        {


            // ==========================================
            // SEED DATA: BRANDS
            // ==========================================

            var brand1 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Audemars Piguet",
                ["FoundingYear"] = 1875,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Independent",
                ["IsActive"] = true,
                ["FounderName"] = "Jules Louis Audemars & Edward Auguste Piguet",
                ["FounderDescriptionNote"] = "Founded by childhood friends Jules Louis Audemars and Edward Auguste Piguet in Le Brassus.",
                ["OriginDescriptionNote"] = "Established in the heart of the Vallée de Joux, Switzerland, widely recognized as the cradle of high-end horology.",
                ["BrandDescriptionNote"] = "One of the 'Holy Trinity' of haute horlogerie, renowned for ultra-complex mechanical movements and pioneering luxury watch designs.",
                ["WatchStyleDescriptionNote"] = "Avant-garde luxury sports watches, best known for octagonal bezels, exposed hex screws, and 'Tapisserie' patterned dials.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/Logo_Audemars_Piguet.svg",
                ["WebsiteUrl"] = "https://www.audemarspiguet.com"
            });

            var brand2 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Breitling",
                ["FoundingYear"] = 1884,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Partners Group",
                ["IsActive"] = true,
                ["FounderName"] = "Léon Breitling",
                ["FounderDescriptionNote"] = "Léon Breitling focused exclusively on chronographs and precision timing instruments for industrial and scientific applications.",
                ["OriginDescriptionNote"] = "Founded in St. Imier, Switzerland, later relocating primary manufacture operations to La Chaux-de-Fonds.",
                ["BrandDescriptionNote"] = "Historically tied to aviation, military flight crews, and emergency rescue instruments with high-precision chronograph engineering.",
                ["WatchStyleDescriptionNote"] = "Bold aviation chronographs, slide-rule bezels, rider-tab bezels, and rugged professional tool watch silhouettes.",
                ["LogoUrl"] = "https://upload.wikimedia.org/wikipedia/commons/d/d5/Logo_Breitling_2018_1884_P.png?utm_source=en.wikipedia.org&utm_campaign=imageinfo&utm_content=original",
                ["WebsiteUrl"] = "https://www.breitling.com"
            });

            var brand3 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Hamilton",
                ["FoundingYear"] = 1892,
                ["CountryOfOrigin"] = "United States",
                ["ParentGroup"] = "Swatch Group",
                ["IsActive"] = true,
                ["FounderName"] = "Keystone Standard Watch Co. Founders",
                ["FounderDescriptionNote"] = "Established following the merger of several American watchmakers in Lancaster, Pennsylvania.",
                ["OriginDescriptionNote"] = "Originated in Lancaster, Pennsylvania, USA, providing pocket watches that synchronized the American railroad system; production later shifted to Bienne, Switzerland.",
                ["BrandDescriptionNote"] = "Combines American heritage with Swiss precision, famous for military service supply in both World Wars and cinematic watch appearances.",
                ["WatchStyleDescriptionNote"] = "Utilitarian military field watches, mid-century retro designs, and distinctive asymmetrical watch cases.",
                ["LogoUrl"] = "https://thumb.wikimedia.org/wikipedia/commons/thumb/e/e8/Hamilton_Watch_Company_Logo.svg/1280px-Hamilton_Watch_Company_Logo.svg.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail",
                ["WebsiteUrl"] = "https://www.hamiltonwatch.com"
            });

            var brand4 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "IWC",
                ["FoundingYear"] = 1868,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Richemont",
                ["IsActive"] = true,
                ["FounderName"] = "Florentine Ariosto Jones",
                ["FounderDescriptionNote"] = "An American watchmaker and engineer from Boston who traveled to Switzerland to combine Swiss craft with modern American manufacturing.",
                ["OriginDescriptionNote"] = "Located in Schaffhausen, eastern German-speaking Switzerland, far from the traditional watchmaking hub of western Geneva.",
                ["BrandDescriptionNote"] = "International Watch Company is renowned for functional engineering, pilot timepieces, and industrial material innovations like ceramic and titanium.",
                ["WatchStyleDescriptionNote"] = "Instrument-style pilot dials, clean dress watches, and robust engineering-focused calendar complications.",
                ["LogoUrl"] = "https://thumb.wikimedia.org/wikipedia/commons/thumb/f/fa/International_Watch_Company_logo.svg/1280px-International_Watch_Company_logo.svg.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail",
                ["WebsiteUrl"] = "https://www.iwc.com"
            });

            var brand5 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Jaeger-LeCoultre",
                ["FoundingYear"] = 1833,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Richemont",
                ["IsActive"] = true,
                ["FounderName"] = "Antoine LeCoultre",
                ["FounderDescriptionNote"] = "Antoine LeCoultre was an inventor who created machines capable of measuring micrometers, setting new accuracy standards.",
                ["OriginDescriptionNote"] = "Based in Le Sentier in the Vallée de Joux, Switzerland.",
                ["BrandDescriptionNote"] = "Known as the 'Watchmaker's Watchmaker' due to historically supplying raw movement blanks (ébauches) to Patek Philippe, Audemars Piguet, and Vacheron Constantin.",
                ["WatchStyleDescriptionNote"] = "Art Deco elegance, iconic swiveling case designs, and ultra-thin high complications.",
                ["LogoUrl"] = "https://upload.wikimedia.org/wikipedia/commons/d/df/Jaeger-LeCoultre_Logo.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail_unscaled",
                ["WebsiteUrl"] = "https://www.jaeger-lecoultre.com"
            });

            var brand6 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Omega",
                ["FoundingYear"] = 1848,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Swatch Group",
                ["IsActive"] = true,
                ["FounderName"] = "Louis Brandt",
                ["FounderDescriptionNote"] = "Louis Brandt established a small pocket watch assembly workshop in La Chaux-de-Fonds.",
                ["OriginDescriptionNote"] = "Founded in La Chaux-de-Fonds, with modern headquarters located in Bienne, Switzerland.",
                ["BrandDescriptionNote"] = "Famous for space exploration history as the first watch worn on the Moon, official Olympic timekeeping, and pioneering the Co-Axial escapement.",
                ["WatchStyleDescriptionNote"] = "Professional dive watches with helium escape valves, manual-wind chronograph instruments, and refined dress lines.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/Omega_Logo.svg",
                ["WebsiteUrl"] = "https://www.omegawatches.com"
            });

            var brand7 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Oris",
                ["FoundingYear"] = 1904,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Independent",
                ["IsActive"] = true,
                ["FounderName"] = "Paul Cattin & Georges Christian",
                ["FounderDescriptionNote"] = "Paul Cattin and Georges Christian purchased the recently closed Lohner & Co factory in Hölstein.",
                ["OriginDescriptionNote"] = "Based in Hölstein in northern Switzerland, named after a nearby brook.",
                ["BrandDescriptionNote"] = "An independent Swiss manufacture focused exclusively on mechanical watches engineered for real-world functionality.",
                ["WatchStyleDescriptionNote"] = "Functional tool dive watches, vintage-inspired pilot watches with big crowns, and clean everyday sports models.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/Oris_logo.svg",
                ["WebsiteUrl"] = "https://www.oris.ch"
            });

            var brand8 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Panerai",
                ["FoundingYear"] = 1860,
                ["CountryOfOrigin"] = "Italy",
                ["ParentGroup"] = "Richemont",
                ["IsActive"] = true,
                ["FounderName"] = "Giovanni Panerai",
                ["FounderDescriptionNote"] = "Giovanni Panerai opened his watchmaker's shop in Florence, which doubled as the city's first watchmaking school.",
                ["OriginDescriptionNote"] = "Founded in Florence, Italy, as an official supplier to the Royal Italian Navy; watch production later moved to Neuchâtel, Switzerland.",
                ["BrandDescriptionNote"] = "Italian military naval heritage known for pioneering luminous materials (Radiomir and Luminor) for underwater operations.",
                ["WatchStyleDescriptionNote"] = "Bold oversized cushion cases, patented crown-protecting locking bridges, wire lugs, and high-contrast sandwich dials.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/Panerai_logo.svg",
                ["WebsiteUrl"] = "https://www.panerai.com"
            });

            var brand9 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Patek Philippe",
                ["FoundingYear"] = 1839,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Independent",
                ["IsActive"] = true,
                ["FounderName"] = "Antoni Patek & Adrien Philippe",
                ["FounderDescriptionNote"] = "Polish businessman Antoni Patek partnered with French watchmaker Adrien Philippe, inventor of the keyless winding mechanism.",
                ["OriginDescriptionNote"] = "Geneva, Switzerland—the central historic hub of high-end traditional Swiss watchmaking.",
                ["BrandDescriptionNote"] = "Widely considered the ultimate traditional Swiss luxury watchmaker, famous for unmatched finishing, family ownership, and grand complications.",
                ["WatchStyleDescriptionNote"] = "Timeless dress watches, intricate perpetual calendars, chime complications, and iconic luxury integrated-bracelet designs.",
                ["LogoUrl"] = "https://upload.wikimedia.org/wikipedia/commons/d/d8/Patek_Philippe_Logo2.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail_unscaled",
                ["WebsiteUrl"] = "https://www.patek.com"
            });

            var brand10 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Rolex",
                ["FoundingYear"] = 1905,
                ["CountryOfOrigin"] = "United Kingdom",
                ["ParentGroup"] = "Independent",
                ["IsActive"] = true,
                ["FounderName"] = "Hans Wilsdorf & Alfred Davis",
                ["FounderDescriptionNote"] = "Hans Wilsdorf and Alfred Davis originally established 'Wilsdorf & Davis' in London before registering the Rolex brand name.",
                ["OriginDescriptionNote"] = "Founded in London, UK, before relocating all manufacturing operations and headquarters to Geneva, Switzerland in 1919.",
                ["BrandDescriptionNote"] = "The world's most recognized luxury watch manufacturer, pioneer of the waterproof Oyster case and self-winding rotor system.",
                ["WatchStyleDescriptionNote"] = "Iconic sports and professional tool watches, robust Oyster steel construction, cyclops date lenses, and rotating function bezels.",
                ["LogoUrl"] = "https://thumb.wikimedia.org/wikipedia/commons/thumb/1/1c/Rolex_wordmark_logo.svg/1280px-Rolex_wordmark_logo.svg.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail",
                ["WebsiteUrl"] = "https://www.rolex.com"
            });

            var brand11 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "TAG Heuer",
                ["FoundingYear"] = 1860,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "LVMH",
                ["IsActive"] = true,
                ["FounderName"] = "Edouard Heuer",
                ["FounderDescriptionNote"] = "Edouard Heuer founded Uhrenmanufaktur Heuer in St. Imier, patenting the oscillating pinion still used in chronographs today.",
                ["OriginDescriptionNote"] = "Originated in St. Imier, Switzerland, expanding into La Chaux-de-Fonds.",
                ["BrandDescriptionNote"] = "Deeply intertwined with motor racing history, precision sports timing instruments, and high-frequency chronographs.",
                ["WatchStyleDescriptionNote"] = "Racer-inspired chronographs, sharp angular case geometry, tachymeter bezels, and bold sports aesthetics.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/TAG_Heuer_logo.svg",
                ["WebsiteUrl"] = "https://www.tagheuer.com"
            });

            var brand12 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Tissot",
                ["FoundingYear"] = 1853,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "Swatch Group",
                ["IsActive"] = true,
                ["FounderName"] = "Charles-Félicien Tissot & Charles-Émile Tissot",
                ["FounderDescriptionNote"] = "Father and son team Charles-Félicien and Charles-Émile Tissot established their casing workshop in Le Locle.",
                ["OriginDescriptionNote"] = "Le Locle, Switzerland, maintaining continuous operations in the city since its founding.",
                ["BrandDescriptionNote"] = "A powerhouse of accessible Swiss watchmaking, offering traditional mechanics and modern quartz innovations at volume.",
                ["WatchStyleDescriptionNote"] = "Integrated-bracelet sports models, classic Swiss dress timepieces, and tactile multi-function sports watches.",
                ["LogoUrl"] = "https://thumb.wikimedia.org/wikipedia/commons/thumb/c/ce/Tissot_Logo.svg/1280px-Tissot_Logo.svg.png?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=thumbnail",
                ["WebsiteUrl"] = "https://www.tissotwatches.com"
            });

            var brand13 = DbSeeder.CreateDomainObject<Brand>(new()
            {
                ["Name"] = "Zenith",
                ["FoundingYear"] = 1865,
                ["CountryOfOrigin"] = "Switzerland",
                ["ParentGroup"] = "LVMH",
                ["IsActive"] = true,
                ["FounderName"] = "Georges Favre-Jacot",
                ["FounderDescriptionNote"] = "Georges Favre-Jacot revolutionized watch manufacturing at age 22 by creating one of the first fully integrated watch factories.",
                ["OriginDescriptionNote"] = "Le Locle, Switzerland, operating out of the same historical manufacturing site for over 150 years.",
                ["BrandDescriptionNote"] = "Famous for creating the legendary El Primero in 1969, one of the world's first integrated automatic chronographs operating at high frequency (36,000 vph).",
                ["WatchStyleDescriptionNote"] = "High-frequency chronographs, signature tri-color overlapping subdials, and vintage aviator watches.",
                ["LogoUrl"] = "https://commons.wikimedia.org/wiki/Special:FilePath/Zenith_logo.svg",
                ["WebsiteUrl"] = "https://www.zenith-watches.com"
            });

            brands.Add(brand1);
            brands.Add(brand2);
            brands.Add(brand3);
            brands.Add(brand4);
            brands.Add(brand5);
            brands.Add(brand6);
            brands.Add(brand7);
            brands.Add(brand8);
            brands.Add(brand9);
            brands.Add(brand10);
            brands.Add(brand11);
            brands.Add(brand12);
            brands.Add(brand13);

            await context.Brands.AddRangeAsync(
                brand1, brand2, brand3, brand4, brand5, brand6, brand7,
                brand8, brand9, brand10, brand11, brand12, brand13

            );
            await context.SaveChangesAsync();
        }
    }
}
