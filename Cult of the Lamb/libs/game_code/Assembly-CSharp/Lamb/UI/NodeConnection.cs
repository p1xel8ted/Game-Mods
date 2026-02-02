// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Map;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NodeConnection : BaseMonoBehaviour
{
  [CompilerGenerated]
  public AdventureMapNode \u003CFrom\u003Ek__BackingField;
  [CompilerGenerated]
  public AdventureMapNode \u003CTo\u003Ek__BackingField;
  [CompilerGenerated]
  public MMUILineRenderer \u003CLineRenderer\u003Ek__BackingField;

  public AdventureMapNode From
  {
    set => this.\u003CFrom\u003Ek__BackingField = value;
    get => this.\u003CFrom\u003Ek__BackingField;
  }

  public AdventureMapNode To
  {
    set => this.\u003CTo\u003Ek__BackingField = value;
    get => this.\u003CTo\u003Ek__BackingField;
  }

  public MMUILineRenderer LineRenderer
  {
    set => this.\u003CLineRenderer\u003Ek__BackingField = value;
    get => this.\u003CLineRenderer\u003Ek__BackingField;
  }

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
