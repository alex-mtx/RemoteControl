using Newtonsoft.Json;
using System.IO;

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

        public static T DeserializeFromFile<T>(string filePath)
        {
            var body = File.ReadAllText(filePath);
            return Deserialize<T>(body);
        }
    }
}
