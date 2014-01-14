using Newtonsoft.Json;

namespace MegaSite.Api.Trash
{
    public static class InternalJsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static T Deserialize<T>(string obj) where T: class
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(obj, new JsonSerializerSettings());
        }
    }
}
