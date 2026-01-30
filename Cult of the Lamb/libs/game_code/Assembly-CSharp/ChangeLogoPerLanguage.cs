// Decompiled with JetBrains decompiler
// Type: ChangeLogoPerLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ChangeLogoPerLanguage : MonoBehaviour
{
  [SerializeField]
  public Image logo;
  [SerializeField]
  public Sprite chineseLogo;
  [SerializeField]
  public Sprite englishLogo;

  public void UpdateLogo()
  {
    if (LocalizationManager.CurrentLanguage == "Chinese (Simplified)")
      this.logo.sprite = this.chineseLogo;
    else
      this.logo.sprite = this.englishLogo;
  }

  public void OnEnable()
  {
    this.UpdateLogo();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLogo);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLogo);
  }
}
