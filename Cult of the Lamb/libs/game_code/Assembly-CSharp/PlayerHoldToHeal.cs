// Decompiled with JetBrains decompiler
// Type: PlayerHoldToHeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerHoldToHeal : BaseMonoBehaviour
{
  public Image ProgressRing;
  public Image FullHealthMask;
  public Tween maskTween;
  public bool cacheAnyPlayerCanHeal = true;

  public void Start() => this.ProgressRing.fillAmount = 0.0f;

  public void Update()
  {
    float num = 0.0f;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((bool) (Object) player)
      {
        if (player.state.CURRENT_STATE == StateMachine.State.Heal)
        {
          player.health.healTimer += Time.deltaTime;
        }
        else
        {
          player.health.healTimer = Mathf.Clamp(player.health.healTimer - Time.deltaTime * 2f, 0.0f, float.MaxValue);
          this.ProgressRing.fillAmount = player.health.healTimer / 1f;
        }
        player.health.healTimer = Mathf.Clamp(player.health.healTimer, 0.0f, 1f);
        if ((double) player.health.healTimer >= (double) num)
        {
          num = player.health.healTimer;
          this.ProgressRing.fillAmount = player.health.healTimer / 1f;
        }
        if (!player.health.ableToHeal && (double) player.health.HP < (double) player.health.totalHP && (double) player.playerSpells.faithAmmo.Ammo >= (double) player.playerSpells.AmmoCost)
          player.health.ableToHeal = true;
        else if (player.health.ableToHeal && ((double) player.health.HP >= (double) player.health.totalHP || (double) player.playerSpells.faithAmmo.Ammo < (double) player.playerSpells.AmmoCost))
          player.health.ableToHeal = false;
      }
    }
    bool flag = false;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((bool) (Object) player && player.health.ableToHeal)
      {
        flag = true;
        break;
      }
    }
    if (this.cacheAnyPlayerCanHeal == flag)
      return;
    this.cacheAnyPlayerCanHeal = flag;
    if (!flag)
    {
      if (this.maskTween != null && this.maskTween.active)
        this.maskTween.Complete();
      this.maskTween = (Tween) DOTweenModuleUI.DOFade(this.FullHealthMask, 0.75f, 0.5f);
    }
    else
    {
      if (this.maskTween != null && this.maskTween.active)
        this.maskTween.Complete();
      this.maskTween = (Tween) DOTweenModuleUI.DOFade(this.FullHealthMask, 0.0f, 0.5f);
    }
  }
}
