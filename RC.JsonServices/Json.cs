using Newtonsoft.Json;
using System;
using System.IO;

namespace RC.JsonServices
{
    public class Json
    {
        public static T Deserialize<T>(string body)
        {
            return JsonConvert.DeserializeObject<T>(body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">Absolute or relative path allowed</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public static T DeserializeFromFile<T>(string filePath)
        {
            var fullPath = GetAbsoluteUri(filePath);
            var body = File.ReadAllText(fullPath);
            return Deserialize<T>(body);
        }

        private static string GetAbsoluteUri(string filePath)
        {
            try
            {
                var uri = new Uri(filePath); //Throw UriFormatException when path it's relative
                return filePath;
            }
            catch (UriFormatException ex) 
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }
         
        }

        public static string Serialize(object body)
        {
            return JsonConvert.SerializeObject(body);
        }
    }
}
