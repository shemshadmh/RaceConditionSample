using AsyncKeyedLock;
using Microsoft.EntityFrameworkCore;
using RaceConditionSample.Entities;
using RaceConditionSample.Interface;
using RaceConditionSample.Models;

namespace RaceConditionSample.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    private static readonly AsyncKeyedLocker<string> _asyncKeyedLocker = new();

    public async Task<IReadOnlyCollection<UserListItem>> GetUsersAsync()
    {
        return await _context.Set<User>().AsNoTracking()
            .Select(user => new UserListItem()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            })
            .ToListAsync();
    }

    public async Task<UserListItem?> GetUserByIdAsync(int userId)
    {
        return await _context.Set<User>().AsNoTracking()
            .Select(user => new UserListItem()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            })
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<bool> CreateUserAsync(UserDto userDto)
    {
        // Check if the username already exists
        if (await UsernameExistsAsync(userDto.Username))
            return false;

        // Check if the email already exists
        if (await EmailExistsAsync(userDto.Email))
            return false;

        var user = new User()
        {
            Email = userDto.Email,
            Username = userDto.Username,
        };

        try
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateUserAsyncKeyedLock(UserDto userDto)
    {
        // Check if the username already exists
        if (await UsernameExistsAsync(userDto.Username))
            return false;

        // Lock the email check
        using (await _asyncKeyedLocker.LockAsync("EmailCheck"))
        {
            // Check if the email already exists
            if (await EmailExistsAsync(userDto.Email))
                return false;
        }

        var user = new User()
        {
            Email = userDto.Email,
            Username = userDto.Username,
        };

        try
        {
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Set<User>().AsNoTracking()
            .AnyAsync(user => user.Username == username);
    }

    private async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Set<User>().AsNoTracking()
            .AnyAsync(user => user.Email == email);
    }
}