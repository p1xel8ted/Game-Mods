// Decompiled with JetBrains decompiler
// Type: Structures_Research1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Xml.Serialization;
using UnityEngine;

#nullable disable
public class Structures_Research1 : Structures_Research
{
  public const int MAX_SLOT_COUNT = 3;
  [XmlIgnore]
  public bool[] _slotReserved = new bool[3];

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
