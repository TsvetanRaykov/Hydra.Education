using System.Threading.Tasks;
using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts;

public interface IStudentsService
{
    Task<StudentDto[]> GetStudentsAsync();
    Task<VideoGroup[]> GetStudentGroups(string studentId);
}