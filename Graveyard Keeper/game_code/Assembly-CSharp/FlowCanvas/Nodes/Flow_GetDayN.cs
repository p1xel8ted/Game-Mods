// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetDayN
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Day num", 0)]
[Category("Game Functions")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (int)})]
public class Flow_GetDayN : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueOutput<int>("day #", (ValueHandler<int>) (() =>
    {
      if ((UnityEngine.Object) MainGame.me == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "MainGame.me is null!");
        return -1;
      }
      if (MainGame.me.save != null)
        return MainGame.me.save.day;
      Debug.LogError((object) "MainGame.me.save is null!");
      return -1;
    }));
  }
}
