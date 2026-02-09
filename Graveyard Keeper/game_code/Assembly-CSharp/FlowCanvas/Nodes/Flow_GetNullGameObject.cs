// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetNullGameObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("NULL GameObject", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
public class Flow_GetNullGameObject : PureFunctionNode<GameObject>
{
  public override GameObject Invoke() => (GameObject) null;
}
