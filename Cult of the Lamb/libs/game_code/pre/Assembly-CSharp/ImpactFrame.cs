// Decompiled with JetBrains decompiler
// Type: ImpactFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ImpactFrame : MonoBehaviour
{
  public Image image;
  private float Duration = 2f;
  private float Delay;
  private bool UseDeltaTime = true;
  private Material clonedMaterial;

  private void OnEnable() => this.image.enabled = false;

  public void ShowForDuration(float _Duration = 0.2f, float _Delay = 0.0f)
  {
    this.image.enabled = true;
    this.Duration = _Duration;
    this.Delay = _Delay;
    this.StartCoroutine((IEnumerator) this.FadeInRoutine());
  }

  public void Show()
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    this.image.enabled = true;
  }

  public void Hide() => this.image.enabled = false;

  private IEnumerator FadeInRoutine()
  {
    ImpactFrame impactFrame = this;
    yield return (object) new WaitForSecondsRealtime(impactFrame.Delay);
    impactFrame.image.enabled = true;
    yield return (object) new WaitForSecondsRealtime(impactFrame.Duration);
    impactFrame.gameObject.SetActive(false);
  }
}
