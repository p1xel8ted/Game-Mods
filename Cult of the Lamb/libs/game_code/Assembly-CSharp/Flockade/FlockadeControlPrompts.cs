// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeControlPrompts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Rewired;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeControlPrompts : UIMenuControlPrompts
{
  [Header("Flockade")]
  [SerializeField]
  public MMControlPrompt _acceptPrompt;
  [SerializeField]
  public MMControlPrompt _backPrompt;
  [SerializeField]
  public Localize _acceptPromptText;
  [SerializeField]
  public Localize _backPromptText;
  [SerializeField]
  [TermsPopup("")]
  public string _cancel;
  [SerializeField]
  [TermsPopup("")]
  public string _fastForward;
  [SerializeField]
  [TermsPopup("")]
  public string _skip;
  public string _accept;
  public string _back;
  [CompilerGenerated]
  public bool \u003CIsAccepting\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CIsSkipping\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsFastForwarding\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsBacking\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CIsCancelling\u003Ek__BackingField;

  public bool IsAccepting
  {
    get => this.\u003CIsAccepting\u003Ek__BackingField;
    set => this.\u003CIsAccepting\u003Ek__BackingField = value;
  }

  public bool IsSkipping
  {
    get => this.\u003CIsSkipping\u003Ek__BackingField;
    set => this.\u003CIsSkipping\u003Ek__BackingField = value;
  }

  public bool IsFastForwarding
  {
    get => this.\u003CIsFastForwarding\u003Ek__BackingField;
    set => this.\u003CIsFastForwarding\u003Ek__BackingField = value;
  }

  public bool IsBacking
  {
    get => this.\u003CIsBacking\u003Ek__BackingField;
    set => this.\u003CIsBacking\u003Ek__BackingField = value;
  }

  public bool IsCancelling
  {
    get => this.\u003CIsCancelling\u003Ek__BackingField;
    set => this.\u003CIsCancelling\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    this._accept = this._acceptPromptText.Term;
    this._back = this._backPromptText.Term;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    MonoSingleton<UINavigatorNew>.Instance.OnInputAllowedOnlyFromSpecificPlayerChanged += new Action<PlayerFarming>(this.UpdateForCurrentPlayer);
  }

  public override void OnDisable()
  {
    if ((bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance)
      MonoSingleton<UINavigatorNew>.Instance.OnInputAllowedOnlyFromSpecificPlayerChanged -= new Action<PlayerFarming>(this.UpdateForCurrentPlayer);
    base.OnDisable();
  }

  public void UpdateForCurrentPlayer(PlayerFarming playerFarming)
  {
    this.playerFarming = playerFarming;
    this.OnActiveControllerChanged((Controller) null);
    this._acceptPrompt.playerFarming = playerFarming;
    this._acceptPrompt.ForceUpdate();
    this._backPrompt.playerFarming = playerFarming;
    this._backPrompt.ForceUpdate();
  }

  public new void ShowAcceptButton()
  {
    if (this._acceptPromptText.isActiveAndEnabled)
      return;
    base.ShowAcceptButton();
    this.ForceRebuild();
    this.IsAccepting = true;
  }

  public new void HideAcceptButton()
  {
    if (!this._acceptPromptText.isActiveAndEnabled || this._acceptPromptText.Term != this._accept)
      return;
    base.HideAcceptButton();
    this.IsAccepting = false;
  }

  public void ShowSkipButton()
  {
    if (this._acceptPromptText.isActiveAndEnabled)
      return;
    this._acceptPromptText.SetTerm(this._skip);
    base.ShowAcceptButton();
    this.ForceRebuild();
    this.IsSkipping = true;
  }

  public void HideSkipButton()
  {
    if (!this._acceptPromptText.isActiveAndEnabled || this._acceptPromptText.Term != this._skip)
      return;
    base.HideAcceptButton();
    this._acceptPromptText.SetTerm(this._accept);
    this.IsSkipping = false;
  }

  public void ShowFastForwardButton()
  {
    if (this._acceptPromptText.isActiveAndEnabled)
      return;
    this._acceptPromptText.SetTerm(this._fastForward);
    base.ShowAcceptButton();
    this.ForceRebuild();
    this.IsFastForwarding = true;
  }

  public void HideFastForwardButton()
  {
    if (!this._acceptPromptText.isActiveAndEnabled || this._acceptPromptText.Term != this._fastForward)
      return;
    base.HideAcceptButton();
    this._acceptPromptText.SetTerm(this._accept);
    this.IsFastForwarding = false;
  }

  public void ShowBackButton()
  {
    if (this._backPromptText.isActiveAndEnabled)
      return;
    base.ShowCancelButton();
    this.ForceRebuild();
    this.IsBacking = true;
  }

  public void HideBackButton()
  {
    if (!this._backPromptText.isActiveAndEnabled || this._backPromptText.Term != this._back)
      return;
    base.HideCancelButton();
    this.IsBacking = false;
  }

  public new void ShowCancelButton()
  {
    if (this._backPromptText.isActiveAndEnabled)
      return;
    this._backPromptText.SetTerm(this._cancel);
    base.ShowCancelButton();
    this.ForceRebuild();
    this.IsCancelling = true;
  }

  public new void HideCancelButton()
  {
    if (!this._backPromptText.isActiveAndEnabled || this._backPromptText.Term != this._cancel)
      return;
    base.HideCancelButton();
    this._backPromptText.SetTerm(this._back);
    this.IsCancelling = false;
  }
}
