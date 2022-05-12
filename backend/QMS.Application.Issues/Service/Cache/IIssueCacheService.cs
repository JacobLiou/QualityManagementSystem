namespace QMS.Application.Issues
{
    public interface IIssueCacheService
    {
        public Task<string> GetUserColumns(long userId);

        public Task SetUserColumns(long userId, string json);

        public Task RemoveUserColumns(long userId);
    }
}