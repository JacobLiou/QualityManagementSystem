using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Web.Core
{
    public class RedisEventSourceStorer : IEventSourceStorer
    {
        private readonly IRedisClient _redisClient;
        public RedisEventSourceStorer(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }
        // 往 Redis 中写入一条
        public async ValueTask WriteAsync(IEventSource eventSource, CancellationToken cancellationToken)
        {
            await _redisClient.WriteAsync(...., cancellationToken);
        }
        // 从 Redis 中读取一条
        public async ValueTask<IEventSource> ReadAsync(CancellationToken cancellationToken)
        {
            return await _redisClient.ReadAsync(...., cancellationToken);
        }
    }
}
