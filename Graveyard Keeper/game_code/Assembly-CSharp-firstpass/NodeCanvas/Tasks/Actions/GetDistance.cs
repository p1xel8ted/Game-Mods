// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetDistance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class GetDistance : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  [BlackboardOnly]
  public BBParameter<float> saveAs;

  public override string info => $"Get Distance to {this.target.ToString()}";

  public override void OnExecute()
  {
    this.saveAs.value = Vector3.Distance(this.agent.position, this.target.value.transform.position);
    this.EndAction();
  }
}
