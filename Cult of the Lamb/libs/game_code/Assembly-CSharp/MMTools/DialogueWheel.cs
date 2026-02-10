// Decompiled with JetBrains decompiler
// Type: MMTools.DialogueWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace MMTools;

public class DialogueWheel : MonoBehaviour
{
  [SerializeField]
  public DialogueWheel.DialogueOption[] _options;
  [SerializeField]
  public RectTransform _arrowRectTransform;
  public List<Response> _responses;
  public IMMSelectable _selectable;

  public event DialogueWheel.GiveAnswer OnGiveAnswer;

  public void OnEnable()
  {
    this._responses = MMConversation.CURRENT_CONVERSATION.Responses;
    this.UpdateLocalization();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalization);
    UIMenuBase.OnFirstMenuShow += new System.Action(this.OnFirstMenuShow);
    UIMenuBase.OnFinalMenuHide += new System.Action(this.OnFinalMenuHide);
    foreach (DialogueWheel.DialogueOption option in this._options)
    {
      DialogueWheel.DialogueOption dialogueOption = option;
      dialogueOption.Button.onClick.AddListener((UnityAction) (() => this.OnOptionClicked(this._options.IndexOf<DialogueWheel.DialogueOption>(dialogueOption))));
      dialogueOption.Button.OnSelected += (System.Action) (() => this.OnOptionSelected(dialogueOption.Button));
      dialogueOption.Button.OnDeselected += (System.Action) (() => this.OnOptionDeselected(dialogueOption.Button));
    }
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalization);
    UIMenuBase.OnFirstMenuShow -= new System.Action(this.OnFirstMenuShow);
    UIMenuBase.OnFinalMenuHide -= new System.Action(this.OnFinalMenuHide);
    foreach (DialogueWheel.DialogueOption option in this._options)
    {
      option.Button.onClick.RemoveAllListeners();
      option.Button.OnSelected = (System.Action) null;
      option.Button.OnDeselected = (System.Action) null;
    }
    this._selectable = (IMMSelectable) null;
  }

  public void OnOptionSelected(MMButton button)
  {
    AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one * 1.2f, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    Quaternion endValue = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Clamp(this._arrowRectTransform.position.x - button.transform.position.x, -1f, 1f) * 90f));
    this._arrowRectTransform.DOKill();
    this._arrowRectTransform.DORotateQuaternion(endValue, 0.15f).SetUpdate<TweenerCore<Quaternion, Quaternion, NoOptions>>(true).SetEase<TweenerCore<Quaternion, Quaternion, NoOptions>>(Ease.InOutSine);
    this._selectable = (IMMSelectable) button;
  }

  public void OnOptionDeselected(MMButton button)
  {
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }

  public void OnOptionClicked(int option)
  {
    AudioManager.Instance.PlayOneShot("event:/sermon/select_upgrade");
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    DialogueWheel.GiveAnswer onGiveAnswer = this.OnGiveAnswer;
    if (onGiveAnswer != null)
      onGiveAnswer(option);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
  }

  public void Update()
  {
    if (UIMenuBase.ActiveMenus.Count > 0 || this._selectable != null)
      return;
    float horizontalAxis = InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if ((double) Mathf.Abs(horizontalAxis) <= 0.0)
      return;
    Vector2 lhs = new Vector2(horizontalAxis, 0.0f);
    MMButton newSelectable = (MMButton) null;
    float num1 = float.MinValue;
    foreach (DialogueWheel.DialogueOption option in this._options)
    {
      Vector2 rhs = new Vector2(Mathf.Clamp(this._arrowRectTransform.position.x - option.Button.transform.position.x, -1f, 1f), 0.0f);
      float num2 = Vector2.Dot(lhs, rhs);
      if ((double) num2 > (double) num1)
      {
        num1 = num2;
        newSelectable = option.Button;
      }
    }
    if (!((UnityEngine.Object) newSelectable != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) newSelectable);
  }

  public void UpdateLocalization()
  {
    if (this._responses == null || this._responses.Count <= 0)
      return;
    for (int index = 0; index < this._options.Length; ++index)
      this._options[index].Text.text = LocalizationManager.GetTranslation(this._responses[index].Term);
  }

  public void OnFirstMenuShow() => this.SetActiveStateForMenu(false);

  public void OnFinalMenuHide() => this.SetActiveStateForMenu(true);

  public virtual void SetActiveStateForMenu(bool state)
  {
    foreach (IMMSelectable componentsInChild in this.gameObject.GetComponentsInChildren<IMMSelectable>())
      componentsInChild.SetInteractionState(state);
    if (this._selectable == null || !state)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this._selectable);
  }

  [Serializable]
  public struct DialogueOption
  {
    public MMButton Button;
    public TextMeshProUGUI Text;
  }

  public delegate void GiveAnswer(int Answer);
}
