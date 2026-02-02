// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindItemNonBindable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class KeybindItemNonBindable : MonoBehaviour
{
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  public int _button;
  [SerializeField]
  [TermsPopup("")]
  public string _bindingTerm;
  [SerializeField]
  public ControllerType _controllerType;
  [SerializeField]
  public Localize _localize;
  [SerializeField]
  public MMButtonPrompt _controlPrompt;

  public int Button => this._button;

  public string BindingTerm => this._bindingTerm;

  public ControllerType ControllerType => this._controllerType;

  public void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.OnActiveControllerChanged(InputManager.General.GetLastActiveController());
  }

  public void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  public void OnActiveControllerChanged(Controller controller)
  {
    Platform platformFromInputType = ControlUtilities.GetPlatformFromInputType(ControlUtilities.GetCurrentInputType(controller));
    if ((UnityEngine.Object) this._controlPrompt != (UnityEngine.Object) null)
      this._controlPrompt.Platform = platformFromInputType;
    this.ForceUpdate();
  }

  public void ForceUpdate()
  {
    if ((UnityEngine.Object) this._localize != (UnityEngine.Object) null)
      this._localize.Term = this._bindingTerm;
    if (!((UnityEngine.Object) this._controlPrompt != (UnityEngine.Object) null))
      return;
    this._controlPrompt.Button = this._button;
  }

  public void OnValidate() => this.ForceUpdate();
}
