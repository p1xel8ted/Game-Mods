// Decompiled with JetBrains decompiler
// Type: StealthCover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StealthCover : BaseMonoBehaviour
{
  public static List<StealthCover> StealthCovers = new List<StealthCover>();
  public float Radius = 1f;
  public Health health;
  public bool ShowGizmos;

  public void OnEnable()
  {
    StealthCover.StealthCovers.Add(this);
    this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    StealthCover.StealthCovers.Remove(this);
  }

  public void OnDisable()
  {
    if ((Object) this.health != (Object) null)
      this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
    StealthCover.StealthCovers.Remove(this);
  }

  public void EndStealth() => StealthCover.StealthCovers.Remove(this);

  public void OnDrawGizmos()
  {
    if (!this.ShowGizmos)
      return;
    Utils.DrawCircleXY(this.transform.position, this.Radius, Color.magenta);
  }
}
