// Decompiled with JetBrains decompiler
// Type: ImpactFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ImpactFrame : MonoBehaviour
{
  public Image image;
  public float Duration = 2f;
  public float Delay;
  public bool UseDeltaTime = true;
  public Material clonedMaterial;

  public void OnEnable() => this.image.enabled = false;

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

  public IEnumerator FadeInRoutine()
  {
    ImpactFrame impactFrame = this;
    yield return (object) new WaitForSecondsRealtime(impactFrame.Delay);
    impactFrame.image.enabled = true;
    yield return (object) new WaitForSecondsRealtime(impactFrame.Duration);
    impactFrame.gameObject.SetActive(false);
  }
}
