using System.Text.Json;

namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class EnumDisplayConverter
    {
    // Maps the numeric values from the Enums the API actually sends to readable strings for display in the UI.
        private static readonly Dictionary<int, string> CaseShapeNames = new()
        {
            [0] = "Ukendt",
            [1] = "Rund",
            [2] = "Rektangulær",
            [3] = "Kvadratisk",
            [4] = "Oval",
            [5] = "Tonneau",
            [6] = "Pude"
        };

        private static readonly Dictionary<int, string> CaseMaterialNames = new()
        {
            [0] = "Ukendt",
            [1] = "Stål",
            [2] = "Guld/stål",
            [3] = "Rosaguld",
            [4] = "Gulguld",
            [5] = "Hvidguld",
            [6] = "Rødguld",
            [7] = "Titanium",
            [8] = "Keramik",
            [9] = "Platin",
            [10] = "Bronze",
            [11] = "Carbon",
            [12] = "Sølv",
            [13] = "Plastik",
            [14] = "Aluminium",
            [15] = "Tantal",
            [16] = "Palladium",
            [17] = "Wolfram"
        };

        private static readonly Dictionary<int, string> BraceletTypeNames = new()
        {
            [0] = "Ukendt",
            [1] = "Stål",
            [2] = "Guld/stål",
            [3] = "Gulguld",
            [4] = "Rosaguld",
            [5] = "Hvidguld",
            [6] = "Rødguld",
            [7] = "Platin",
            [8] = "Titanium",
            [9] = "Keramik",
            [10] = "Læder",
            [11] = "Krokodilleskind",
            [12] = "Kalveskind",
            [13] = "Firbenskind",
            [14] = "Strudseskind",
            [15] = "Gummi",
            [16] = "Silikone",
            [17] = "Tekstil",
            [18] = "Satin",
            [19] = "Plastik"
        };

        private static readonly Dictionary<int, string> GenderNames = new()
        {
            [0] = "Unisex",
            [1] = "Herre",
            [2] = "Dame"
        };

        private static readonly Dictionary<int, string> MovementTypeNames = new()
        {
            [0] = "Ukendt",
            [1] = "Automatisk",
            [2] = "Håndoptræk",
            [3] = "Quartz"
        };

        private static readonly Dictionary<int, string> WearGradeNames = new()
        {
            [0] = "Som nyt",
            [1] = "Minimal slitage",
            [2] = "Brugt",
            [3] = "Velbrugt",
            [4] = "Hårdt brugt"
        };
        private static string RawText(JsonElement el) => el.ValueKind switch
        {
            JsonValueKind.String => el.GetString() ?? "",
            JsonValueKind.Number => el.GetInt32().ToString(),
            _ => el.ToString()
        };

        private static string Lookup(Dictionary<int, string> map, JsonElement el)
        {
            var raw = RawText(el);
            return int.TryParse(raw, out var code) && map.TryGetValue(code, out var name) ? name : raw;
        }

        public static string CaseShape(JsonElement el) => Lookup(CaseShapeNames, el);
        public static string CaseMaterial(JsonElement el) => Lookup(CaseMaterialNames, el);
        public static string BraceletType(JsonElement el) => Lookup(BraceletTypeNames, el);
        public static string Gender(JsonElement el) => Lookup(GenderNames, el);
        public static string MovementType(JsonElement el) => Lookup(MovementTypeNames, el);
        public static string WearGrade(JsonElement el) => Lookup(WearGradeNames, el);

        // Kept for anything that just needs the raw value with no mapping.
        public static string Text(JsonElement el) => RawText(el);

        private static IReadOnlyList<(int Value, string Label)> ToOptions(Dictionary<int, string> map) =>
            map.OrderBy(kv => kv.Key).Select(kv => (kv.Key, kv.Value)).ToList();

        public static IReadOnlyList<(int Value, string Label)> CaseShapeOptions => ToOptions(CaseShapeNames);
        public static IReadOnlyList<(int Value, string Label)> CaseMaterialOptions => ToOptions(CaseMaterialNames);
        public static IReadOnlyList<(int Value, string Label)> MovementTypeOptions => ToOptions(MovementTypeNames);
        public static IReadOnlyList<(int Value, string Label)> GenderOptions => ToOptions(GenderNames);
        public static IReadOnlyList<(int Value, string Label)> BraceletTypeOptions => ToOptions(BraceletTypeNames);
        public static IReadOnlyList<(int Value, string Label)> WearGradeOptions => ToOptions(WearGradeNames);


    }
}
