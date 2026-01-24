// Decompiled with JetBrains decompiler
// Type: BossEntranceDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
