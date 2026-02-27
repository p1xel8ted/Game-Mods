// Decompiled with JetBrains decompiler
// Type: ChangeLogoPerLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ChangeLogoPerLanguage : MonoBehaviour
{
  [SerializeField]
  private Image logo;
  [SerializeField]
  private Sprite chineseLogo;
  [SerializeField]
  private Sprite englishLogo;

  public void UpdateLogo()
  {
    if (LocalizationManager.CurrentLanguage == "Chinese (Simplified)")
      this.logo.sprite = this.chineseLogo;
    else
      this.logo.sprite = this.englishLogo;
  }

  private void OnEnable()
  {
    this.UpdateLogo();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLogo);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLogo);
  }
}
