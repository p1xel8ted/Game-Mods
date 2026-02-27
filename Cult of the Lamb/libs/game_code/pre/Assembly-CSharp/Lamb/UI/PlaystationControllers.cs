// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PlaystationControllers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class PlaystationControllers : InputDisplay
{
  [SerializeField]
  private RectTransform _dualShock4Container;
  [SerializeField]
  private RectTransform _dualSenseContainer;
  private GameObject _dualShock4;
  private GameObject _dualSense;

  public override void Configure(InputType inputType)
  {
    if ((Object) this._dualShock4 == (Object) null && (Object) MonoSingleton<UIManager>.Instance.PS4ControllerTemplate != (Object) null)
    {
      this._dualShock4 = MonoSingleton<UIManager>.Instance.PS4ControllerTemplate.Instantiate<InputController>((Transform) this._dualShock4Container).gameObject;
      this._dualShock4.transform.localPosition = Vector3.zero;
      this._dualShock4.transform.localScale = Vector3.one;
    }
    if ((Object) this._dualSense == (Object) null && (Object) MonoSingleton<UIManager>.Instance.PS5ControllerTemplate != (Object) null)
    {
      this._dualSense = MonoSingleton<UIManager>.Instance.PS5ControllerTemplate.Instantiate<InputController>((Transform) this._dualSenseContainer).gameObject;
      this._dualSense.transform.localPosition = Vector3.zero;
      this._dualSense.transform.localScale = Vector3.one;
    }
    this._dualShock4.SetActive(inputType == InputType.DualShock4);
    this._dualSense.SetActive(inputType == InputType.DualSense);
  }
}
