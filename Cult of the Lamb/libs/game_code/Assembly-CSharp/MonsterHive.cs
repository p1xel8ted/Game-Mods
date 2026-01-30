// Decompiled with JetBrains decompiler
// Type: MonsterHive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MonsterHive : BaseMonoBehaviour
{
  public GameObject MonsterPrefab;
  public GameObject Den;
  public GameObject HoomanTrap;
  public Worshipper worshipper;
  public Health health;

  public void Start()
  {
    GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Wild Life/Big Spider") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
    gameObject.transform.position = this.Den.transform.position;
    gameObject.GetComponent<BigSpider>().MonsterDen = this;
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if ((Object) this.worshipper != (Object) null)
      this.worshipper.FreeFromHive();
    this.worshipper = (Worshipper) null;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((Object) this.worshipper != (Object) null)
      this.worshipper.FreeFromHive();
    this.worshipper = (Worshipper) null;
    int num = -1;
    while (++num < 1)
      Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Wild Life/Big Spider") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true).transform.position = this.Den.transform.position;
  }

  public void Update()
  {
    if (!((Object) this.worshipper != (Object) null))
      return;
    this.worshipper.transform.position = this.HoomanTrap.transform.position + new Vector3(0.0f, 0.0f, -0.2f);
  }
}
