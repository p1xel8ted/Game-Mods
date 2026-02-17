// Decompiled with JetBrains decompiler
// Type: BreakableCrystal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BreakableCrystal : LongGrass
{
  [Header("Crystal")]
  [SerializeField]
  public GameObject crystalObject;
  [SerializeField]
  public GameObject rubbleObject;

  public void Awake() => this.health.OnDie += new Health.DieAction(this.OnDie);

  public override void SetCut()
  {
    this.SwitchCrystalState(true);
    base.SetCut();
  }

  public override void ResetCut()
  {
    this.SwitchCrystalState(false);
    base.ResetCut();
  }

  public new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.SwitchCrystalState(true);
  }

  public void SwitchCrystalState(bool isDestroyed)
  {
    this.crystalObject.SetActive(!isDestroyed);
    this.rubbleObject.SetActive(isDestroyed);
    this.health.enabled = !isDestroyed;
  }
}
