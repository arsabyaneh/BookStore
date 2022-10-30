using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.Contracts;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            CategoryRepository  = new CategoryRepository(dbContext);
            CoverTypeRepository = new CoverTypeRepository(dbContext);
            ProductRepository = new ProductRepository(dbContext);
        }

        public ICategoryRepository CategoryRepository { get; private set; }

        public ICoverTypeRepository CoverTypeRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
