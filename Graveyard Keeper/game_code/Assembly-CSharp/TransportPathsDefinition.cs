// Decompiled with JetBrains decompiler
// Type: TransportPathsDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TransportPathsDefinition : BalanceBaseObject
{
  public const string DEFAULT_DESTINATION_ZONE_ID = "mf_wood";
  public string source_zone_id;
  public string station_wgo_id;
  public string destination_zone_id;
  public List<string> transport_items = new List<string>();
}
