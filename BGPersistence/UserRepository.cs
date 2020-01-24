using BGBLL.Interfaces.Persistence;
using BGDomain.Entities;
using BGPersistence.Repositories;

namespace BGPersistence
{
    public class UserRepository : BgRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}