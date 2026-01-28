// Decompiled with JetBrains decompiler
// Type: GetAllFontImageNameIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class GetAllFontImageNameIcons : MonoBehaviour
{
  public TextMeshProUGUI text;
  public string s = "";

  public void GetText()
  {
    for (int Type = 0; Type < 117; ++Type)
      this.s += FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) Type);
    this.text.text = this.s;
  }
}
