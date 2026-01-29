// Decompiled with JetBrains decompiler
// Type: BossEntranceDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
