// Decompiled with JetBrains decompiler
// Type: EnemyHopperBurpPoison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyHopperBurpPoison : EnemyHopperBurp
{
  [SerializeField]
  private PoisonBomb poisonBomb;
  [SerializeField]
  private float bombDuration;

  protected override IEnumerator ShootProjectileRoutine()
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
    Object.Instantiate<PoisonBomb>(hopperBurpPoison.poisonBomb, hopperBurpPoison.targetObject.transform.position, Quaternion.identity, hopperBurpPoison.transform.parent).Play(hopperBurpPoison.transform.position, hopperBurpPoison.bombDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
