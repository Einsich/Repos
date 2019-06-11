using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node:MonoBehaviour, IPointerDownHandler {
    public NodeType Type;
    public UnitElement Unit;
    private SpriteRenderer nodeRender;
    public int x, y;

    public void InitializeNode( NodeType type,int j,int i) {
        Type = type;
        nodeRender = GetComponent<SpriteRenderer>();
        x = j;
        y = i;
    }
    public void SetColor(Color color = default(Color)) {
        nodeRender.color = color;
    }
    static float TimeLastTap = 0;
    static Node CurTap = null;
    public static Node GetNode { get { return Time.time - TimeLastTap <= Time.deltaTime ? CurTap : null; } }

    public void OnPointerDown(PointerEventData eventData)
    {
        CurTap = this;
        TimeLastTap = Time.time;
        if (Unit)
            MySlider.SetUnit(Unit);
    }
}
public enum NodeType {
    Ground,
    UnWalkable,
    UnFireable,
    BlockWall
}
