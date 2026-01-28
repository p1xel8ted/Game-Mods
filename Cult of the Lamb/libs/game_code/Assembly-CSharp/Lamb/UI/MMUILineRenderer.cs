// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUILineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class MMUILineRenderer : MaskableGraphic, ISerializationCallbackReceiver
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  public float _width = 10f;
  [SerializeField]
  [HideInInspector]
  public string _serializedRoot;
  [NonSerialized]
  public MMUILineRenderer.Branch _root = new MMUILineRenderer.Branch();
  public List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((UnityEngine.Object) this._texture == (UnityEngine.Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public MMUILineRenderer.Branch Root
  {
    get
    {
      if (this._root == null)
      {
        this.OnAfterDeserialize();
        Debug.LogError((object) ("Root wasn't deserialzied during object creation! Object: " + this.transform.parent.gameObject.name));
      }
      return this._root;
    }
  }

  public List<MMUILineRenderer.BranchPoint> Points
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

  public void OnBeforeSerialize()
  {
    JsonSerializerSettings settings = new JsonSerializerSettings();
    settings.Converters.Add((JsonConverter) new Vector2Converter());
    settings.Converters.Add((JsonConverter) new ColorConverter());
    this._serializedRoot = JsonConvert.SerializeObject((object) this._root, settings);
  }

  public void OnAfterDeserialize()
  {
    this._root = JsonConvert.DeserializeObject<MMUILineRenderer.Branch>(this._serializedRoot);
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
    this.DrawLine(this._root, (MMUILineRenderer.BranchPoint) null, vh);
  }

  public float GetLength(List<MMUILineRenderer.BranchPoint> branch)
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
    Dictionary<List<MMUILineRenderer.BranchPoint>, float> dictionary = new Dictionary<List<MMUILineRenderer.BranchPoint>, float>()
    {
      {
        this._root.Points,
        0.0f
      }
    };
    this._root.TotalLength = this.GetLength(this._root.Points);
    while (index1 < dictionary.Keys.Count)
    {
      List<MMUILineRenderer.BranchPoint> key = dictionary.Keys.ToList<List<MMUILineRenderer.BranchPoint>>()[index1];
      float num2 = dictionary[key];
      for (int index2 = 0; index2 < key.Count; ++index2)
      {
        if (key[index2].Branches != null && key[index2].Branches.Count > 0)
        {
          foreach (MMUILineRenderer.Branch branch in key[index2].Branches)
          {
            List<MMUILineRenderer.BranchPoint> branchPointList = new List<MMUILineRenderer.BranchPoint>();
            branchPointList.Add(new MMUILineRenderer.BranchPoint(key[index2]));
            branchPointList.AddRange((IEnumerable<MMUILineRenderer.BranchPoint>) branch.Points);
            dictionary.Add(branchPointList, num2);
            branch.TotalLength = this.GetLength(branchPointList);
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
    MMUILineRenderer.Branch branch,
    MMUILineRenderer.BranchPoint startingPoint,
    VertexHelper vertexHelper)
  {
    UIVertex v = new UIVertex()
    {
      color = (Color32) this.color
    };
    Vector2 zero = (Vector2) Vector3.zero;
    float num1 = 1.57079637f;
    float num2 = 0.0f;
    float num3 = branch.TotalLength * branch.Fill;
    List<MMUILineRenderer.BranchPoint> branchPointList = new List<MMUILineRenderer.BranchPoint>((IEnumerable<MMUILineRenderer.BranchPoint>) branch.Points);
    if (startingPoint != null)
      branchPointList.Insert(0, startingPoint);
    if (branch.FillStyle == MMUILineRenderer.FillStyle.Reverse)
      branchPointList.Reverse();
    for (int index = 0; index < branchPointList.Count; ++index)
    {
      if (branchPointList[index].Branches != null && branchPointList[index].Branches.Count > 0)
      {
        foreach (MMUILineRenderer.Branch branch1 in branchPointList[index].Branches)
          this.DrawLine(branch1, new MMUILineRenderer.BranchPoint(branchPointList[index]), vertexHelper);
      }
      if ((double) num2 > (double) num3 || index == branchPointList.Count - 1)
        break;
      int currentVertCount = vertexHelper.currentVertCount;
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 1);
      vertexHelper.AddTriangle(currentVertCount + 1, currentVertCount + 2, currentVertCount + 3);
      float num4 = Mathf.Atan2(branchPointList[index + 1].y - branchPointList[index].y, branchPointList[index + 1].x - branchPointList[index].x);
      Vector2 vector2_1 = new Vector2(Mathf.Cos(num4 + num1), Mathf.Sin(num4 + num1));
      Vector2 vector2_2 = new Vector2(Mathf.Cos(num4 - num1), Mathf.Sin(num4 - num1));
      float num5 = Vector2.Distance(branchPointList[index].Point, branchPointList[index + 1].Point);
      num2 += num5;
      float t = (double) num2 <= (double) num3 ? 1f : 1f - (num2 - num3) / num5;
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
      if ((UnityEngine.Object) this._texture != (UnityEngine.Object) null)
        zero.y += num5 * t / (float) this._texture.height;
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
    [SerializeField]
    public List<MMUILineRenderer.Branch> branches = new List<MMUILineRenderer.Branch>();

    [JsonIgnore]
    public float x => this.Point.x;

    [JsonIgnore]
    public float y => this.Point.y;

    public List<MMUILineRenderer.Branch> Branches
    {
      get
      {
        if (this.branches == null)
          this.branches = new List<MMUILineRenderer.Branch>();
        return this.branches;
      }
    }

    public BranchPoint() => this.Point = Vector2.zero;

    public BranchPoint(MMUILineRenderer.BranchPoint point) => this.Point = point.Point;

    public BranchPoint(Vector2 point) => this.Point = point;

    public BranchPoint(float x, float y) => this.Point = new Vector2(x, y);

    public void AddBranch()
    {
      this.Branches.Add(new MMUILineRenderer.Branch()
      {
        Points = new List<MMUILineRenderer.BranchPoint>()
        {
          new MMUILineRenderer.BranchPoint(this.Point)
        }
      });
    }

    public MMUILineRenderer.Branch AddNewBranch()
    {
      MMUILineRenderer.Branch branch = new MMUILineRenderer.Branch();
      this.Branches.Add(branch);
      return branch;
    }
  }

  [Serializable]
  public class Branch
  {
    public int Hash;
    public List<MMUILineRenderer.BranchPoint> Points = new List<MMUILineRenderer.BranchPoint>();
    public MMUILineRenderer.FillStyle FillStyle;
    [SerializeField]
    public Color _color = Color.white;
    [SerializeField]
    [Range(0.0f, 1f)]
    public float _fill = 1f;
    public float TotalLength;
    public bool _isDirty;

    public bool IsDirty
    {
      set
      {
        this._isDirty = value;
        if (this.Points == null)
          return;
        foreach (MMUILineRenderer.BranchPoint point in this.Points)
        {
          if (point.Branches != null)
          {
            foreach (MMUILineRenderer.Branch branch in point.Branches)
              branch.IsDirty = value;
          }
        }
      }
      get
      {
        if (this.Points != null)
        {
          foreach (MMUILineRenderer.BranchPoint point in this.Points)
          {
            if (point.Branches != null)
            {
              foreach (MMUILineRenderer.Branch branch in point.Branches)
              {
                if (branch.IsDirty)
                  return true;
              }
            }
          }
        }
        return this._isDirty;
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

    public MMUILineRenderer.Branch FindBranchByHash(int hash)
    {
      if (this.Hash == hash)
        return this;
      foreach (MMUILineRenderer.BranchPoint point in this.Points)
      {
        if (point.Branches != null && point.Branches.Count > 0)
        {
          foreach (MMUILineRenderer.Branch branch in point.Branches)
          {
            MMUILineRenderer.Branch branchByHash = branch.FindBranchByHash(hash);
            if (branchByHash != null)
              return branchByHash;
          }
        }
      }
      return (MMUILineRenderer.Branch) null;
    }

    public void SetAllFillModes(MMUILineRenderer.FillStyle fillStyle)
    {
      this.FillStyle = fillStyle;
      foreach (MMUILineRenderer.BranchPoint point in this.Points)
      {
        if (point.Branches != null && point.Branches.Count > 0)
        {
          foreach (MMUILineRenderer.Branch branch in point.Branches)
            branch.SetAllFillModes(fillStyle);
        }
      }
    }
  }
}
