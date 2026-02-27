// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasRenderer))]
public class FlockadeLineRenderer : MaskableGraphic, ISerializationCallbackReceiver
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  public float _width = 10f;
  [SerializeField]
  [HideInInspector]
  public string _serializedRoot;
  [NonSerialized]
  public FlockadeLineRenderer.Branch _root = new FlockadeLineRenderer.Branch();
  public List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get => (bool) (UnityEngine.Object) this._texture ? this._texture : (Texture) Graphic.s_WhiteTexture;
  }

  public FlockadeLineRenderer.Branch Root => this._root;

  public List<FlockadeLineRenderer.BranchPoint> Points
  {
    get => this._root.Points;
    set
    {
      this._root.Points = value;
      this._root.IsDirty = true;
      this.UpdateValues();
    }
  }

  public float Fill
  {
    get => this._root.Fill;
    set => this._root.Fill = value;
  }

  public Color Color
  {
    get => this._root.Color;
    set => this._root.Color = value;
  }

  public float Width
  {
    get => this._width;
    set
    {
      this._width = value;
      this._root.IsDirty = true;
    }
  }

  public Texture Texture
  {
    get => this._texture;
    set
    {
      this._texture = value;
      this.UpdateRendering();
    }
  }

  public void OnBeforeSerialize() => this._serializedRoot = JsonUtility.ToJson((object) this._root);

  public void OnAfterDeserialize()
  {
    this._root = JsonUtility.FromJson<FlockadeLineRenderer.Branch>(this._serializedRoot);
  }

  public void UpdateRendering()
  {
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  public void UpdateValues()
  {
    double longestDistance = (double) this.GetLongestDistance();
  }

  public override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.UpdateRendering();
  }

  public override void OnPopulateMesh(VertexHelper vh)
  {
    vh.Clear();
    this._verts.Clear();
    this.DrawLine(this._root, (FlockadeLineRenderer.BranchPoint) null, vh);
  }

  public static float GetLength(List<FlockadeLineRenderer.BranchPoint> branch)
  {
    float length = 0.0f;
    for (int index = 0; index < branch.Count - 1; ++index)
      length += Vector2.Distance(branch[index].Point, branch[index + 1].Point);
    return length;
  }

  public void Update()
  {
    if (!Application.isPlaying || !this._root.IsDirty)
      return;
    this.UpdateRendering();
    this._root.IsDirty = false;
  }

  public float GetLongestDistance()
  {
    int num1 = 0;
    float longestDistance = 0.0f;
    int index1 = 0;
    Dictionary<List<FlockadeLineRenderer.BranchPoint>, float> dictionary = new Dictionary<List<FlockadeLineRenderer.BranchPoint>, float>()
    {
      {
        this._root.Points,
        0.0f
      }
    };
    this._root.TotalLength = FlockadeLineRenderer.GetLength(this._root.Points);
    while (index1 < dictionary.Keys.Count)
    {
      List<FlockadeLineRenderer.BranchPoint> key = dictionary.Keys.ToList<List<FlockadeLineRenderer.BranchPoint>>()[index1];
      float num2 = dictionary[key];
      for (int index2 = 0; index2 < key.Count; ++index2)
      {
        if (key[index2].Branches != null && key[index2].Branches.Count > 0)
        {
          foreach (FlockadeLineRenderer.Branch branch in key[index2].Branches)
          {
            List<FlockadeLineRenderer.BranchPoint> branchPointList = new List<FlockadeLineRenderer.BranchPoint>()
            {
              new FlockadeLineRenderer.BranchPoint(key[index2])
            };
            branchPointList.AddRange((IEnumerable<FlockadeLineRenderer.BranchPoint>) branch.Points);
            dictionary.Add(branchPointList, num2);
            branch.TotalLength = FlockadeLineRenderer.GetLength(branchPointList);
          }
        }
        if (index2 < key.Count - 1)
          num2 += Vector2.Distance(key[index2].Point, key[index2 + 1].Point);
      }
      if ((double) num2 > (double) longestDistance)
        longestDistance = num2;
      ++index1;
      ++num1;
      if (num1 > 100000)
      {
        Debug.Log((object) "Number of iterations exceeded maximum. Check algorithm!".Colour(Color.red));
        break;
      }
    }
    return longestDistance;
  }

  public void DrawLine(
    FlockadeLineRenderer.Branch branch,
    FlockadeLineRenderer.BranchPoint startingPoint,
    VertexHelper vertexHelper)
  {
    UIVertex v = new UIVertex()
    {
      color = (Color32) this.color
    };
    Vector2 zero = (Vector2) Vector3.zero;
    float num1 = 0.0f;
    float num2 = branch.TotalLength * branch.Fill;
    List<FlockadeLineRenderer.BranchPoint> branchPointList = new List<FlockadeLineRenderer.BranchPoint>((IEnumerable<FlockadeLineRenderer.BranchPoint>) branch.Points);
    if (startingPoint != null)
      branchPointList.Insert(0, startingPoint);
    if (branch.FillStyle == FlockadeLineRenderer.FillStyle.Reverse)
      branchPointList.Reverse();
    for (int index = 0; index < branchPointList.Count; ++index)
    {
      if (branchPointList[index].Branches != null && branchPointList[index].Branches.Count > 0)
      {
        foreach (FlockadeLineRenderer.Branch branch1 in branchPointList[index].Branches)
          this.DrawLine(branch1, new FlockadeLineRenderer.BranchPoint(branchPointList[index]), vertexHelper);
      }
      if ((double) num1 > (double) num2 || index == branchPointList.Count - 1)
        break;
      int currentVertCount = vertexHelper.currentVertCount;
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 1);
      vertexHelper.AddTriangle(currentVertCount + 1, currentVertCount + 2, currentVertCount + 3);
      float num3 = Mathf.Atan2(branchPointList[index + 1].y - branchPointList[index].y, branchPointList[index + 1].x - branchPointList[index].x);
      Vector2 vector2_1 = new Vector2(Mathf.Cos(num3 + 1.57079637f), Mathf.Sin(num3 + 1.57079637f));
      Vector2 vector2_2 = new Vector2(Mathf.Cos(num3 - 1.57079637f), Mathf.Sin(num3 - 1.57079637f));
      float num4 = Vector2.Distance(branchPointList[index].Point, branchPointList[index + 1].Point);
      num1 += num4;
      float t = (double) num1 <= (double) num2 ? 1f : 1f - (num1 - num2) / num4;
      zero.x = 0.0f;
      v.position = (Vector3) (branchPointList[index].Point + vector2_1 * this._width);
      v.uv0 = (Vector4) zero;
      v.color = (Color32) branch.Color;
      vertexHelper.AddVert(v);
      zero.x = 1f;
      v.position = (Vector3) (branchPointList[index].Point + vector2_2 * this._width);
      v.uv0 = (Vector4) zero;
      v.color = (Color32) branch.Color;
      vertexHelper.AddVert(v);
      zero.x = 0.0f;
      if ((bool) (UnityEngine.Object) this._texture)
        zero.y += num4 * t / (float) this._texture.height;
      else
        ++zero.y;
      v.position = (Vector3) (Vector2.Lerp(branchPointList[index].Point, branchPointList[index + 1].Point, t) + vector2_1 * this._width);
      v.uv0 = (Vector4) zero;
      v.color = (Color32) branch.Color;
      vertexHelper.AddVert(v);
      zero.x = 1f;
      v.position = (Vector3) (Vector2.Lerp(branchPointList[index].Point, branchPointList[index + 1].Point, t) + vector2_2 * this._width);
      v.uv0 = (Vector4) zero;
      v.color = (Color32) branch.Color;
      vertexHelper.AddVert(v);
    }
  }

  [Serializable]
  public enum FillStyle
  {
    Standard,
    Reverse,
  }

  [Serializable]
  public class BranchPoint
  {
    public Vector2 Point;
    public List<FlockadeLineRenderer.Branch> Branches = new List<FlockadeLineRenderer.Branch>();

    public float x => this.Point.x;

    public float y => this.Point.y;

    public BranchPoint(FlockadeLineRenderer.BranchPoint point) => this.Point = point.Point;

    public BranchPoint(Vector2 point) => this.Point = point;

    public BranchPoint(float x, float y) => this.Point = new Vector2(x, y);

    public void AddBranch()
    {
      this.Branches.Add(new FlockadeLineRenderer.Branch()
      {
        Points = new List<FlockadeLineRenderer.BranchPoint>()
        {
          new FlockadeLineRenderer.BranchPoint(this.Point)
        }
      });
    }

    public FlockadeLineRenderer.Branch AddNewBranch()
    {
      FlockadeLineRenderer.Branch branch = new FlockadeLineRenderer.Branch();
      this.Branches.Add(branch);
      return branch;
    }
  }

  [Serializable]
  public class Branch
  {
    public int Hash;
    public List<FlockadeLineRenderer.BranchPoint> Points = new List<FlockadeLineRenderer.BranchPoint>();
    public FlockadeLineRenderer.FillStyle FillStyle;
    [SerializeField]
    public Color _color = Color.white;
    [SerializeField]
    [Range(0.0f, 1f)]
    public float _fill = 1f;
    public float TotalLength;
    public bool _isDirty;

    public bool IsDirty
    {
      get
      {
        return this.Points != null && this.Points.Where<FlockadeLineRenderer.BranchPoint>((Func<FlockadeLineRenderer.BranchPoint, bool>) (point => point.Branches != null)).SelectMany<FlockadeLineRenderer.BranchPoint, FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.BranchPoint, IEnumerable<FlockadeLineRenderer.Branch>>) (point => (IEnumerable<FlockadeLineRenderer.Branch>) point.Branches)).Any<FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.Branch, bool>) (branch => branch.IsDirty)) || this._isDirty;
      }
      set
      {
        this._isDirty = value;
        if (this.Points == null)
          return;
        foreach (FlockadeLineRenderer.Branch branch in this.Points.Where<FlockadeLineRenderer.BranchPoint>((Func<FlockadeLineRenderer.BranchPoint, bool>) (point => point.Branches != null)).SelectMany<FlockadeLineRenderer.BranchPoint, FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.BranchPoint, IEnumerable<FlockadeLineRenderer.Branch>>) (point => (IEnumerable<FlockadeLineRenderer.Branch>) point.Branches)))
          branch.IsDirty = value;
      }
    }

    public Color Color
    {
      get => this._color;
      set
      {
        this._color = value;
        this._isDirty = true;
      }
    }

    public float Fill
    {
      get => this._fill;
      set
      {
        this._fill = value;
        this._isDirty = true;
      }
    }

    public Branch()
    {
      if (this.Hash != 0)
        return;
      this.Hash = new System.Random(Guid.NewGuid().GetHashCode()).Next(int.MinValue, int.MaxValue).ToString().GetStableHashCode();
    }

    public FlockadeLineRenderer.Branch FindBranchByHash(int hash)
    {
      return this.Hash == hash ? this : this.Points.Where<FlockadeLineRenderer.BranchPoint>((Func<FlockadeLineRenderer.BranchPoint, bool>) (point =>
      {
        List<FlockadeLineRenderer.Branch> branches = point.Branches;
        return branches != null && branches.Count > 0;
      })).SelectMany<FlockadeLineRenderer.BranchPoint, FlockadeLineRenderer.Branch, FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.BranchPoint, IEnumerable<FlockadeLineRenderer.Branch>>) (point => (IEnumerable<FlockadeLineRenderer.Branch>) point.Branches), (Func<FlockadeLineRenderer.BranchPoint, FlockadeLineRenderer.Branch, FlockadeLineRenderer.Branch>) ((_, branch) => branch.FindBranchByHash(hash))).FirstOrDefault<FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.Branch, bool>) (target => target != null));
    }

    public void SetAllFillModes(FlockadeLineRenderer.FillStyle fillStyle)
    {
      this.FillStyle = fillStyle;
      foreach (FlockadeLineRenderer.Branch branch in this.Points.Where<FlockadeLineRenderer.BranchPoint>((Func<FlockadeLineRenderer.BranchPoint, bool>) (point =>
      {
        List<FlockadeLineRenderer.Branch> branches = point.Branches;
        return branches != null && branches.Count > 0;
      })).SelectMany<FlockadeLineRenderer.BranchPoint, FlockadeLineRenderer.Branch>((Func<FlockadeLineRenderer.BranchPoint, IEnumerable<FlockadeLineRenderer.Branch>>) (point => (IEnumerable<FlockadeLineRenderer.Branch>) point.Branches)))
        branch.SetAllFillModes(fillStyle);
    }
  }
}
