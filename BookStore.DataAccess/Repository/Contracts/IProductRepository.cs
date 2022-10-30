using BookStore.Models;

namespace BookStore.DataAccess.Repository.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
