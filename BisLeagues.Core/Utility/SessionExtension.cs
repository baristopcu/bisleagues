using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BisLeagues.Core.Utility
{
    public static class SessionExtension
    {
        public static void SetObjectAsJson<T>(this ISession session, string key, T value)
        {
            var serialized = JsonConvert.SerializeObject(value);
            session.SetString(key,serialized);
            var valu2e = session.GetString(key);

        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }

    }
}
