// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_NearestDockPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[Name("Find Nearest Dock Point From WGO", 0)]
[Color("eed9a7")]
public class Flow_NearestDockPoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> target_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<Transform>("Dock Point", (ValueHandler<Transform>) (() =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(target_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.Log((object) " WGO is null!");
        return (Transform) null;
      }
      List<DockPoint> dockPointList = new List<DockPoint>();
      List<DockPoint> list = ((IEnumerable<DockPoint>) worldGameObject.GetComponentsInChildren<DockPoint>()).ToList<DockPoint>();
      if (list.Count == 0)
      {
        Debug.Log((object) "No Dock Points in WGO!");
        return (Transform) null;
      }
      DockPoint dockPoint1 = (DockPoint) null;
      float num1 = float.PositiveInfinity;
      foreach (DockPoint dockPoint2 in list)
      {
        float num2 = Vector2.Distance((Vector2) dockPoint2.transform.position, (Vector2) MainGame.me.player.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          dockPoint1 = dockPoint2;
        }
      }
      return dockPoint1.transform;
    }));
  }
}
