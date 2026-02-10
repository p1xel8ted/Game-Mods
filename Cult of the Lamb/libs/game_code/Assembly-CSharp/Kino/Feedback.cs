// Decompiled with JetBrains decompiler
// Type: Kino.Feedback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Kino;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class Feedback : MonoBehaviour
{
  [SerializeField]
  public Color _color = Color.white;
  [SerializeField]
  [Range(-1f, 1f)]
  public float _offsetX;
  [SerializeField]
  [Range(-1f, 1f)]
  public float _offsetY;
  [SerializeField]
  [Range(-5f, 5f)]
  public float _rotation;
  [SerializeField]
  [Range(0.95f, 1.05f)]
  public float _scale = 1f;
  [SerializeField]
  public bool _jaggies;
  [SerializeField]
  public Shader _shader;
  [SerializeField]
  public Mesh _mesh;
  public Material _material;
  public RenderTexture _delayBuffer;
  public CommandBuffer _command;

  public Color color
  {
    get => this._color;
    set => this._color = value;
  }

  public float offsetX
  {
    get => this._offsetX;
    set => this._offsetX = value;
  }

  public float offsetY
  {
    get => this._offsetY;
    set => this._offsetY = value;
  }

  public float rotation
  {
    get => this._rotation;
    set => this._rotation = value;
  }

  public float scale
  {
    get => this._scale;
    set => this._scale = value;
  }

  public bool jaggies
  {
    get => this._jaggies;
    set => this._jaggies = value;
  }

  public Vector4 rotationMatrixAsVector
  {
    get
    {
      double f = -1.0 * Math.PI / 180.0 * (double) this._rotation;
      float y = Mathf.Sin((float) f);
      float num = Mathf.Cos((float) f);
      return new Vector4(num, y, -y, num);
    }
  }

  public void StartFeedback()
  {
    Camera component = this.GetComponent<Camera>();
    this._delayBuffer = RenderTexture.GetTemporary(component.pixelWidth, component.pixelHeight);
    this._delayBuffer.wrapMode = TextureWrapMode.Clamp;
    this._material.SetTexture("_MainTex", (Texture) this._delayBuffer);
    if (this._command == null)
    {
      this._command = new CommandBuffer();
      this._command.name = "Kino.Feedback";
    }
    this._command.Clear();
    this._command.DrawMesh(this._mesh, Matrix4x4.identity, this._material, 0, 0);
    component.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this._command);
  }

  public void OnEnable()
  {
    if (!((UnityEngine.Object) this._material == (UnityEngine.Object) null))
      return;
    this._material = new Material(Shader.Find("Hidden/Kino/Feedback"));
    this._material.hideFlags = HideFlags.HideAndDontSave;
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this._delayBuffer == (UnityEngine.Object) null)
      return;
    RenderTexture.ReleaseTemporary(this._delayBuffer);
    this._delayBuffer = (RenderTexture) null;
    this.GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this._command);
  }

  public void OnDestroy()
  {
    if (Application.isPlaying)
      UnityEngine.Object.Destroy((UnityEngine.Object) this._material);
    else
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this._material);
  }

  public void Update()
  {
    if ((UnityEngine.Object) this._delayBuffer == (UnityEngine.Object) null)
      return;
    Camera component = this.GetComponent<Camera>();
    if (component.pixelWidth != this._delayBuffer.width || component.pixelHeight != this._delayBuffer.height)
    {
      this.OnDisable();
    }
    else
    {
      this._material.SetColor("_Color", this._color);
      this._material.SetVector("_Offset", (Vector4) (new Vector2(this._offsetX, this._offsetY) * -0.05f));
      this._material.SetVector("_Rotation", this.rotationMatrixAsVector);
      this._material.SetFloat("_Scale", 2f - this._scale);
      this._delayBuffer.filterMode = this._jaggies ? FilterMode.Point : FilterMode.Bilinear;
    }
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((UnityEngine.Object) this._delayBuffer == (UnityEngine.Object) null)
      this.StartFeedback();
    Graphics.Blit((Texture) source, this._delayBuffer);
    Graphics.Blit((Texture) source, destination);
  }
}
