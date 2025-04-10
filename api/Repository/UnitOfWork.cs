using api.Data;
using api.Repository.IRepository;

namespace api.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IStockRepository Stock { get; private set; }
        public ICommentRepository Comment{ get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Stock = new StockRepository(_db);
            Comment = new CommentReposiotry(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
