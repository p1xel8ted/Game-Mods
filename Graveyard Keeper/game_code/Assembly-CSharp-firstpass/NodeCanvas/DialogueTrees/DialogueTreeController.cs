// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.DialogueTreeController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

public class DialogueTreeController : GraphOwner<DialogueTree>, IDialogueActor
{
  string IDialogueActor.name => this.name;

  Texture2D IDialogueActor.portrait => (Texture2D) null;

  Sprite IDialogueActor.portraitSprite => (Sprite) null;

  Color IDialogueActor.dialogueColor => Color.white;

  Vector3 IDialogueActor.dialoguePosition => Vector3.zero;

  Transform IDialogueActor.transform => this.transform;

  public void StartDialogue()
  {
    this.graph = this.GetInstance(this.graph);
    this.graph.StartGraph((Component) this, this.blackboard, true);
  }

  public void StartDialogue(IDialogueActor instigator)
  {
    this.graph = this.GetInstance(this.graph);
    this.graph.StartGraph(instigator is Component ? (Component) instigator : (Component) instigator.transform, this.blackboard, true);
  }

  public void StartDialogue(IDialogueActor instigator, Action<bool> callback)
  {
    this.graph = this.GetInstance(this.graph);
    this.graph.StartGraph(instigator is Component ? (Component) instigator : (Component) instigator.transform, this.blackboard, true, callback);
  }

  public void StartDialogue(Action<bool> callback)
  {
    this.graph = this.GetInstance(this.graph);
    this.graph.StartGraph((Component) this, this.blackboard, true, callback);
  }

  public void SetActorReference(string paramName, IDialogueActor actor)
  {
    if (!((UnityEngine.Object) this.behaviour != (UnityEngine.Object) null))
      return;
    this.behaviour.SetActorReference(paramName, actor);
  }

  public void SetActorReferences(Dictionary<string, IDialogueActor> actors)
  {
    if (!((UnityEngine.Object) this.behaviour != (UnityEngine.Object) null))
      return;
    this.behaviour.SetActorReferences(actors);
  }

  public IDialogueActor GetActorReferenceByName(string paramName)
  {
    return !((UnityEngine.Object) this.behaviour != (UnityEngine.Object) null) ? (IDialogueActor) null : this.behaviour.GetActorReferenceByName(paramName);
  }
}
