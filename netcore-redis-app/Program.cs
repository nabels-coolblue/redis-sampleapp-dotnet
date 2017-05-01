using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace netcore_redis_app
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSampleAsync().Wait();
        }
            
        public static async Task RunSampleAsync()
        {
            var key = "myDate";
            var value = DateTime.Now.ToString();
            
            var cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = "192.168.56.11:6379"
            });
            Console.WriteLine("Connected to Redis");

            Console.WriteLine($"Setting ['{key}'] to: '{value}'");
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(value), new DistributedCacheEntryOptions());
        }
    }
}
