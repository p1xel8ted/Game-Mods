// Decompiled with JetBrains decompiler
// Type: HUDLayerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;

#nullable disable
public class HUDLayerDisplay : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;

  private void OnEnable() => this.OnTempleKeys();

  private void OnDisable()
  {
  }

  private void OnTempleKeys()
  {
    this.Text.text = "<sprite name=\"icon_key\"> " + (object) Inventory.TempleKeys;
  }
}
