// Decompiled with JetBrains decompiler
// Type: SpawnDeadBodyOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SpawnDeadBodyOnDeath : BaseMonoBehaviour
{
  private Health health;
  public DeadBodySliding deadBodySliding;
  public float Speed = 1700f;

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDisable() => this.health.OnDie -= new Health.DieAction(this.OnDie);

  public void ReEnable(float Speed)
  {
    this.enabled = true;
    this.Speed = Speed;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    GameObject gameObject = this.deadBodySliding.gameObject.Spawn();
    DeadBodySliding component = gameObject.GetComponent<DeadBodySliding>();
    gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    gameObject.transform.parent = this.transform.parent;
    bool explode = AttackFlags.HasFlag((Enum) Health.AttackFlags.Skull);
    if (!(bool) (UnityEngine.Object) component || !(bool) (UnityEngine.Object) Attacker)
      return;
    component.Init(this.gameObject, Utils.GetAngle(Attacker.transform.position, this.transform.position), this.Speed, explode);
  }
}
