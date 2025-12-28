using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmsService.Application.Commands;
using EmsService.Application.Queries;
using EmsService.Application.DTOs;

namespace EmsService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnergyReadingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnergyReadingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateEnergyReading([FromBody] CreateEnergyReadingCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetEnergyReading), new { id = result.Value }, result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnergyReadingDto>> GetEnergyReading(string id)
    {
        var query = new GetEnergyReadingQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnergyReadingDto>>> GetAllEnergyReadings()
    {
        var query = new GetAllEnergyReadingsQuery();
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return StatusCode(500, result.Error);
        }

        return Ok(result.Value);
    }
}
