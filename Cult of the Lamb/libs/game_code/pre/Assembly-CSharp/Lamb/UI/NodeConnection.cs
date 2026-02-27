// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Map;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NodeConnection : BaseMonoBehaviour
{
  public AdventureMapNode From { private set; get; }

  public AdventureMapNode To { private set; get; }

  public MMUILineRenderer LineRenderer { private set; get; }

  public void Configure(
    AdventureMapNode from,
    AdventureMapNode to,
    MMUILineRenderer lineRenderer,
    Material solidLine,
    Material dottedLine)
  {
    this.From = from;
    this.To = to;
    this.LineRenderer = lineRenderer;
    if (to.State == NodeStates.Visited && from.State == NodeStates.Visited)
    {
      lineRenderer.Color = UIAdventureMapOverlayController.VisitedColour;
      lineRenderer.material = solidLine;
    }
    else if (to.State == NodeStates.Attainable && from.State == NodeStates.Visited)
    {
      lineRenderer.Color = UIAdventureMapOverlayController.AvailableColour;
      lineRenderer.material = dottedLine;
    }
    else
    {
      lineRenderer.Color = UIAdventureMapOverlayController.LockedColour;
      lineRenderer.material = dottedLine;
    }
  }
}
