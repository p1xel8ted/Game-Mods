// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.DTNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

public abstract class DTNode : Node
{
  [SerializeField]
  public string _actorName = "INSTIGATOR";
  [SerializeField]
  public string _actorParameterID;

  public override string name
  {
    get
    {
      if (!this.requireActorSelection)
        return base.name;
      return this.DLGTree.definedActorParameterNames.Contains(this.actorName) ? $"{this.actorName}" : $"<color=#d63e3e>* {this._actorName} *</color>";
    }
  }

  public virtual bool requireActorSelection => true;

  public override int maxInConnections => -1;

  public override int maxOutConnections => 1;

  public sealed override System.Type outConnectionType => typeof (DTConnection);

  public sealed override bool allowAsPrime => true;

  public sealed override Alignment2x2 commentsAlignment => Alignment2x2.Right;

  public sealed override Alignment2x2 iconAlignment => Alignment2x2.Bottom;

  public DialogueTree DLGTree => (DialogueTree) this.graph;

  public string actorName
  {
    get
    {
      DialogueTree.ActorParameter parameterById = this.DLGTree.GetParameterByID(this._actorParameterID);
      return parameterById == null ? this._actorName : parameterById.name;
    }
    set
    {
      if (!(this._actorName != value) || string.IsNullOrEmpty(value))
        return;
      this._actorName = value;
      this._actorParameterID = this.DLGTree.GetParameterByName(value)?.ID;
    }
  }

  public IDialogueActor finalActor
  {
    get
    {
      return this.DLGTree.GetActorReferenceByID(this._actorParameterID) ?? this.DLGTree.GetActorReferenceByName(this._actorName);
    }
  }
}
