// Decompiled with JetBrains decompiler
// Type: MMPause_ToggleScreenShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;

#nullable disable
public class MMPause_ToggleScreenShake : BaseMonoBehaviour
{
  public TextMeshProUGUI text;

  private void OnEnable()
  {
    this.text.text = DataManager.Instance.Options_ScreenShake ? "Disable Screen Shake" : "Enable Screen Shake";
  }

  public void ToggleScreenShake()
  {
    DataManager.Instance.Options_ScreenShake = !DataManager.Instance.Options_ScreenShake;
    this.text.text = DataManager.Instance.Options_ScreenShake ? "Disable Screen Shake" : "Enable Screen Shake";
  }
}
