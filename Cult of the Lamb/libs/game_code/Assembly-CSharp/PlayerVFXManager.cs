// Decompiled with JetBrains decompiler
// Type: PlayerVFXManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class PlayerVFXManager : BaseMonoBehaviour
{
  public SimpleSFX RedThunderStrike;
  public GameObject chargingParticles;
  public SkeletonRenderer playerSpine;
  public StateMachine state;

  public void EmitGhostTwirlAttack() => this.stopEmitChargingParticles();

  public void emitChargingParticles() => this.chargingParticles.SetActive(true);

  public void stopEmitChargingParticles() => this.chargingParticles.SetActive(false);
}
