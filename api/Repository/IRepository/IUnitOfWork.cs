namespace api.Repository.IRepository
{
    public interface IUnitOfWork
    { 
        public IStockRepository Stock { get; }
        Task SaveAsync();
    }
}
