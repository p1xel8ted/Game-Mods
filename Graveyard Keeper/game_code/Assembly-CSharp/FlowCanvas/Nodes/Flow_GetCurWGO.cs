// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCurWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Icon("Cube", false, "")]
[Name("Get current WGO", 0)]
[Category("Game Functions")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
[Color("00ff00")]
public class Flow_GetCurWGO : MyPureFunctionNode<WorldGameObject>
{
  public override WorldGameObject Invoke() => this.wgo;
}
