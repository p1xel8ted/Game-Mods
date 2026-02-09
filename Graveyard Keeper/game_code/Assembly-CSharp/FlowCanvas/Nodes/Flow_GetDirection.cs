// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetDirection
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Direction", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[ParadoxNotion.Design.Icon("Human", false, "")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
public class Flow_GetDirection : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("Char WGO");
    ValueInput<bool> in_invert = this.AddValueInput<bool>("invert");
    this.AddValueOutput<Direction>("Direction", (ValueHandler<Direction>) (() =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null || !worldGameObject.components.character.enabled)
        return Direction.None;
      Direction dir = worldGameObject.components.character.direction.ToDirection();
      if (dir == Direction.None)
      {
        Debug.LogError((object) "Wrong char direction!");
        return dir;
      }
      if (in_invert.value)
        dir = dir.Opposite();
      return dir;
    }));
  }
}
