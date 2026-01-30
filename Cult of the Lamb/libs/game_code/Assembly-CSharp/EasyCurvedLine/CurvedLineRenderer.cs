// Decompiled with JetBrains decompiler
// Type: EasyCurvedLine.CurvedLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public LineRenderer line;
  public LineSmoother_ lineSmoother = new LineSmoother_();
  public CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
  public Transform[] linePointTransforms = new Transform[0];
  public Vector3[] linePositions = new Vector3[0];
  public Vector3[] linePositionsOld = new Vector3[0];

  public void GetLines()
  {
    this.linePoints = this.GetComponentsInChildren<CurvedLinePoint>();
    this.linePointTransforms = new Transform[this.linePoints.Length];
    for (int index = 0; index < this.linePointTransforms.Length; ++index)
      this.linePointTransforms[index] = this.linePoints[index].transform;
    this.line = this.GetComponent<LineRenderer>();
  }

  public void OnEnable()
  {
    this.linePoints = this.GetComponentsInChildren<CurvedLinePoint>();
    this.linePointTransforms = new Transform[this.linePoints.Length];
    for (int index = 0; index < this.linePointTransforms.Length; ++index)
      this.linePointTransforms[index] = this.linePoints[index].transform;
    this.line = this.GetComponent<LineRenderer>();
  }

  public void Update()
  {
    this.GetPoints();
    this.SetPointsToLine();
  }

  public void GetPoints()
  {
    if (this.linePositions.Length != this.linePoints.Length)
      this.linePositions = new Vector3[this.linePoints.Length];
    for (int index = 0; index < this.linePoints.Length; ++index)
      this.linePositions[index] = this.linePointTransforms[index].position + this.Offset3;
  }

  public void SetPointsToLine()
  {
    if (this.linePositionsOld.Length != this.linePositions.Length)
      this.linePositionsOld = new Vector3[this.linePositions.Length];
    bool flag = false;
    for (int index = 0; index < this.linePositions.Length; ++index)
    {
      if (this.linePositions[index] != this.linePositionsOld[index])
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    Vector3[] positions = this.lineSmoother.SmoothLine(this.linePositions, this.lineSegmentSize);
    this.line.positionCount = positions.Length;
    this.line.SetPositions(positions);
    this.line.startWidth = this.lineWidth;
    this.line.endWidth = this.useCustomEndWidth ? this.endWidth : this.lineWidth;
  }

  public void OnDrawGizmosSelected() => this.Update();

  public void OnDrawGizmos()
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
