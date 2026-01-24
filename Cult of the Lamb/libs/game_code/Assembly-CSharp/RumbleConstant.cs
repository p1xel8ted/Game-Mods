// Decompiled with JetBrains decompiler
// Type: RumbleConstant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RumbleConstant : MonoBehaviour
{
  [Range(0.0f, 10f)]
  [SerializeField]
  public float LowFrequency;
  [Range(0.0f, 10f)]
  [SerializeField]
  public float HighFrequency = 1f;
  [SerializeField]
  public bool PlayOnEnable;
  [SerializeField]
  public bool PlaySfx;

  public void OnEnable()
  {
    if (!this.PlayOnEnable)
      return;
    this.StartRumble();
  }

  public void OnDisable() => this.StopRumble();

  public void StartRumble()
  {
    MMVibrate.RumbleContinuous(this.LowFrequency, this.HighFrequency);
    if (!this.PlaySfx)
      return;
    MMVibrate.rumbleEventInstance = AudioManager.Instance.PlayOneShotWithInstanceDontStart("event:/comic sfx/temple_door_hum 2");
    int num = (int) MMVibrate.rumbleEventInstance.start();
  }

  public void StopRumble()
  {
    MMVibrate.StopRumble();
    if (!this.PlaySfx || !MMVibrate.rumbleEventInstance.isValid())
      return;
    AudioManager.Instance.StopLoop(MMVibrate.rumbleEventInstance);
  }
}
