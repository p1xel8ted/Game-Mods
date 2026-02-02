// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RadialProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RadialProgress : BaseMonoBehaviour
{
  [SerializeField]
  public Image _squareRadial;
  [SerializeField]
  public Image _roundRadial;
  public float _progress;

  public float Progress
  {
    get => this._progress;
    set
    {
      this._progress = value;
      this._squareRadial.fillAmount = this._progress;
      this._roundRadial.fillAmount = this._progress;
    }
  }

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
    InputType currentInputType = ControlUtilities.GetCurrentInputType(controller);
    this._squareRadial.gameObject.SetActive(currentInputType == InputType.Keyboard);
    this._roundRadial.gameObject.SetActive(currentInputType != InputType.Keyboard);
  }
}
