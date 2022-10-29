using BookStore.Models;

namespace BookStore.DataAccess.Repository.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);

        void Save();
    }
}
