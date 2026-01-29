// Decompiled with JetBrains decompiler
// Type: ChangeSkinOnAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using UnityEngine;

#nullable disable
public class ChangeSkinOnAttack : MonoBehaviour
{
  [SerializeField]
  public EnemyBat EnemyBat;
  [SerializeField]
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string DefaultSkin;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string[] AttackSkins;

  public void OnEnable()
  {
    this.EnemyBat.OnAttack += new Action<int>(this.OnAttack);
    this.EnemyBat.OnAttackComplete += new System.Action(this.OnAttackComplete);
  }

  public void OnDisable()
  {
    this.EnemyBat.OnAttack -= new Action<int>(this.OnAttack);
    this.EnemyBat.OnAttackComplete -= new System.Action(this.OnAttackComplete);
  }

  public void OnAttack(int obj) => this.Spine.Skeleton.SetSkin(this.AttackSkins[obj]);

  public void OnAttackComplete() => this.Spine.Skeleton.SetSkin(this.DefaultSkin);
}
