using Newtonsoft.Json;

namespace RC.JsonServices
{
    public class Json
    {
        public static T Deserialize<T>(string body)
        {
            return JsonConvert.DeserializeObject<T>(body);
        }

        public static string Serialize(object body)
        {
            return JsonConvert.SerializeObject(body);
        }
    }
}
