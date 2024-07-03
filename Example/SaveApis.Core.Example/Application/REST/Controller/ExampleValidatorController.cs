using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SaveApis.Core.Example.Application.Models;

namespace SaveApis.Core.Example.Application.REST.Controller;

[ApiController]
[Route("validator")]
public class ExampleValidatorController(IValidator<ExampleValidatorModel> validator) : ControllerBase
{
    [HttpGet("valid")]
    public async Task<IActionResult> Valid()
    {
        var model = new ExampleValidatorModel("Test");

        await validator.ValidateAndThrowAsync(model);

        return Ok();
    }

    [HttpGet("invalid")]
    public async Task<IActionResult> Invalid()
    {
        var model = new ExampleValidatorModel(string.Empty);

        await validator.ValidateAndThrowAsync(model);

        return Ok();
    }
}