namespace Hydra.Module.Video.Backend.Controllers;

using System.Threading.Tasks;
using Contracts;
using Models;
using Microsoft.AspNetCore.Mvc;

public class StudentsController : ApiControllerBase
{
    private readonly IStudentService _studentService;
    public StudentsController(ModuleVideoSettings configuration, IStudentService studentService) : base(configuration)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [Route("{id}/groups")]
    public async Task<ActionResult<GroupResponseDto[]>> Get(string id)
    {
        var studentGroups = await _studentService.GetStudentGroups(id);
        return Ok(studentGroups);
    }
}