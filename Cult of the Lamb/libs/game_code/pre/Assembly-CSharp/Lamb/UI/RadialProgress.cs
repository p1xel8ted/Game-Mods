// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RadialProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RadialProgress : BaseMonoBehaviour
{
  [SerializeField]
  private Image _squareRadial;
  [SerializeField]
  private Image _roundRadial;
  private float _progress;

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

  private void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.OnActiveControllerChanged(InputManager.General.GetLastActiveController());
  }

  private void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  private void OnActiveControllerChanged(Controller controller)
  {
    InputType currentInputType = ControlUtilities.GetCurrentInputType(controller);
    this._squareRadial.gameObject.SetActive(currentInputType == InputType.Keyboard);
    this._roundRadial.gameObject.SetActive(currentInputType != InputType.Keyboard);
  }
}
