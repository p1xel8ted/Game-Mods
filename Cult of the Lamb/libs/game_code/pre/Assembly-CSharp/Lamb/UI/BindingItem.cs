// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BindingItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using Rewired;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class BindingItem : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  private int _category;
  [SerializeField]
  [HideInInspector]
  private int _action;
  [SerializeField]
  [HideInInspector]
  private Pole _axisContribution;
  [SerializeField]
  private ControllerType _controllerType;
  [SerializeField]
  private MMControlPrompt _controlPrompt;
  [SerializeField]
  private Animator _controlPromptAnimator;
  [SerializeField]
  private MMButton _bindingButton;
  [SerializeField]
  [HideInInspector]
  private string _bindingTerm;
  [SerializeField]
  private KeybindItem _keybindItem;

  public MMButton BindingButton => this._bindingButton;

  public string BindingTerm
  {
    get => this._bindingTerm;
    set => this._bindingTerm = value;
  }

  public int Category
  {
    get => this._category;
    set
    {
      this._category = value;
      this._controlPrompt.Category = this._category;
    }
  }

  public int Action
  {
    get => this._action;
    set
    {
      this._action = value;
      this._controlPrompt.Action = this._action;
    }
  }

  public Pole AxisContribution
  {
    get => this._axisContribution;
    set
    {
      this._axisContribution = value;
      this._controlPrompt.AxisContribution = (int) this._axisContribution;
    }
  }

  public ControllerType ControllerType
  {
    get => this._controllerType;
    set => this._controllerType = value;
  }

  private void OnValidate()
  {
    if (!((UnityEngine.Object) this._controlPrompt != (UnityEngine.Object) null))
      return;
    this._controlPrompt.Category = this._category;
    this._controlPrompt.AxisContribution = (int) this._axisContribution;
    this._controlPrompt.Action = this._action;
    if (this._controllerType != ControllerType.Mouse)
      return;
    this._controlPrompt.PrioritizeMouse = true;
  }

  private void Awake()
  {
    if ((UnityEngine.Object) this._bindingButton == (UnityEngine.Object) null)
      return;
    this._bindingButton.OnSelected += (System.Action) (() =>
    {
      this.ShowSelected();
      this._keybindItem.ShowSelected();
    });
    this._bindingButton.OnDeselected += (System.Action) (() =>
    {
      this.ShowNormal();
      this._keybindItem.ShowNormal();
    });
  }

  public void ShowSelected()
  {
    this._controlPromptAnimator.ResetAllTriggers();
    this._controlPromptAnimator.SetTrigger("Selected");
  }

  public void ShowNormal()
  {
    this._controlPromptAnimator.ResetAllTriggers();
    this._controlPromptAnimator.SetTrigger("Normal");
  }
}
