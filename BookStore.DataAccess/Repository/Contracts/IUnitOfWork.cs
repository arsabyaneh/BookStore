namespace BookStore.DataAccess.Repository.Contracts
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }

        ICoverTypeRepository CoverTypeRepository { get; }

        void Save();
    }
}
