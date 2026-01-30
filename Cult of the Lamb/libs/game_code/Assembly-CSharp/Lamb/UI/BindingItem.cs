// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BindingItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Rewired;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class BindingItem : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  public int _category;
  [SerializeField]
  [HideInInspector]
  public int _action;
  [SerializeField]
  [HideInInspector]
  public Pole _axisContribution;
  [SerializeField]
  public ControllerType _controllerType;
  [SerializeField]
  public MMControlPrompt _controlPrompt;
  [SerializeField]
  public MMButton _bindingButton;
  [SerializeField]
  [HideInInspector]
  public string _bindingTerm;

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

  public void OnValidate()
  {
    if (!((Object) this._controlPrompt != (Object) null))
      return;
    this._controlPrompt.Category = this._category;
    this._controlPrompt.AxisContribution = (int) this._axisContribution;
    this._controlPrompt.Action = this._action;
    if (this._controllerType != ControllerType.Mouse)
      return;
    this._controlPrompt.PrioritizeMouse = true;
  }
}
