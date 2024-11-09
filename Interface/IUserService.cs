using RaceConditionSample.Models;

namespace RaceConditionSample.Interface;

public interface IUserService
{
    Task<IReadOnlyCollection<UserListItem>> GetUsersAsync();

    Task<UserListItem?> GetUserByIdAsync(int userId);

    Task<bool> CreateUserAsync(UserDto userDto);

    Task<bool> CreateUserAsyncKeyedLock(UserDto userDto);
}