// Decompiled with JetBrains decompiler
// Type: HUDLayerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;

#nullable disable
public class HUDLayerDisplay : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;

  public void OnEnable() => this.OnTempleKeys();

  public void OnDisable()
  {
  }

  public void OnTempleKeys()
  {
    this.Text.text = "<sprite name=\"icon_key\"> " + Inventory.TempleKeys.ToString();
  }
}
