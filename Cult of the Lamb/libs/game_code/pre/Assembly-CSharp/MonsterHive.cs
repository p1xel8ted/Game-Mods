// Decompiled with JetBrains decompiler
// Type: MonsterHive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MonsterHive : BaseMonoBehaviour
{
  public GameObject MonsterPrefab;
  public GameObject Den;
  public GameObject HoomanTrap;
  public Worshipper worshipper;
  private Health health;

  private void Start()
  {
    GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Wild Life/Big Spider") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
    gameObject.transform.position = this.Den.transform.position;
    gameObject.GetComponent<BigSpider>().MonsterDen = this;
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if ((Object) this.worshipper != (Object) null)
      this.worshipper.FreeFromHive();
    this.worshipper = (Worshipper) null;
  }

  private void OnDie(
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

  private void Update()
  {
    if (!((Object) this.worshipper != (Object) null))
      return;
    this.worshipper.transform.position = this.HoomanTrap.transform.position + new Vector3(0.0f, 0.0f, -0.2f);
  }
}
