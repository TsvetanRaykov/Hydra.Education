namespace Hydra.Module.Video.Backend.Contracts;

using Models;
using System.Threading.Tasks;

public interface IStudentService
{
    Task<GroupResponseDto[]> GetStudentGroups(string studentId);
}