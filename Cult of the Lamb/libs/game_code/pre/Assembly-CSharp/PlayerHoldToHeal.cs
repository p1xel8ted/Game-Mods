// Decompiled with JetBrains decompiler
// Type: PlayerHoldToHeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerHoldToHeal : BaseMonoBehaviour
{
  public Image ProgressRing;
  public Image FullHealthMask;
  private float Timer;
  private bool ableToHeal;
  private Tween maskTween;

  private void Start() => this.ProgressRing.fillAmount = 0.0f;

  private void Update()
  {
    if (!(bool) (Object) PlayerFarming.Instance)
      return;
    if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Heal)
    {
      this.Timer += Time.deltaTime;
      this.ProgressRing.fillAmount = this.Timer / 1f;
    }
    else
    {
      this.Timer = Mathf.Clamp(this.Timer - Time.deltaTime * 2f, 0.0f, float.MaxValue);
      this.ProgressRing.fillAmount = this.Timer / 1f;
    }
    if (!this.ableToHeal && (double) PlayerFarming.Instance.health.HP < (double) PlayerFarming.Instance.health.totalHP && (double) FaithAmmo.Ammo >= (double) FaithAmmo.Total)
    {
      if (this.maskTween != null && this.maskTween.active)
        this.maskTween.Complete();
      this.maskTween = (Tween) DOTweenModuleUI.DOFade(this.FullHealthMask, 0.0f, 0.5f);
      this.ableToHeal = true;
    }
    else
    {
      if (!this.ableToHeal || (double) PlayerFarming.Instance.health.HP < (double) PlayerFarming.Instance.health.totalHP && (double) FaithAmmo.Ammo >= (double) FaithAmmo.Total)
        return;
      if (this.maskTween != null && this.maskTween.active)
        this.maskTween.Complete();
      this.maskTween = (Tween) DOTweenModuleUI.DOFade(this.FullHealthMask, 0.75f, 0.5f);
      this.ableToHeal = false;
    }
  }
}
