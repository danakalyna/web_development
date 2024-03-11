using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityCenterApi.Services.Implementations
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPermission> CreateUserPermissionAsync(UserPermission newUserPermission)
        {
            _context.UserPermissions.Add(newUserPermission);
            await _context.SaveChangesAsync();
            return newUserPermission;
        }

        public async Task<IEnumerable<UserPermission>> GetAllUserPermissionsAsync()
        {
            return await _context.UserPermissions.ToListAsync();
        }

        public async Task<UserPermission> GetUserPermissionByIdAsync(int userPermissionId)
        {
            return await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserPermissionId == userPermissionId);
        }

        public async Task<UserPermission> UpdateUserPermissionAsync(int userPermissionId, UserPermission updatedUserPermission)
        {
            var userPermission = await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserPermissionId == userPermissionId);

            if (userPermission == null)
            {
                return null;
            }

            // Update the properties
            userPermission.PermissionName = updatedUserPermission.PermissionName;
            // ... set other properties

            _context.UserPermissions.Update(userPermission);
            await _context.SaveChangesAsync();
            return userPermission;
        }

        public async Task<bool> DeleteUserPermissionAsync(int userPermissionId)
        {
            var userPermission = await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserPermissionId == userPermissionId);

            if (userPermission == null)
            {
                return false;
            }

            _context.UserPermissions.Remove(userPermission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
