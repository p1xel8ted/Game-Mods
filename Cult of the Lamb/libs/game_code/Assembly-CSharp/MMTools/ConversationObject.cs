// Decompiled with JetBrains decompiler
// Type: MMTools.ConversationObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MMTools;

public class ConversationObject
{
  public List<ConversationEntry> Entries;
  public List<Response> Responses;
  public List<DoctrineResponse> DoctrineResponses;
  public Action CallBack;

  public ConversationObject(
    List<ConversationEntry> Entries,
    List<Response> Responses,
    Action CallBack,
    List<DoctrineResponse> DoctrineResponses = null)
  {
    this.Entries = Entries;
    this.Responses = Responses;
    this.DoctrineResponses = DoctrineResponses;
    this.CallBack = CallBack;
  }

  public void Clear()
  {
    if (this.Entries != null)
      this.Entries.Clear();
    this.Entries = (List<ConversationEntry>) null;
    if (this.Responses != null)
      this.Responses.Clear();
    this.Responses = (List<Response>) null;
    if (this.DoctrineResponses != null)
      this.DoctrineResponses.Clear();
    this.DoctrineResponses = (List<DoctrineResponse>) null;
  }
}
