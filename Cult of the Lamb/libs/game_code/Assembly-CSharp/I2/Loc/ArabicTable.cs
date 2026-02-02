// Decompiled with JetBrains decompiler
// Type: I2.Loc.ArabicTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace I2.Loc;

public class ArabicTable
{
  public static List<ArabicMapping> mapList;
  public static ArabicTable arabicMapper;

  public ArabicTable()
  {
    ArabicTable.mapList = new List<ArabicMapping>();
    ArabicTable.mapList.Add(new ArabicMapping(1569, 65152));
    ArabicTable.mapList.Add(new ArabicMapping(1575, 65165));
    ArabicTable.mapList.Add(new ArabicMapping(1571, 65155));
    ArabicTable.mapList.Add(new ArabicMapping(1572, 65157));
    ArabicTable.mapList.Add(new ArabicMapping(1573, 65159));
    ArabicTable.mapList.Add(new ArabicMapping(1609, 64508));
    ArabicTable.mapList.Add(new ArabicMapping(1574, 65161));
    ArabicTable.mapList.Add(new ArabicMapping(1576, 65167));
    ArabicTable.mapList.Add(new ArabicMapping(1578, 65173));
    ArabicTable.mapList.Add(new ArabicMapping(1579, 65177));
    ArabicTable.mapList.Add(new ArabicMapping(1580, 65181));
    ArabicTable.mapList.Add(new ArabicMapping(1581, 65185));
    ArabicTable.mapList.Add(new ArabicMapping(1582, 65189));
    ArabicTable.mapList.Add(new ArabicMapping(1583, 65193));
    ArabicTable.mapList.Add(new ArabicMapping(1584, 65195));
    ArabicTable.mapList.Add(new ArabicMapping(1585, 65197));
    ArabicTable.mapList.Add(new ArabicMapping(1586, 65199));
    ArabicTable.mapList.Add(new ArabicMapping(1587, 65201));
    ArabicTable.mapList.Add(new ArabicMapping(1588, 65205));
    ArabicTable.mapList.Add(new ArabicMapping(1589, 65209));
    ArabicTable.mapList.Add(new ArabicMapping(1590, 65213));
    ArabicTable.mapList.Add(new ArabicMapping(1591, 65217));
    ArabicTable.mapList.Add(new ArabicMapping(1592, 65221));
    ArabicTable.mapList.Add(new ArabicMapping(1593, 65225));
    ArabicTable.mapList.Add(new ArabicMapping(1594, 65229));
    ArabicTable.mapList.Add(new ArabicMapping(1601, 65233));
    ArabicTable.mapList.Add(new ArabicMapping(1602, 65237));
    ArabicTable.mapList.Add(new ArabicMapping(1603, 65241));
    ArabicTable.mapList.Add(new ArabicMapping(1604, 65245));
    ArabicTable.mapList.Add(new ArabicMapping(1605, 65249));
    ArabicTable.mapList.Add(new ArabicMapping(1606, 65253));
    ArabicTable.mapList.Add(new ArabicMapping(1607, 65257));
    ArabicTable.mapList.Add(new ArabicMapping(1608, 65261));
    ArabicTable.mapList.Add(new ArabicMapping(1610, 65265));
    ArabicTable.mapList.Add(new ArabicMapping(1570, 65153));
    ArabicTable.mapList.Add(new ArabicMapping(1577, 65171));
    ArabicTable.mapList.Add(new ArabicMapping(1662, 64342));
    ArabicTable.mapList.Add(new ArabicMapping(1670, 64378));
    ArabicTable.mapList.Add(new ArabicMapping(1688, 64394));
    ArabicTable.mapList.Add(new ArabicMapping(1711, 64402));
    ArabicTable.mapList.Add(new ArabicMapping(1705, 64398));
  }

  public static ArabicTable ArabicMapper
  {
    get
    {
      if (ArabicTable.arabicMapper == null)
        ArabicTable.arabicMapper = new ArabicTable();
      return ArabicTable.arabicMapper;
    }
  }

  public int Convert(int toBeConverted)
  {
    foreach (ArabicMapping map in ArabicTable.mapList)
    {
      if (map.from == toBeConverted)
        return map.to;
    }
    return toBeConverted;
  }
}
