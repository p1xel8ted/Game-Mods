// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NintendoControllers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NintendoControllers : InputDisplay
{
  [SerializeField]
  private RectTransform _joyConsDetachedContainer;
  [SerializeField]
  private RectTransform _joyConsDockedContainer;
  [SerializeField]
  private RectTransform _handheldContainer;
  [SerializeField]
  private RectTransform _proControllerContainer;
  private GameObject _joyConsDetached;
  private GameObject _joyConsDocked;
  private GameObject _handheld;
  private GameObject _proController;

  public override void Configure(InputType inputType)
  {
    if ((Object) this._joyConsDocked == (Object) null)
    {
      this._joyConsDocked = MonoSingleton<UIManager>.Instance.SwitchJoyConsDockedTemplate.Instantiate<InputController>((Transform) this._joyConsDockedContainer).gameObject;
      this._joyConsDocked.transform.localPosition = Vector3.zero;
      this._joyConsDocked.transform.localScale = Vector3.one;
    }
    if ((Object) this._joyConsDetached == (Object) null)
    {
      this._joyConsDetached = MonoSingleton<UIManager>.Instance.SwitchJoyConsTemplate.Instantiate<InputController>((Transform) this._joyConsDetachedContainer).gameObject;
      this._joyConsDetached.transform.localPosition = Vector3.zero;
      this._joyConsDetached.transform.localScale = Vector3.one;
    }
    if ((Object) this._handheld == (Object) null)
    {
      this._handheld = MonoSingleton<UIManager>.Instance.SwitchHandheldTemplate.Instantiate<InputController>((Transform) this._handheldContainer).gameObject;
      this._handheld.transform.localPosition = Vector3.zero;
      this._handheld.transform.localScale = Vector3.one;
    }
    if ((Object) this._proController == (Object) null)
    {
      this._proController = MonoSingleton<UIManager>.Instance.SwitchProControllerTemplate.Instantiate<InputController>((Transform) this._proControllerContainer).gameObject;
      this._proController.transform.localPosition = Vector3.zero;
      this._proController.transform.localScale = Vector3.one;
    }
    this._joyConsDetached.SetActive(inputType == InputType.SwitchJoyConsDetached);
    this._joyConsDocked.SetActive(inputType == InputType.SwitchJoyConsDocked);
    this._handheld.SetActive(inputType == InputType.SwitchHandheld);
    this._proController.SetActive(inputType == InputType.SwitchProController);
  }
}
