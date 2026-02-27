// Decompiled with JetBrains decompiler
// Type: CriticalTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CriticalTimer : MonoBehaviour
{
  [SerializeField]
  private Image wheel;
  [SerializeField]
  private CanvasGroup canvasGroup;
  [SerializeField]
  private SkeletonGraphic chargingIcon;

  public void UpdateCharging(float normalised)
  {
    if ((double) normalised > 1.0)
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
