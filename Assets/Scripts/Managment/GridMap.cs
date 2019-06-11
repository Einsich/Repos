using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour {
    public static Node[, ] BattleField;
    public static int Size = 6;
    private static Node lastNode;
    public static Node marcerOnGrid;

    public static void CreateGrid() {
        marcerOnGrid = Resources.LoadAll<Node>("GridMarcer") [0]; // TODO Change to nonarray
        BattleField = new Node[Size, Size];
        Vector3 BotomLeft = Vector3.zero - Vector3.up * (Size / 2) - Vector3.right * (Size / 2);

        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                BattleField[i, j] = Instantiate(marcerOnGrid, BotomLeft + Vector3.right * j + Vector3.up * i, Quaternion.identity);
                BattleField[i, j].InitializeNode(NodeType.Ground, j, i);
            }
        }
    }
    public static bool InsideMap(int x,int y)
    {
        return 0 <= x && x < Size && 0 <= y && y < Size;
    }
}
