using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace backend.db.Models;

public class HistoryTreasure
{
    public int Id { get; set; }
    
    [Required]
    public int Row { get; set; }
    
    [Required]
    public int Col { get; set; }

    [Required]
    public int Treasure { get; set; }
    
    [Required]
    public string Treasure_MapJson { get; set; } = string.Empty;

    [NotMapped]
    public int[][] Treasure_Map
    {
        get => JsonSerializer.Deserialize<int[][]>(Treasure_MapJson) ?? new int[0][];
        set => Treasure_MapJson = JsonSerializer.Serialize(value);
    }

    [Required]
    public double Minimum_Path { get; set; }

    [Required]
    public string Track_PathJson { get; set; } = string.Empty;

    [NotMapped]
    public List<int> Track_Path
    {
        get => JsonSerializer.Deserialize<List<int>>(Track_PathJson) ?? new List<int>();
        set => Track_PathJson = JsonSerializer.Serialize(value);
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 