using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Forms
{
    public delegate List<Node> GetCells(Node from, int distance,int mindistance);

    public static GetCells[] forms = { Cross, Diagonal };

    static int[] dxCross = { 1, 0, -1, 0 }, dyCross = { 0, 1, 0, -1 },
        dxDiag = { 1, 1, -1, -1 }, dyDiag = { 1, -1, -1, 1 };

    static List<Node> Cross(Node from,int distance, int mindistance=1)
    {
        List<Node> result = new List<Node>();
        int x = from.x, y = from.y;
        for (var d = 0; d < 4; d++)
            for (var i = mindistance; i <= distance; i++)
                if (GridMap.InsideMap(x + i * dxCross[d], y + i * dyCross[d]))
                {
                    var nodes = GridMap.BattleField[y + i * dyCross[d], x + i * dxCross[d]];
                    if (nodes.Type == NodeType.Ground)
                        result.Add(nodes);
                    if (nodes.Type == NodeType.BlockWall)
                        break;
                }
        return result;
    }

    static List<Node> Diagonal(Node from, int distance, int mindistance=1)
    {
        List<Node> result = new List<Node>();
        int x = from.x, y = from.y;
        for (var d = 0; d < 4; d++)
            for (var i = mindistance; i <= distance; i++)
                if (GridMap.InsideMap(x + i * dxDiag[d], y + i * dyDiag[d]))
                {
                    var nodes = GridMap.BattleField[y + i * dyDiag[d], x + i * dxDiag[d]];
                    if (nodes.Type == NodeType.Ground)
                        result.Add(nodes);
                    if (nodes.Type == NodeType.BlockWall)
                        break;
                }
        return result;
    }
}
public enum FormType
{
    Cross,
    Diagonal
}