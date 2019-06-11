using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitElement : MonoBehaviour {
	public bool Controlled;
	public bool FlagConrolled;
    public FormType Form;
    Forms.GetCells GetNods;
    public Node CurNode;
	public int MaxDistance;
	public BasicWeapon Weapon;

    public int MaxHP, MaxAP;
	public int HP, AP;
    public void Start()
    {
        GetNods = Forms.forms[(int)Form];
        HP = MaxHP;
        AP = MaxAP;
    }
    public List<Node> WalkableZone() {
        return GetNods(CurNode, MaxDistance, 1);
    }
	public void MoveToNode(Node node) {

		CurNode.Unit = null;
        CurNode = node;
		StartCoroutine(LerpMove(node));
	}
	public void ShootToNode(Node node) {
		Weapon.MakeShoot(node);
        if (node.Unit)
        {
            Weapon.Hit(node.Unit);
        }
	}

	public IEnumerator LerpMove(Node targetNode) {
		FlagConrolled = true;
		Manager.ManagerStatic.CleanGrid();
		var waiter = new WaitForSeconds(1f / 60);

		targetNode.Unit = this;

		Vector3 finalePose = targetNode.transform.position;
		Transform ttf = transform;
		while ((finalePose - ttf.position).sqrMagnitude > 0.001f) {
			ttf.position = Vector3.Lerp(ttf.position, finalePose, 0.15f);
			yield return waiter;
		}

		transform.position = finalePose;
		if (Controlled) Manager.ManagerStatic.EndTurn();
		FlagConrolled = false;

	}
    
}
