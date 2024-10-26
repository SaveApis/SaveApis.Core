using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Core.Example.Application.Models;
using SaveApis.Core.Example.Application.Models.DTO;

namespace SaveApis.Core.Example.Application.Backend.REST.Controller;

[ApiController]
[Route("mapper")]
public class ExampleMapperController(IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var model = new ExampleMapperModel(1, "Test");
        var dto = mapper.Map<ExampleMapperDto>(model);

        return Ok(dto);
    }
}