using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour {
    public FormType Form;
    Forms.GetCells GetNods;
    public UnitElement Unit {
        get {
            if (myunit == null)
                myunit = transform.GetComponentInParent<UnitElement>();
            return myunit;
        }
    }
    private UnitElement myunit;
    /// <summary>
    /// значения в процентах 
    /// </summary>
    public int DamageForHP, DamageForAP;
    public int Damage;
    public int Mindistance, MaxDistance, BulletPerShoot;
    public float WaitTime;
    public AudioClip Sound;
    private GameObject _bullet;
    private UnitElement targetUnit;

    public void Start()
    {
        GetNods = Forms.forms[(int)Form];
    }
    public List<Node> FireableZone()
    {
        return GetNods(Unit.CurNode, MaxDistance, Mindistance);
    }
    public virtual void MakeShoot(Node node) {

        if (_bullet == null) _bullet = Resources.Load<GameObject>("Staff/Bullet") as GameObject;
        if (node.Unit == null) return;
        targetUnit = node.Unit;
        StartCoroutine(MoveBullet(node));
    }
    public void Hit(UnitElement enemy)
    {
        int HPD = (int)(DamageForHP * 0.01f * Damage);
        int APD = (int)(DamageForAP * 0.01f * Damage);
        HPD = enemy.AP - APD < APD ? APD - (enemy.AP - APD) : 0;
        enemy.HP = Mathf.Clamp(enemy.HP - HPD, 0, enemy.HP);
        enemy.AP = Mathf.Clamp(enemy.AP - APD, 0, enemy.AP);
    }
    public virtual IEnumerator MoveBullet(Node targetNode) {
        Unit.FlagConrolled = true;
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < BulletPerShoot; i++) {
            //Manager.ManagerStatic.PlaySpecific(Sound); TODO make sound player on manger

            Vector3 targetPosition = targetNode.transform.position;
            Vector3 selfPosition = transform.position + (targetPosition - transform.position).normalized * 0.01f;

            if (targetUnit == null) yield break;
            GameObject localBullet = Instantiate(_bullet, selfPosition, Quaternion.identity);
            float distantion = Vector3.Distance(selfPosition, targetPosition);
            while (Vector3.Distance(localBullet.transform.position, targetPosition) > 0.05f) {
                localBullet.transform.Translate((targetPosition - selfPosition) / 10);//moving bullet
                yield return new WaitForFixedUpdate();
            }

            //targetUnit.unit.DamageSystem.GetBlood(localBullet.transform.position, transform.position);
            Destroy(localBullet);
            yield return new WaitForSeconds(WaitTime);

        }

        //targetUnit.GetDamage(Damage)
        Unit.FlagConrolled = false;
        Manager.ManagerStatic.EndTurn();
    }

}
