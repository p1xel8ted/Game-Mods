// Decompiled with JetBrains decompiler
// Type: MMTools.BuyObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MMTools;

public class BuyObject
{
  public List<BuyEntry> Entries;
  public List<Response> Responses;
  public Action CallBack;

  public BuyObject(List<BuyEntry> Entries, List<Response> Responses, Action CallBack)
  {
    this.Entries = Entries;
    this.Responses = Responses;
    this.CallBack = CallBack;
  }

  public void Clear()
  {
    if (this.Entries != null)
      this.Entries.Clear();
    this.Entries = (List<BuyEntry>) null;
    if (this.Responses != null)
      this.Responses.Clear();
    this.Responses = (List<Response>) null;
  }
}
