using ComponentTradeCenter.Server.Data.Base;
using ConcurrentCollections;

namespace ComponentTradeCenter.Server.Services
{
    public class OnlineUsersService
    {
        protected ConcurrentHashSet<UserBase> OnlineUsersSet { get; } = new ConcurrentHashSet<UserBase>();

        public bool Login(UserBase user)
        {
            return OnlineUsersSet.Add(user);
        }

        public bool IsOnline(UserBase user)
        {
            return OnlineUsersSet.Contains(user);
        }

        public bool Logout(UserBase user)
        {
            return OnlineUsersSet.TryRemove(user);
        }
    }
}
