// Decompiled with JetBrains decompiler
// Type: StealthCover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StealthCover : BaseMonoBehaviour
{
  public static List<StealthCover> StealthCovers = new List<StealthCover>();
  public float Radius = 1f;
  private Health health;
  public bool ShowGizmos;

  private void OnEnable()
  {
    StealthCover.StealthCovers.Add(this);
    this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
  }

  private void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    StealthCover.StealthCovers.Remove(this);
  }

  private void OnDisable()
  {
    if ((Object) this.health != (Object) null)
      this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
    StealthCover.StealthCovers.Remove(this);
  }

  public void EndStealth() => StealthCover.StealthCovers.Remove(this);

  private void OnDrawGizmos()
  {
    if (!this.ShowGizmos)
      return;
    Utils.DrawCircleXY(this.transform.position, this.Radius, Color.magenta);
  }
}
