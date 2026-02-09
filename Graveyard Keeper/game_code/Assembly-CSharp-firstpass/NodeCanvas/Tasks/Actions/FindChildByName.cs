// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindChildByName
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Find a transform child by name within the agent's transform")]
public class FindChildByName : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<string> childName;
  [BlackboardOnly]
  public BBParameter<Transform> saveAs;

  public override string info => $"{this.saveAs} = {this.agentInfo}.FindChild({this.childName})";

  public override void OnExecute()
  {
    Transform transform = this.agent.Find(this.childName.value);
    this.saveAs.value = transform;
    this.EndAction((Object) transform != (Object) null);
  }
}
