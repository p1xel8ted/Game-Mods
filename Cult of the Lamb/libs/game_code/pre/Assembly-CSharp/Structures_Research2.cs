// Decompiled with JetBrains decompiler
// Type: Structures_Research2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Xml.Serialization;
using UnityEngine;

#nullable disable
public class Structures_Research2 : Structures_Research
{
  private const int MAX_SLOT_COUNT = 3;
  [XmlIgnore]
  private bool[] _slotReserved = new bool[3];

  public override bool[] SlotReserved => this._slotReserved;

  public override Vector3 GetResearchPosition(int slotIndex)
  {
    Vector3 position = this.Data.Position;
    switch (slotIndex)
    {
      case 0:
        position += new Vector3(-0.5f, -0.5f);
        break;
      case 1:
        position += new Vector3(2.5f, 2.5f);
        break;
      case 2:
        position += new Vector3(-0.5f, 2.5f);
        break;
    }
    return position;
  }
}
