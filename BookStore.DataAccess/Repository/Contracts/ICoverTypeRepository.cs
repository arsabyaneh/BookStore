using BookStore.Models;

namespace BookStore.DataAccess.Repository.Contracts
{
    public interface ICoverTypeRepository
    {
        void Update(CoverType coverType);
    }
}
