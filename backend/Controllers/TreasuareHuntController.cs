using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using backend.models;
using backend.services;
namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreasuareHuntController : ControllerBase
{
    private readonly TreasureHuntService _treasureHuntService;

    public TreasuareHuntController(TreasureHuntService treasureHuntService)
    {
        _treasureHuntService = treasureHuntService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] TreasureHuntRequest request)
    {
        // Validate form request
        if (request.Treasure_Map == null || request.Treasure_Map.Length == 0)
        {
            return BadRequest(new { error = "Treasure_Map cannot be null or empty" });
        }

        if (request.Treasure_Map.Length != request.Row)
        {
            return BadRequest(new { error = $"Treasure_Map must have exactly {request.Row} rows" });
        }

        for (int i = 0; i < request.Treasure_Map.Length; i++)
        {
            if (request.Treasure_Map[i] == null || request.Treasure_Map[i].Length != request.Col)
            {
                return BadRequest(new { error = $"Row {i} must have exactly {request.Col} columns" });
            }
        }

        // Find treasure
        Solution solution = _treasureHuntService.FindTreasure(request);

        // Return response
        return Ok(new { 
            message = "Treasure hunt request validated successfully",
            solution = solution

        });
    }
}

