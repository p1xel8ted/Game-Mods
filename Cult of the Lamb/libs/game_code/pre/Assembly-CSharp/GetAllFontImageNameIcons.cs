// Decompiled with JetBrains decompiler
// Type: GetAllFontImageNameIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class GetAllFontImageNameIcons : MonoBehaviour
{
  public TextMeshProUGUI text;
  private string s = "";

  public void GetText()
  {
    for (int Type = 0; Type < 117; ++Type)
      this.s += FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) Type);
    this.text.text = this.s;
  }
}
