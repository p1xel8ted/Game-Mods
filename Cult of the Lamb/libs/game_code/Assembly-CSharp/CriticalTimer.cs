// Decompiled with JetBrains decompiler
// Type: CriticalTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CriticalTimer : MonoBehaviour
{
  [SerializeField]
  public Image wheel;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public SkeletonGraphic chargingIcon;

  public void UpdateCharging(float normalised)
  {
    if ((double) normalised >= 1.0)
    {
      if (!this.chargingIcon.enabled)
      {
        AudioManager.Instance.PlayOneShot("event:/weapon/crit_ready", this.gameObject);
        this.chargingIcon.enabled = true;
      }
      if (this.chargingIcon.AnimationState.GetCurrent(0).Animation.Name != "charged")
        this.chargingIcon.AnimationState.SetAnimation(0, "charged", true);
      if ((double) this.canvasGroup.alpha != 1.0)
        return;
      this.canvasGroup.DOKill();
      this.canvasGroup.DOFade(0.0f, 0.25f);
    }
    else
    {
      if (this.chargingIcon.enabled)
        this.chargingIcon.enabled = false;
      if ((double) this.canvasGroup.alpha != 1.0)
        return;
      this.canvasGroup.DOKill();
      this.canvasGroup.DOFade(0.0f, 0.25f);
    }
  }
}
