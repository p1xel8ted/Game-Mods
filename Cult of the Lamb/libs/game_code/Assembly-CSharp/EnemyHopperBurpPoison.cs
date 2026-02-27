// Decompiled with JetBrains decompiler
// Type: EnemyHopperBurpPoison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyHopperBurpPoison : EnemyHopperBurp
{
  [SerializeField]
  public PoisonBomb poisonBomb;
  [SerializeField]
  public float bombDuration;

  public override IEnumerator ShootProjectileRoutine()
  {
    EnemyHopperBurpPoison hopperBurpPoison = this;
    if (!((Object) hopperBurpPoison.targetObject == (Object) null))
    {
      CameraManager.shakeCamera(0.2f, hopperBurpPoison.LookAngle);
      Object.Instantiate<PoisonBomb>(hopperBurpPoison.poisonBomb, hopperBurpPoison.targetObject.transform.position, Quaternion.identity, hopperBurpPoison.transform.parent).Play(hopperBurpPoison.transform.position, hopperBurpPoison.bombDuration, hopperBurpPoison.health.team);
      yield return (object) new WaitForEndOfFrame();
    }
  }
}
