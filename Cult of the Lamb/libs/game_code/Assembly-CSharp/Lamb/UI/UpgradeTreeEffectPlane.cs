// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeEffectPlane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeTreeEffectPlane : MaskableGraphic
{
  [Header("Grid")]
  [SerializeField]
  public float _gridSize = 1f;
  [SerializeField]
  public int _horizontalPoints = 2;
  [SerializeField]
  public int _verticalPoints = 2;
  [Header("Effects")]
  [SerializeField]
  public float _falloff;
  [SerializeField]
  public float _verticalFalloff;
  [SerializeField]
  public AnimationCurve _verticalFalloffCurve;
  [SerializeField]
  public float _intensity = 1f;
  [SerializeField]
  public List<UpgradeTreeNode> _nodes = new List<UpgradeTreeNode>();
  [SerializeField]
  [HideInInspector]
  public List<UpgradeTreeEffectPlane.BakedVertex> _bakedVertices = new List<UpgradeTreeEffectPlane.BakedVertex>();
  public List<UIVertex> _verts = new List<UIVertex>();

  public override void OnPopulateMesh(VertexHelper vh)
  {
    vh.Clear();
    this._verts.Clear();
    if (this._bakedVertices.Count < 3)
      return;
    UIVertex uiVertex = new UIVertex();
    Color black = Color.black;
    for (int index = 0; index < this._bakedVertices.Count; ++index)
    {
      uiVertex.position = (Vector3) this._bakedVertices[index].Position;
      uiVertex.uv0 = (Vector4) this._bakedVertices[index].UV;
      Color color = this._bakedVertices[index].Color;
      foreach (UpgradeTreeEffectPlane.BakedNodeData bakedNode in this._bakedVertices[index].BakedNodes)
      {
        color.r += bakedNode.AlphaContribution * bakedNode.GetUnavailability();
        color.g += bakedNode.AlphaContribution * bakedNode.GetAvailability();
        color.b += (float) ((double) bakedNode.AlphaContribution * (double) bakedNode.GetUnlocked() * (1.0 - (double) bakedNode.GetHidden()));
      }
      color.r = Mathf.Clamp(color.r, 0.0f, 1f);
      color.g = Mathf.Clamp(color.g, 0.0f, 1f);
      color.b = Mathf.Clamp(color.b, 0.0f, 1f);
      color.a = Mathf.Clamp(color.a, 0.0f, 1f);
      uiVertex.color = (Color32) color;
      this._verts.Add(uiVertex);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = 0; idx0 < vh.currentVertCount - this._horizontalPoints - 1; ++idx0)
    {
      if (idx0 <= 0 || (idx0 + 1) % this._horizontalPoints != 0)
      {
        vh.AddTriangle(idx0, idx0 + this._horizontalPoints, idx0 + this._horizontalPoints + 1);
        vh.AddTriangle(idx0, idx0 + this._horizontalPoints + 1, idx0 + 1);
      }
    }
  }

  [Serializable]
  public class BakedNodeData
  {
    public UpgradeTreeNode Node;
    public float AlphaContribution;

    public float GetUnavailability() => this.Node.UnavailableWeight;

    public float GetAvailability() => this.Node.AvailableWeight;

    public float GetUnlocked() => this.Node.UnlockedWeight;

    public float GetHidden() => this.Node.HiddenWeight;
  }

  [Serializable]
  public class BakedVertex
  {
    public Vector2 Position;
    public Color Color;
    public Vector2 UV;
    public List<UpgradeTreeEffectPlane.BakedNodeData> BakedNodes;
  }
}
