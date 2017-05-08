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

        public static void Main(string[] args)
        {
            try
            {
                using (var redisManager = new PooledRedisClientManager($"{REDIS_HOST}:{REDIS_PORT}") { ConnectTimeout = 500 })
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
                                lock (redis)
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

                    /* Let's check what we have inside the list */
                    var list = redis.GetRangeFromList("mylist", 0, -1);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"{i}) {list[i]}");
                    }

                    redis.Dispose();
                }
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
                Console.WriteLine("[FATAL] Generic Exception occurred, reason unknown.");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
