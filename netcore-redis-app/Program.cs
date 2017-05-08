using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using ServiceStack.Redis;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace netcore_redis_app
{
    class Program
    {
        private const string REDIS_HOST = "192.168.56.11";
        private const string REDIS_PORT = "6379";

        public static async Task DoWork()
        {
            using (var redisManager = new PooledRedisClientManager($"{REDIS_HOST}:{REDIS_PORT}"))
            using (var redis = redisManager.GetClient())
            {
                /* Set a key */
                var value = "hello world";
                redis.Set("foo", value);
                Console.WriteLine($"SET: {value}");

                /* Try a GET */
                var foo = redis.Get<string>("foo");
                Console.WriteLine($"GET foo: {foo}");

                /* Create a list of numbers, from 0 to 4 */
                redis.Remove("mylist");

                var tasks = new List<Task>();
                for (int i = 0; i < 4; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        var threadId = Thread.CurrentThread.ManagedThreadId - 4;

                        for (int j = 0; j < 4; j++)
                        {
                            lock(redis)
                            {
                                redis.PushItemToList("mylist", $"element-t{threadId}-{j}");

                                var counter = redis.Increment("counter", 1);
                                Console.WriteLine($"INCR counter: {counter} from thread: {threadId}");
                            }
                        }
                    }));
                }
                var t = Task.WhenAll(tasks);
                t.Wait();
            }

            // /* PING server */
            // reply = redisCommand(c,"PING");
            // printf("PING: %s\n", reply->str);
            // freeReplyObject(reply);

            // /* Set a key */
            // reply = redisCommand(c,"SET %s %s", "foo", "hello world");
            // printf("SET: %s\n", reply->str);
            // freeReplyObject(reply);

            // /* Try a GET */
            // reply = redisCommand(c,"GET foo");
            // printf("GET foo: %s\n", reply->str);
            // freeReplyObject(reply);

            // /* Create a list of numbers, from 0 to 4 */
            // reply = redisCommand(c,"DEL mylist");
            // freeReplyObject(reply);

            //    var cache = new RedisCache(new RedisCacheOptions
            //{
            //    Configuration = $"{REDIS_HOST}:{REDIS_PORT}"
            //});

            try
            {
                //cache.
                //Console.WriteLine($"Setting ['{key}'] to: '{value}'");
                //cache.Set(key, Encoding.UTF8.GetBytes(value), new DistributedCacheEntryOptions());
            }
            catch (StackExchange.Redis.RedisConnectionException e)
            {
                Console.WriteLine("[FATAL] Exception occured when connecting to Redis.");
                Console.WriteLine($"Please check whether the specified host {REDIS_HOST}:{REDIS_PORT} is up and running by running:");
                Console.WriteLine($"redis-cli -h {REDIS_HOST} -p {REDIS_PORT} ping");
                Console.WriteLine($"Full exception details:");
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("[FATAL] Generic Exception occurred when writing a value to Redis, reason unknown.");
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }

        public static void Main(string[] args)
        {
            DoWork().Wait();
        }
    }
}




