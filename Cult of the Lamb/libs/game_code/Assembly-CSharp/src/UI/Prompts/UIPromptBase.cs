// Decompiled with JetBrains decompiler
// Type: src.UI.Prompts.UIPromptBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace src.UI.Prompts;

public abstract class UIPromptBase : UIMenuBase
{
  [SerializeField]
  public RectTransform _containerRectTransform;
  public Transform OffsetContainer;
  public CoopIndicatorIcon CoopIndicatorIcon;
  public static List<UIPromptBase> prompts = new List<UIPromptBase>();
  [HideInInspector]
  public PlayerFarming playerFarming;

  public override bool _addToActiveMenus => false;

  public void Start() => this._canvasGroup.alpha = 0.0f;

  public void Init(PlayerFarming playerFarming)
  {
    this.playerFarming = playerFarming;
    if (CoopManager.CoopActive && (Object) this.CoopIndicatorIcon != (Object) null)
    {
      this.CoopIndicatorIcon.SetIcon(playerFarming.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
      this.CoopIndicatorIcon.gameObject.SetActive(true);
    }
    if (!UIPromptBase.prompts.Contains(this))
      UIPromptBase.prompts.Add(this);
    UIPromptBase.UpdateAllPromptPositions();
  }

  public static void UpdateAllPromptPositions()
  {
    bool flag1 = true;
    foreach (UIPromptBase prompt in UIPromptBase.prompts)
    {
      if ((bool) (Object) prompt.playerFarming)
      {
        float x = 0.0f;
        float indicatorYoffset = (float) HUD_Manager.SingleIndicatorYOffset;
        float num = 1f;
        if (CoopManager.CoopActive)
        {
          x = prompt.playerFarming.isLamb ? (float) -HUD_Manager.CoopIndicatorXOffset : (float) HUD_Manager.CoopIndicatorXOffset;
          indicatorYoffset = (float) HUD_Manager.CoopIndicatorYOffset;
          num = HUD_Manager.CoopIndicatorScale;
        }
        prompt.OffsetContainer.transform.localPosition = new Vector3(x, indicatorYoffset, 0.0f);
        prompt.OffsetContainer.transform.localScale = new Vector3(num, num, num);
      }
    }
    bool flag2 = false;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      float x = 0.0f;
      float y = 50f;
      float num = 1f;
      if (flag1)
      {
        if (CoopManager.CoopActive)
        {
          if (index == 0 && (bool) (Object) player && (bool) (Object) player.indicator && (bool) (Object) player.indicator.TopInfoContainer && player.indicator.TopInfoContainer.gameObject.activeSelf)
          {
            CanvasGroup component;
            if (player.indicator.TopInfoContainer.TryGetComponent<CanvasGroup>(out component) && (Object) component != (Object) null && (double) component.alpha > 0.0)
              flag2 = true;
          }
          else if (index == 1 && (bool) (Object) player)
            y = flag2 ? 180f : 115f;
        }
      }
      else if (CoopManager.CoopActive)
      {
        x = player.isLamb ? (float) -HUD_Manager.CoopIndicatorXOffset : (float) HUD_Manager.CoopIndicatorXOffset;
        y = (float) HUD_Manager.CoopIndicatorYOffset;
        num = HUD_Manager.CoopIndicatorScale;
      }
      if ((bool) (Object) player.indicator)
      {
        player.indicator.CoopOffsetTransform.localPosition = new Vector3(x, y, 0.0f);
        player.indicator.CoopOffsetTransform.localScale = new Vector3(num, num, num);
      }
    }
  }

  public void OnEnable()
  {
    if (!UIPromptBase.prompts.Contains(this))
      UIPromptBase.prompts.Add(this);
    UIPromptBase.UpdateAllPromptPositions();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public new void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
    if (UIPromptBase.prompts.Contains(this))
      UIPromptBase.prompts.Remove(this);
    UIPromptBase.UpdateAllPromptPositions();
  }

  public override void OnShowStarted()
  {
    this._canvasGroup.DOKill();
    this._containerRectTransform.DOKill();
    this.Localize();
  }

  public override void OnHideStarted()
  {
    this._canvasGroup.DOKill();
    this._containerRectTransform.DOKill();
  }

  public abstract void Localize();

  public override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIPromptBase uiPromptBase = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
    Vector3 localPosition = uiPromptBase._containerRectTransform.localPosition;
    uiPromptBase._containerRectTransform.localPosition = localPosition + Vector3.up * 50f;
    uiPromptBase._canvasGroup.alpha = 0.0f;
    uiPromptBase._canvasGroup.DOFade(1f, 0.3f);
    uiPromptBase._containerRectTransform.DOLocalMove(localPosition, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override IEnumerator DoHideAnimation()
  {
    UIPromptBase uiPromptBase = this;
    if (PlayerFarming.playersCount > 1)
      Object.Destroy((Object) uiPromptBase.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.transform.position);
    uiPromptBase._canvasGroup.DOFade(0.0f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
