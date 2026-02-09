// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetWGOParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (float)})]
[ParadoxNotion.Design.Icon("CubeArrow", false, "")]
[Category("Game Functions")]
[Name("Get WGO Param", 0)]
public class Flow_GetWGOParam : PureFunctionNode<float, string, WorldGameObject>
{
  public override float Invoke(string param, WorldGameObject wgo)
  {
    if (!((UnityEngine.Object) wgo == (UnityEngine.Object) null))
      return wgo.GetParam(param);
    Debug.LogError((object) "WGO is null");
    return 0.0f;
  }
}
