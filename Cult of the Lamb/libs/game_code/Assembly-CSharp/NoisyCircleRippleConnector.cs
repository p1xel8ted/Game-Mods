// Decompiled with JetBrains decompiler
// Type: NoisyCircleRippleConnector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NoisyCircleRippleConnector : MonoBehaviour
{
  public NoisyCircleRippleManager rippleManager;
  public Transform rippleSpawnPoint;

  public void OnTriggered()
  {
    this.rippleManager.UpdateSpawnPoint(this.rippleSpawnPoint);
    this.rippleManager.EmitRipple();
  }
}
