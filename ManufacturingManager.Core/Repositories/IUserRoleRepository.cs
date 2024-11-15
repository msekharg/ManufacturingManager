using System.Data;

namespace ManufacturingManager.Core.Repositories
{
    public interface IUserRoleRepository
    {
        UserRole UserRole(int profileId);
        IList<UserRole> GetUserRoleList();
    }
}
