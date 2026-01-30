// Decompiled with JetBrains decompiler
// Type: DisablePerLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class DisablePerLanguage : MonoBehaviour
{
  public MMButton _button;
  public CanvasGroup _canvasGroup;
  public bool disabled;

  public void Start()
  {
    this._button = this.GetComponent<MMButton>();
    this._canvasGroup = this.GetComponent<CanvasGroup>();
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.CheckLanguage);
    this.CheckLanguage();
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.CheckLanguage);
  }

  public void CheckLanguage()
  {
    if (!((Object) this._button != (Object) null) || !((Object) this._canvasGroup != (Object) null))
      return;
    if (LocalizationManager.CurrentLanguage == "English")
    {
      this._button.Interactable = true;
      this._canvasGroup.alpha = 1f;
      this._canvasGroup.interactable = true;
      this.disabled = false;
    }
    else
    {
      this._canvasGroup.interactable = false;
      this._button.Interactable = false;
      this._canvasGroup.alpha = 0.5f;
      this.disabled = true;
    }
  }
}
