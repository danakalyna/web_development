using CommunityCenterApi.Models;
using CommunityCenterApi.DB;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.Services.Interfaces;
using CommunityCenterApi.Helpers;

namespace CommunityCenterApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User newUser)
        {
            newUser.PasswordHash = PasswordHasher.HashPassword(newUser.PasswordHash);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> UpdateUserAsync(Guid userId, User updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return null;
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            // ... other properties

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _context.Users
                                             .AsNoTracking()
                                             .SingleOrDefaultAsync(u => u.Email == username);

            if (user == null)
            {
                return null;
            }

            // Assume we have a method that gets the hashed password from the database.
            // For example, 'GetUserHashedPassword' would retrieve the stored hash for the given user.
            var hashedPassword = user.PasswordHash;

            // Now we use the PasswordHasher helper to check the provided password against the stored hash.
            var verificationResult = PasswordHasher.CheckPassword(hashedPassword, password);

            if (!verificationResult.Verified)
            {
                return null;
            }

            // The 'Verified' property tells us if the password was correct.
            return user;
        }
    }
}
