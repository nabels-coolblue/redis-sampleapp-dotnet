using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;

namespace netcore_redis_app
{
    class Program
    {
        private const string REDIS_HOST = "192.168.56.11";
        private const string REDIS_PORT = "6379";

        public static void Main(string[] args)
        {
            var key = "myDate";
            var value = DateTime.Now.ToString();
            
            var cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = $"{REDIS_HOST}:{REDIS_PORT}"
            });
            
            try
            {
                Console.WriteLine($"Setting ['{key}'] to: '{value}'");
                cache.Set(key, Encoding.UTF8.GetBytes(value), new DistributedCacheEntryOptions());
            }
            catch (RedisConnectionException e)
            {
                Console.WriteLine("[FATAL] Exception occured when connecting to Redis.");
                Console.WriteLine($"Please check whether the specified host {REDIS_HOST}:{REDIS_PORT} is up and running by running:");
                Console.WriteLine($"redis-cli -h {REDIS_HOST} -p {REDIS_PORT} ping");
                Console.WriteLine($"Full exception details:");
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("[FATAL] Generic Exception occured when writing a value to Redis, reason unknown.");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
