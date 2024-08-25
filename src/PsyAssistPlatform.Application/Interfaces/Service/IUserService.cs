using PsyAssistPlatform.Application.Interfaces.Dto.User;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IUserService
{
    Task<IEnumerable<IUser>> GetActiveUsersAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<IUser>> GetAllUsersAsync(CancellationToken cancellationToken);
    
    Task<IUser?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IUser> CreateUserAsync(ICreateUser userData, CancellationToken cancellationToken);
    
    Task<IUser> UpdateUserAsync(int id, IUpdateUser userData, CancellationToken cancellationToken);
    
    Task UnblockUserAsync(int id, CancellationToken cancellationToken);
    
    Task BlockUserAsync(int id, CancellationToken cancellationToken);
}