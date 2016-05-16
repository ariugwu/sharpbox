using Newtonsoft.Json;

namespace sharpbox.Cli.Helper
{
    public static class Format
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships

        public static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source, SerializerSettings);
        }
    }
}
