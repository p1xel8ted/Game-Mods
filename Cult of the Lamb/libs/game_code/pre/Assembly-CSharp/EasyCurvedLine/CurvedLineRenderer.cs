// Decompiled with JetBrains decompiler
// Type: EasyCurvedLine.CurvedLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EasyCurvedLine;

[RequireComponent(typeof (LineRenderer))]
public class CurvedLineRenderer : BaseMonoBehaviour
{
  public float lineSegmentSize = 0.15f;
  public float lineWidth = 0.1f;
  [Tooltip("Enable this to set a custom width for the line end")]
  public bool useCustomEndWidth;
  [Tooltip("Custom width for the line end")]
  public float endWidth = 0.1f;
  [Header("Gizmos")]
  public bool showGizmos = true;
  public float gizmoSize = 0.1f;
  public Color gizmoColor = new Color(1f, 0.0f, 0.0f, 0.5f);
  public Vector3 Offset3 = new Vector3(0.0f, 0.0f, 0.0f);
  private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
  private Vector3[] linePositions = new Vector3[0];
  private Vector3[] linePositionsOld = new Vector3[0];

  public void Update()
  {
    this.GetPoints();
    this.SetPointsToLine();
  }

  private void GetPoints()
  {
    this.linePoints = this.GetComponentsInChildren<CurvedLinePoint>();
    this.linePositions = new Vector3[this.linePoints.Length];
    for (int index = 0; index < this.linePoints.Length; ++index)
      this.linePositions[index] = this.linePoints[index].transform.position + this.Offset3;
  }

  private void SetPointsToLine()
  {
    if (this.linePositionsOld.Length != this.linePositions.Length)
      this.linePositionsOld = new Vector3[this.linePositions.Length];
    bool flag = false;
    for (int index = 0; index < this.linePositions.Length; ++index)
    {
      if (this.linePositions[index] != this.linePositionsOld[index])
        flag = true;
    }
    if (!flag)
      return;
    LineRenderer component = this.GetComponent<LineRenderer>();
    Vector3[] positions = LineSmoother_.SmoothLine(this.linePositions, this.lineSegmentSize);
    component.positionCount = positions.Length;
    component.SetPositions(positions);
    component.startWidth = this.lineWidth;
    component.endWidth = this.useCustomEndWidth ? this.endWidth : this.lineWidth;
  }

  private void OnDrawGizmosSelected() => this.Update();

  private void OnDrawGizmos()
  {
    if (this.linePoints.Length == 0)
      this.GetPoints();
    foreach (CurvedLinePoint linePoint in this.linePoints)
    {
      linePoint.showGizmo = this.showGizmos;
      linePoint.gizmoSize = this.gizmoSize;
      linePoint.gizmoColor = this.gizmoColor;
    }
  }
}
