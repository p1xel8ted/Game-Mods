// Decompiled with JetBrains decompiler
// Type: BossEntranceDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BossEntranceDoor : BaseMonoBehaviour
{
  public Bounds LimitCameraBounds;

  public void OnEnable()
  {
    GameManager.GetInstance().CamFollowTarget.SetCameraLimits(this.LimitCameraBounds);
  }

  public void OnDisable() => GameManager.GetInstance().CamFollowTarget.DisableCameraLimits();
}
