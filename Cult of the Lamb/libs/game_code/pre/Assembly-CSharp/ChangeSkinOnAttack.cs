// Decompiled with JetBrains decompiler
// Type: ChangeSkinOnAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using UnityEngine;

#nullable disable
public class ChangeSkinOnAttack : MonoBehaviour
{
  [SerializeField]
  private EnemyBat EnemyBat;
  [SerializeField]
  private SkeletonAnimation Spine;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  private string DefaultSkin;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  private string[] AttackSkins;

  private void OnEnable()
  {
    this.EnemyBat.OnAttack += new Action<int>(this.OnAttack);
    this.EnemyBat.OnAttackComplete += new System.Action(this.OnAttackComplete);
  }

  private void OnDisable()
  {
    this.EnemyBat.OnAttack -= new Action<int>(this.OnAttack);
    this.EnemyBat.OnAttackComplete -= new System.Action(this.OnAttackComplete);
  }

  private void OnAttack(int obj) => this.Spine.Skeleton.SetSkin(this.AttackSkins[obj]);

  private void OnAttackComplete() => this.Spine.Skeleton.SetSkin(this.DefaultSkin);
}
