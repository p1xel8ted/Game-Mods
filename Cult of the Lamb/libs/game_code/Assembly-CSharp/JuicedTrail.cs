// Decompiled with JetBrains decompiler
// Type: JuicedTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class JuicedTrail : MonoBehaviour
{
  public SimpleSpineDeactivateAfterPlay SimpleSpineDeactivateAfterPlay;
  public EnemyBurrowingTrail EnemyBurrowingTrail;
  public SkeletonAnimation SkeletonAnimation;
  public ColliderEvents ColliderEvents;

  public void SetContinious(bool continous)
  {
    this.SimpleSpineDeactivateAfterPlay.enabled = !continous;
    this.EnemyBurrowingTrail.enabled = !continous;
    this.SkeletonAnimation.AnimationState.GetCurrent(0).Loop = continous;
    this.EnemyBurrowingTrail.DamageCollider.enabled = continous;
  }

  public void Update()
  {
    if (this.EnemyBurrowingTrail.enabled)
      return;
    this.EnemyBurrowingTrail.DamageCollider.enabled = true;
  }
}
