// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleScreenShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
