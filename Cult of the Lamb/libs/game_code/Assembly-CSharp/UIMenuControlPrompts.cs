// Decompiled with JetBrains decompiler
// Type: UIMenuControlPrompts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Rewired;
using src.UINavigator;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIMenuControlPrompts : BaseMonoBehaviour
{
  [Header("Menu")]
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public UIMenuBase _attachedMenu;
  [Header("Prompt Containers")]
  [SerializeField]
  public GameObject _acceptPromptContainer;
  [SerializeField]
  public GameObject _cancelPromptContainer;
  [Header("CoOp")]
  [SerializeField]
  public CoopIndicatorIcon _coopIndicatorIcon;
  [HideInInspector]
  public PlayerFarming playerFarming;

  public void Start() => this.ForceRebuild();

  public void OnCancelButtonClicked() => this._attachedMenu.OnCancelButtonInput();

  public void ShowAcceptButton()
  {
    if (!((UnityEngine.Object) this._acceptPromptContainer != (UnityEngine.Object) null))
      return;
    this._acceptPromptContainer.gameObject.SetActive(true);
  }

  public void UpdateAcceptTerm(string s)
  {
    this._acceptPromptContainer.GetComponentInChildren<MMControlPrompt>().UpdateTerm(s);
    if (!((UnityEngine.Object) this._acceptPromptContainer != (UnityEngine.Object) null))
      return;
    this._acceptPromptContainer.gameObject.SetActive(true);
  }

  public void HideAcceptButton()
  {
    if (!((UnityEngine.Object) this._acceptPromptContainer != (UnityEngine.Object) null))
      return;
    this._acceptPromptContainer.gameObject.SetActive(false);
  }

  public void ShowCancelButton()
  {
    if (!((UnityEngine.Object) this._cancelPromptContainer != (UnityEngine.Object) null))
      return;
    this._cancelPromptContainer.gameObject.SetActive(true);
  }

  public void HideCancelButton()
  {
    if (!((UnityEngine.Object) this._cancelPromptContainer != (UnityEngine.Object) null))
      return;
    this._cancelPromptContainer.gameObject.SetActive(false);
  }

  public virtual void OnEnable()
  {
    this._coopIndicatorIcon.gameObject.SetActive(false);
    this.OnActiveControllerChanged((Controller) null);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.ForceRebuild);
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
  }

  public virtual void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.ForceRebuild);
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  public void OnActiveControllerChanged(Controller obj)
  {
    if (PlayerFarming.players.Count <= 1 || !((UnityEngine.Object) this._coopIndicatorIcon != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      this.playerFarming = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer : PlayerFarming.Instance;
    if (!(bool) (UnityEngine.Object) this.playerFarming)
      return;
    this._coopIndicatorIcon.gameObject.SetActive(true);
    if (this.playerFarming.isLamb)
      this._coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
    else
      this._coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
  }

  public void ForceRebuild() => LayoutRebuilder.ForceRebuildLayoutImmediate(this._rectTransform);
}
