using System.Collections.Generic;
using System.Threading.Tasks;

namespace sharpbox.Membership.Model
{
    using System;

    public class UserClaimStore : Microsoft.AspNet.Identity.IUserClaimStore<User, Guid>
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            throw new System.NotImplementedException();
        }
    }
}
