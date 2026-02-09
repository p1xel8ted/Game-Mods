// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.MultipleChoiceRequestInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.DialogueTrees;

public class MultipleChoiceRequestInfo
{
  public IDialogueActor actor;
  public Dictionary<IStatement, int> options;
  public float availableTime;
  public bool showLastStatement;
  public Action<int> SelectOption;

  public MultipleChoiceRequestInfo(
    IDialogueActor actor,
    Dictionary<IStatement, int> options,
    float availableTime,
    bool showLastStatement,
    Action<int> callback)
  {
    this.actor = actor;
    this.options = options;
    this.availableTime = availableTime;
    this.showLastStatement = showLastStatement;
    this.SelectOption = callback;
  }

  public MultipleChoiceRequestInfo(
    IDialogueActor actor,
    Dictionary<IStatement, int> options,
    float availableTime,
    Action<int> callback)
  {
    this.actor = actor;
    this.options = options;
    this.availableTime = availableTime;
    this.SelectOption = callback;
  }
}
