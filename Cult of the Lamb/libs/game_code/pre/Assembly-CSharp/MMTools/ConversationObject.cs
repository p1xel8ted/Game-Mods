// Decompiled with JetBrains decompiler
// Type: MMTools.ConversationObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
