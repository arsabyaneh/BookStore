using BookStore.Models;

namespace BookStore.DataAccess.Repository.Contracts
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
