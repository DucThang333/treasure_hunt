using System.ComponentModel.DataAnnotations;

namespace backend.models;

public class TreasureHuntRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Row must be greater than 0")]
    public int Row { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Column must be greater than 0")]
    public int Col { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Treasure must be greater than 0")]
    public int Treasure { get; set; }

    [Required]
    public int[][] Treasure_Map { get; set; } = new int[0][];
}

