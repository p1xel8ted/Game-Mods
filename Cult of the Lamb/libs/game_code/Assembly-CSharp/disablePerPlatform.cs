// Decompiled with JetBrains decompiler
// Type: disablePerPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class disablePerPlatform : BaseMonoBehaviour
{
  public bool disableOnDesktop;
  public bool disableOnConsole;
  public bool disableOnSwitch;
  public bool disableOn8thGenConsoles;
  public bool disableOnLowQuality;

  public void Awake()
  {
    if (this.disableOnLowQuality && SettingsManager.Settings.Graphics.EnvironmentDetail == 0)
      this.gameObject.SetActive(false);
    else if (this.disableOnDesktop)
    {
      this.gameObject.SetActive(true);
    }
    else
    {
      int num1 = this.disableOn8thGenConsoles ? 1 : 0;
      int num2 = this.disableOnSwitch ? 1 : 0;
      int num3 = this.disableOnConsole ? 1 : 0;
    }
  }
}
