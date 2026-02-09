// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.SubtitlesRequestInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NodeCanvas.DialogueTrees;

public class SubtitlesRequestInfo
{
  public IDialogueActor actor;
  public IStatement statement;
  public Action Continue;

  public SubtitlesRequestInfo(IDialogueActor actor, IStatement statement, Action callback)
  {
    this.actor = actor;
    this.statement = statement;
    this.Continue = callback;
  }
}
