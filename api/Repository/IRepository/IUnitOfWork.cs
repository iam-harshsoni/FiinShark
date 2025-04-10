namespace api.Repository.IRepository
{
    public interface IUnitOfWork
    { 
        public IStockRepository Stock { get; }
        public ICommentRepository Comment{ get; }
        Task SaveAsync();
    }
}
