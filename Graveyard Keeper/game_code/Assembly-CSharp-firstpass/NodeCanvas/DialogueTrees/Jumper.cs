// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.Jumper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Color("00b9e8")]
[ParadoxNotion.Design.Icon("Set", false, "")]
[Description("Select a target node to jump to.\nFor your convenience in identifying nodes in the dropdown, please give a Tag name to the nodes you want to use in this way.")]
[Name("JUMP", 0)]
[Category("Control")]
public class Jumper : DTNode
{
  [SerializeField]
  public string _sourceNodeUID;
  public object _sourceNode;

  public string sourceNodeUID
  {
    get => this._sourceNodeUID;
    set => this._sourceNodeUID = value;
  }

  public DTNode sourceNode
  {
    get
    {
      if (this._sourceNode == null)
      {
        this._sourceNode = (object) this.graph.allNodes.OfType<DTNode>().FirstOrDefault<DTNode>((Func<DTNode, bool>) (n => n.UID == this.sourceNodeUID));
        if (this._sourceNode == null)
          this._sourceNode = new object();
      }
      return this._sourceNode as DTNode;
    }
    set => this._sourceNode = (object) value;
  }

  public override int maxOutConnections => 0;

  public override bool requireActorSelection => false;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this.sourceNode == null)
      return this.Error("Target Node of Jumper node is null");
    this.DLGTree.EnterNode(this.sourceNode);
    return NodeCanvas.Status.Success;
  }

  [CompilerGenerated]
  public bool \u003Cget_sourceNode\u003Eb__6_0(DTNode n) => n.UID == this.sourceNodeUID;
}
