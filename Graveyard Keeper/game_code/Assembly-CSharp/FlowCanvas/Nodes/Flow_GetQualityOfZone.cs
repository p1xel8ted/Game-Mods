// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetQualityOfZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("CubeArrow", false, "")]
[Name("Get Quality Of Zone", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (float)})]
[Category("Game Functions")]
public class Flow_GetQualityOfZone : PureFunctionNode<float, string>
{
  public override float Invoke(string zone_id)
  {
    WorldZone zoneById = WorldZone.GetZoneByID(zone_id);
    return (UnityEngine.Object) zoneById == (UnityEngine.Object) null ? 0.0f : zoneById.GetTotalQuality();
  }
}
