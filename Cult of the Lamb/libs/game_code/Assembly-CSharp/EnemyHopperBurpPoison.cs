// Decompiled with JetBrains decompiler
// Type: EnemyHopperBurpPoison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyHopperBurpPoison hopperBurpPoison = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    CameraManager.shakeCamera(0.2f, hopperBurpPoison.LookAngle);
    Object.Instantiate<PoisonBomb>(hopperBurpPoison.poisonBomb, hopperBurpPoison.targetObject.transform.position, Quaternion.identity, hopperBurpPoison.transform.parent).Play(hopperBurpPoison.transform.position, hopperBurpPoison.bombDuration, hopperBurpPoison.health.team);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
