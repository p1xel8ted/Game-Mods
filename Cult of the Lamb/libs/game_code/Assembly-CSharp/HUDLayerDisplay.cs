// Decompiled with JetBrains decompiler
// Type: HUDLayerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
