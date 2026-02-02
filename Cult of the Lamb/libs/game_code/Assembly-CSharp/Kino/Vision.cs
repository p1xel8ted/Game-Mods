// Decompiled with JetBrains decompiler
// Type: Kino.Vision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Kino Image Effects/Vision")]
public class Vision : MonoBehaviour
{
  [SerializeField]
  public Vision.Source _source;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _blendRatio = 0.5f;
  [SerializeField]
  public bool _preferDepthNormals;
  [SerializeField]
  public float _depthRepeat = 1f;
  [SerializeField]
  public bool _validateNormals;
  [SerializeField]
  public float _motionOverlayAmplitude = 10f;
  [SerializeField]
  public float _motionVectorsAmplitude = 50f;
  [SerializeField]
  [Range(8f, 64f)]
  public int _motionVectorsResolution = 16 /*0x10*/;
  [SerializeField]
  public Shader _shader;
  public Material _material;
  public Vision.ArrowArray _arrows;

  public int source
  {
    get => (int) this._source;
    set => this._source = (Vision.Source) (value % 3);
  }

  public Camera TargetCamera => this.GetComponent<Camera>();

  public bool IsGBufferAvailable
  {
    get => this.TargetCamera.actualRenderingPath == RenderingPath.DeferredShading;
  }

  public void PrepareArrows()
  {
    int vectorsResolution = this._motionVectorsResolution;
    int columns = vectorsResolution * Screen.width / Screen.height;
    if (this._arrows.columnCount == columns && this._arrows.rowCount == vectorsResolution)
      return;
    this._arrows.DestroyMesh();
    this._arrows.BuildMesh(columns, vectorsResolution);
  }

  public void DrawArrows(float aspect)
  {
    this.PrepareArrows();
    float y = 1f / (float) this._motionVectorsResolution;
    this._material.SetVector("_Scale", (Vector4) new Vector2(y / aspect, y));
    this._material.SetFloat("_Blend", this._blendRatio);
    this._material.SetFloat("_Amplitude", this._motionVectorsAmplitude);
    this._material.SetPass(5);
    Graphics.DrawMeshNow(this._arrows.mesh, Matrix4x4.identity);
  }

  public void OnEnable()
  {
    this._material = new Material(Shader.Find("Hidden/Kino/Vision"));
    this._material.hideFlags = HideFlags.DontSave;
    this._arrows = new Vision.ArrowArray();
    this.PrepareArrows();
  }

  public void OnDisable()
  {
    Object.DestroyImmediate((Object) this._material);
    this._material = (Material) null;
    this._arrows.DestroyMesh();
    this._arrows = (Vision.ArrowArray) null;
  }

  public void Update()
  {
    if (this._source == Vision.Source.Depth)
    {
      if (this._preferDepthNormals)
        this.TargetCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
      else
        this.TargetCamera.depthTextureMode |= DepthTextureMode.Depth;
    }
    if (this._source == Vision.Source.Normals && (this._preferDepthNormals || !this.IsGBufferAvailable))
      this.TargetCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
    if (this._source != Vision.Source.MotionVectors)
      return;
    this.TargetCamera.depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (this._source == Vision.Source.Depth)
    {
      this._material.SetFloat("_Blend", this._blendRatio);
      this._material.SetFloat("_Repeat", this._depthRepeat);
      int pass = this._preferDepthNormals ? 1 : 0;
      Graphics.Blit((Texture) source, destination, this._material, pass);
    }
    else if (this._source == Vision.Source.Normals)
    {
      this._material.SetFloat("_Blend", this._blendRatio);
      this._material.SetFloat("_Validate", this._validateNormals ? 1f : 0.0f);
      int pass = this._preferDepthNormals || !this.IsGBufferAvailable ? 2 : 3;
      Graphics.Blit((Texture) source, destination, this._material, pass);
    }
    else
    {
      if (this._source != Vision.Source.MotionVectors)
        return;
      this._material.SetFloat("_Blend", this._blendRatio);
      this._material.SetFloat("_Amplitude", this._motionOverlayAmplitude);
      Graphics.Blit((Texture) source, destination, this._material, 4);
      this.DrawArrows((float) source.width / (float) source.height);
    }
  }

  public class ArrowArray
  {
    public Mesh _mesh;
    [CompilerGenerated]
    public int \u003CcolumnCount\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CrowCount\u003Ek__BackingField;

    public Mesh mesh => this._mesh;

    public int columnCount
    {
      get => this.\u003CcolumnCount\u003Ek__BackingField;
      set => this.\u003CcolumnCount\u003Ek__BackingField = value;
    }

    public int rowCount
    {
      get => this.\u003CrowCount\u003Ek__BackingField;
      set => this.\u003CrowCount\u003Ek__BackingField = value;
    }

    public void BuildMesh(int columns, int rows)
    {
      Vector3[] vector3Array = new Vector3[6]
      {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1f, 0.0f),
        new Vector3(0.0f, 1f, 0.0f),
        new Vector3(-1f, 1f, 0.0f),
        new Vector3(0.0f, 1f, 0.0f),
        new Vector3(1f, 1f, 0.0f)
      };
      int capacity = 6 * columns * rows;
      List<Vector3> inVertices = new List<Vector3>(capacity);
      List<Vector2> uvs = new List<Vector2>(capacity);
      for (int index1 = 0; index1 < rows; ++index1)
      {
        for (int index2 = 0; index2 < columns; ++index2)
        {
          Vector2 vector2 = new Vector2((0.5f + (float) index2) / (float) columns, (0.5f + (float) index1) / (float) rows);
          for (int index3 = 0; index3 < 6; ++index3)
          {
            inVertices.Add(vector3Array[index3]);
            uvs.Add(vector2);
          }
        }
      }
      int[] indices = new int[capacity];
      for (int index = 0; index < capacity; ++index)
        indices[index] = index;
      this._mesh = new Mesh();
      this._mesh.hideFlags = HideFlags.DontSave;
      this._mesh.SetVertices((List<Vector3>) inVertices);
      this._mesh.SetUVs(0, (List<Vector2>) uvs);
      this._mesh.SetIndices(indices, MeshTopology.Lines, 0);
      this._mesh.UploadMeshData(true);
      this.columnCount = columns;
      this.rowCount = rows;
    }

    public void DestroyMesh()
    {
      if ((Object) this._mesh != (Object) null)
        Object.DestroyImmediate((Object) this._mesh);
      this._mesh = (Mesh) null;
    }
  }

  public enum Source
  {
    Depth,
    Normals,
    MotionVectors,
    NumValues,
  }
}
