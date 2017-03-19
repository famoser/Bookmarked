namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IApiService
    {
        IApiRepository<T, CollectionModel> ResolveRepository<T>() where T : ISyncModel;
    }
}
