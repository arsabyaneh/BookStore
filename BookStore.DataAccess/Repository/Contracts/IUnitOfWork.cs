namespace BookStore.DataAccess.Repository.Contracts
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }

        void Save();
    }
}
