using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public static Manager ManagerStatic;
    #region Unit list + Turn
    public List<UnitElement> FriendList, EnemyList;
    public UnitElement CurrentUnit;
    private List<Node> ZoneMarced = new List<Node>();
    public int Turn;
    public Button[] ModeButtons;
    private UnitElement GetCurrentUnit() {
        int n = (Turn + 1) / 2;
        if (Turn % 2 == 0)
            return FriendList[n % FriendList.Count];
        else
            return EnemyList[n % EnemyList.Count];

    }
    #endregion
    #region TurnMode
    public enum EnumTurnMode {
        _moveMode,
        _attackMode,
        _percMode,
        _noneMode,
        _itemMode

    }
    public EnumTurnMode TurnMode;

    public void SetTurnMode(EnumTurnMode mode) {

        CleanGrid();
        if (TurnMode == mode)
            TurnMode = EnumTurnMode._noneMode;
        else
            TurnMode = mode;

    }
    void Update() {
        if (CurrentUnit == null || CurrentUnit.FlagConrolled) return;
        if (CurrentUnit.Controlled) {
            switch (TurnMode) {
                case EnumTurnMode._moveMode:
                    GetMoving();
                    break;
                case EnumTurnMode._attackMode:
                    GetAttack();
                    break;
                    //case EnumTurnMode._percMode:
                    //    GetPerc();
                    //    break;
                    //case EnumTurnMode._itemMode:
                    //   GetItemUsage();
                    //    break;
            }
        }
    }
    void LateUpdate() {

        if (CurrentUnit != null && CurrentUnit.Controlled) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetTurnMode(EnumTurnMode._moveMode);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetTurnMode(EnumTurnMode._attackMode);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetTurnMode(EnumTurnMode._percMode);
        }
    }
    #endregion
    #region Actions

    private void GetMoving() {
        if (ZoneMarced.Count == 0 && !CurrentUnit.FlagConrolled) {
            ZoneMarced = CurrentUnit.WalkableZone();
            foreach (var item in ZoneMarced) {
                if (item.Unit != null) continue;
                item.SetColor(Color.green);
                ColoredMarcer.Add(item);
            }
        }

        var node = Node.GetNode;
        if (ZoneMarced.Contains(node) && node.Unit == null) {
            CurrentUnit.MoveToNode(node);
        }
    }
    void GetAttack() {
        if (ZoneMarced.Count == 0) {
            ZoneMarced = CurrentUnit.Weapon.FireableZone();

            foreach (var item in ZoneMarced) {
                item.SetColor(Color.red);
                if (item.Unit != null) {
                    item.SetColor(Color.yellow);
                }
                ColoredMarcer.Add(item);
            }
        }

        var node = Node.GetNode;
        if (ZoneMarced.Contains(node) && node.Unit != null) {
            CurrentUnit.ShootToNode(node);
            TurnMode = EnumTurnMode._noneMode;
        }
    }

    #endregion
    #region Garbage
    public List<Node> ColoredMarcer = new List<Node>();
    public void CleanGrid() {
        foreach (var item in ColoredMarcer) {
            item.SetColor(Color.white);
        }
        ZoneMarced.Clear();
    }
    #endregion

    void Start() {
        ManagerStatic = this;
        GridMap.CreateGrid();
        SetEnemyOnZone();
        SetFriendOnZone();
        CurrentUnit = GetCurrentUnit();
        ModeButtons[0].onClick.AddListener(() => SetTurnMode(EnumTurnMode._moveMode));
        ModeButtons[1].onClick.AddListener(() => SetTurnMode(EnumTurnMode._attackMode));
        ModeButtons[2].onClick.AddListener(() => SetTurnMode(EnumTurnMode._percMode));
    }
    private void SetEnemyOnZone() {
        foreach (UnitElement item in EnemyList) {
            int size = GridMap.Size;
            item.CurNode = GridMap.BattleField[Random.Range(size - 1, size), Random.Range(0, size)];
            item.transform.position = item.CurNode.transform.position;
            item.CurNode.Unit = item;
        }
    }
    private void SetFriendOnZone()
    {
        foreach (UnitElement item in FriendList)
        {
            int size = GridMap.Size;
            item.CurNode = GridMap.BattleField[Random.Range(0, 1), Random.Range(0, size)];
            item.transform.position = item.CurNode.transform.position;
            item.CurNode.Unit = item;
        }
    }

    public void EndTurn() {
        Turn++;
        CurrentUnit = GetCurrentUnit();
        CleanGrid();
        SetTurnMode( EnumTurnMode._noneMode);
    }

}
