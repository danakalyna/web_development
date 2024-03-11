using CommunityCenterApi.Models;

namespace CommunityCenterApi.Services.Interfaces
{
    public interface IUserPermissionService
    {
        Task<UserPermission> CreateUserPermissionAsync(UserPermission newUserPermission);
        Task<IEnumerable<UserPermission>> GetAllUserPermissionsAsync();
        Task<UserPermission> GetUserPermissionByIdAsync(int userPermissionId);
        Task<UserPermission> UpdateUserPermissionAsync(int userPermissionId, UserPermission updatedUserPermission);
        Task<bool> DeleteUserPermissionAsync(int userPermissionId);
    }
}
