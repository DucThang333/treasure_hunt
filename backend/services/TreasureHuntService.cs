using backend.db;
using backend.db.Models;
using backend.models;
using Microsoft.EntityFrameworkCore;

namespace backend.services;

public class TreasureHuntService
{
    private readonly ApplicationDbContext _context;

    public TreasureHuntService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(Solution solution, bool saveSuccess, string? errorMessage)> FindTreasure(TreasureHuntRequest data)
    {
        // để lưu chìa khóa lớn nhất đã tìm thấy
        int MaxKey = 0;

        // trong trường hợp điểm bắt đầu là dương số 1 (vì chìa khóa số 0 mở được dương 1)
        if (data.Treasure_Map[0][0] == 1) {
            MaxKey = 1;
        }

        List<Solution> solutions = new List<Solution>{
            new Solution{
                Nodes = new List<SolutionNode>{
                    new SolutionNode{
                        Row = 0,
                        Col = 0,
                        Step = 0,
                        Key = MaxKey,
                    }
                },
                TotalEnergyConsumed = 0
            }
        };
        
        // duyệt qua từng dương kho báu bắt buộc phải qua
        for (int i = 0; i < data.Treasure - MaxKey; i++) {
            // duyệt qua tất cả các con đường đi đến kho báu đã tìm thấy
            List<Solution> newSolutions = new List<Solution>();
            for(int j = 0; j < solutions.Count; j++) {
                // duyệt tất cả các kho báu có trong bản đồ
                for(int k = 0; k < data.Treasure_Map.Length; k++) {
                    for(int l = 0; l < data.Treasure_Map[k].Length; l++) {
                        if(data.Treasure_Map[k][l] == solutions[j].Nodes.Last().Key + 1) {
                            // lưu lại con đường đi đến kho báu
                            newSolutions.Add(new Solution{
                                Nodes = new List<SolutionNode>(solutions[j].Nodes){
                                    new SolutionNode{
                                        Row = k,
                                        Col = l,
                                        Step = solutions[j].Nodes.Last().Step + 1,
                                        Key = data.Treasure_Map[k][l]
                                    }
                                    },
                                TotalEnergyConsumed = solutions[j].TotalEnergyConsumed + CalculateEnergyConsumed(solutions[j].Nodes.Last().Row, solutions[j].Nodes.Last().Col, k, l)
                            });
                        }
                    }
                }
            }
            solutions = newSolutions;
        }

        Solution result = solutions.FirstOrDefault(solution => solution.TotalEnergyConsumed == solutions.Min(s => s.TotalEnergyConsumed));
        
        // Try to save data to database, but continue even if it fails
        bool saveSuccess = true;
        string? errorMessage = null;
        
        try
        {
            _context.HistoryTreasures.Add(new HistoryTreasure{
                Row = data.Row,
                Col = data.Col,
                Treasure = data.Treasure,
                Treasure_Map = data.Treasure_Map,
                Minimum_Path = result.TotalEnergyConsumed,
                Track_Path = result.Nodes
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            saveSuccess = false;
            errorMessage = ex.Message;
            Console.WriteLine($"Warning: Failed to save treasure hunt result to database: {ex.Message}");
        }

        return (result, saveSuccess, errorMessage);
    }

    public double CalculateEnergyConsumed(int currentRow, int currentCol, int nextRow, int nextCol) {
        return Math.Sqrt(Math.Pow(nextRow - currentRow, 2) + Math.Pow(nextCol - currentCol, 2));
    }

    public async Task<HistoryTreasure?> GetHistoryTreasureHunt(int id)
    {
        return await _context.HistoryTreasures.FindAsync(id);
    }

    /// <summary>
    /// Get all treasure hunt history records
    /// </summary>
    /// <returns>List of all history records ordered by creation date (newest first)</returns>
    public async Task<List<HistoryTreasure>> GetAllHistoryTreasureHunt()
    {
        return await _context.HistoryTreasures
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }
}

/// <summary>
/// Generic pagination result class
/// </summary>
/// <typeparam name="T">Type of items in the result</typeparam>
public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

