// Decompiled with JetBrains decompiler
// Type: PlayerVFXManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class PlayerVFXManager : BaseMonoBehaviour
{
  public SimpleSFX RedThunderStrike;
  public GameObject chargingParticles;
  public SkeletonRenderer playerSpine;
  public StateMachine state;

  public void EmitGhostTwirlAttack()
  {
    BiomeConstants.Instance.EmitGhostTwirlAttack(this.transform.position, this.state.facingAngle);
    this.stopEmitChargingParticles();
  }

  public void emitChargingParticles() => this.chargingParticles.SetActive(true);

  public void stopEmitChargingParticles() => this.chargingParticles.SetActive(false);
}
