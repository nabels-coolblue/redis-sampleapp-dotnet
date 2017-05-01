using System;
using ServiceStack.Redis;

namespace netcore_redis_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new RedisManagerPool("192.168.56.11:6379");
            using (var client = manager.GetClient())
            {
                client.Set("date", DateTime.UtcNow);
            }
        }
    }
}
