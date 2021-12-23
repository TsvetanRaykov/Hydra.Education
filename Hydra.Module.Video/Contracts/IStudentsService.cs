using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts;

public interface IStudentsService
{
    Task<StudentDto[]> GetStudentsAsync();
    Task<bool> AddStudentsToGroup(string[] studentIds, string groupId);
    Task<bool> RemoveStudentsFromGroup(string[] studentIds, string groupId);
}