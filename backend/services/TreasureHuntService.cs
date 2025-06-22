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

    public Solution FindTreasure(TreasureHuntRequest data)
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

        
        return solutions.FirstOrDefault(solution => solution.TotalEnergyConsumed == solutions.Min(s => s.TotalEnergyConsumed));
    }

    public double CalculateEnergyConsumed(int currentRow, int currentCol, int nextRow, int nextCol) {
        return Math.Sqrt(Math.Pow(nextRow - currentRow, 2) + Math.Pow(nextCol - currentCol, 2));
    }

    public async Task<HistoryTreasure?> GetHistoryTreasureHunt(int id)
    {
        return await _context.HistoryTreasures.FindAsync(id);
    }
}

public class Solution {
    public List<SolutionNode> Nodes { get; set; } = new List<SolutionNode>();
    public double TotalEnergyConsumed { get; set; }
}

public class SolutionNode {
    public int Row { get; set; }
    public int Col { get; set; }
    public int Step { get; set; }
    public int Key { get; set; }
}