// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindItemNonBindable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class KeybindItemNonBindable : MonoBehaviour
{
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  private int _button;
  [SerializeField]
  [TermsPopup("")]
  private string _bindingTerm;
  [SerializeField]
  private ControllerType _controllerType;
  [SerializeField]
  private Localize _localize;
  [SerializeField]
  private MMButton _mmButton;
  [SerializeField]
  private TextMeshProUGUI _text;
  [SerializeField]
  private Animator _textAnimator;
  [SerializeField]
  private MMButtonPrompt _controlPrompt;
  [SerializeField]
  private Animator _controlPromptAnimator;

  public int Button => this._button;

  public string BindingTerm => this._bindingTerm;

  public ControllerType ControllerType => this._controllerType;

  private void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.OnActiveControllerChanged(InputManager.General.GetLastActiveController());
  }

  private void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  private void Awake()
  {
    this._mmButton.OnSelected += (System.Action) (() =>
    {
      this._textAnimator.ResetAllTriggers();
      this._textAnimator.SetTrigger("Selected");
      this._controlPromptAnimator.ResetAllTriggers();
      this._controlPromptAnimator.SetTrigger("Selected");
    });
    this._mmButton.OnDeselected += (System.Action) (() =>
    {
      this._textAnimator.ResetAllTriggers();
      this._textAnimator.SetTrigger("Normal");
      this._controlPromptAnimator.ResetAllTriggers();
      this._controlPromptAnimator.SetTrigger("Normal");
    });
  }

  private void OnActiveControllerChanged(Controller controller)
  {
    Platform platformFromInputType = ControlUtilities.GetPlatformFromInputType(ControlUtilities.GetCurrentInputType(controller));
    if ((UnityEngine.Object) this._controlPrompt != (UnityEngine.Object) null)
      this._controlPrompt.Platform = platformFromInputType;
    this.ForceUpdate();
  }

  private void ForceUpdate()
  {
    if ((UnityEngine.Object) this._localize != (UnityEngine.Object) null)
      this._localize.Term = this._bindingTerm;
    if (!((UnityEngine.Object) this._controlPrompt != (UnityEngine.Object) null))
      return;
    this._controlPrompt.Button = this._button;
  }

  private void OnValidate() => this.ForceUpdate();
}
