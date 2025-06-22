using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using backend.models;
using backend.services;
using backend.db.Models;
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
    public async Task<IActionResult> Post([FromBody] TreasureHuntRequest request)
    {
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

        var (solution, saveSuccess, errorMessage) = await _treasureHuntService.FindTreasure(request);

        var response = new { 
            message = "Treasure hunt request validated successfully",
            solution = solution,
            databaseSave = new {
                success = saveSuccess,
                message = saveSuccess ? "Result saved to database" : "Failed to save result to database",
                error = errorMessage
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Get all treasure hunt history
    /// </summary>
    /// <returns>List of all treasure hunt records</returns>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        try
        {
            var history = await _treasureHuntService.GetAllHistoryTreasureHunt();
            return Ok(new { 
                message = "History retrieved successfully",
                data = history
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    /// <summary>
    /// Get specific treasure hunt history by ID
    /// </summary>
    /// <param name="id">History record ID</param>
    /// <returns>Specific treasure hunt record</returns>
    [HttpGet("history/{id}")]
    public async Task<IActionResult> GetHistoryById(int id)
    {
        try
        {
            var history = await _treasureHuntService.GetHistoryTreasureHunt(id);
            if (history == null)
            {
                return NotFound(new { error = "History record not found" });
            }

            return Ok(new { 
                message = "History record retrieved successfully",
                data = history
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }
}

