// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleScreenShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;

#nullable disable
public class MMPause_ToggleScreenShake : BaseMonoBehaviour
{
  public TextMeshProUGUI text;

  public void OnEnable()
  {
    this.text.text = DataManager.Instance.Options_ScreenShake ? "Disable Screen Shake" : "Enable Screen Shake";
  }

  public void ToggleScreenShake()
  {
    DataManager.Instance.Options_ScreenShake = !DataManager.Instance.Options_ScreenShake;
    this.text.text = DataManager.Instance.Options_ScreenShake ? "Disable Screen Shake" : "Enable Screen Shake";
  }
}
