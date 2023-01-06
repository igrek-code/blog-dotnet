using Bloogs.Entities;

namespace Bloogs.Data
{
    public interface IUserService
    {
        Task<User> SuperVisorAccountSeed();
    }
}
