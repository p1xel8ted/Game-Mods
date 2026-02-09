// Decompiled with JetBrains decompiler
// Type: NoisyCircleRippleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NoisyCircleRippleManager : MonoBehaviour
{
  public NoisyCircleRippleController ripple1;
  public NoisyCircleRippleController ripple2;
  public NoisyCircleRippleController ripple3;
  public bool running;

  public void EmitRipplePosition(Vector3 position)
  {
    Debug.Log((object) "Emit Ripple");
    if (this.running)
      return;
    this.StartCoroutine((IEnumerator) this.EmitRippleRoutine());
  }

  public void UpdateSpawnPoint(Transform position)
  {
    this.ripple1.spawnPoint = position;
    this.ripple2.spawnPoint = position;
    this.ripple3.spawnPoint = position;
  }

  public void EmitRipple()
  {
    Debug.Log((object) "Emit Ripple");
    if (this.running)
      return;
    this.StartCoroutine((IEnumerator) this.EmitRippleRoutine());
  }

  public IEnumerator EmitRippleRoutine()
  {
    this.running = true;
    this.ripple1.TriggerAtUV();
    yield return (object) new WaitForSeconds(0.18f);
    this.ripple2.TriggerAtUV();
    yield return (object) new WaitForSeconds(0.2f);
    this.ripple3.TriggerAtUV();
    this.running = false;
  }
}
