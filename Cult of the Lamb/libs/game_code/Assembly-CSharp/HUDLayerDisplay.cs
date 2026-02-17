// Decompiled with JetBrains decompiler
// Type: HUDLayerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
