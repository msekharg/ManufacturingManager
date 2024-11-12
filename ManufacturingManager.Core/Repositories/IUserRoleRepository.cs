using System.Data;

namespace ManufacturingManager.Core.Repositories
{
    public interface IUserRoleRepository
    {
        UserRole UserRole(int profileId);
        Task<IEnumerable<UserRole>> GetUserRoleList();
    }
}
