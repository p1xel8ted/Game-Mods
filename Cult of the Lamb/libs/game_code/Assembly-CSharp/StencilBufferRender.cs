// Decompiled with JetBrains decompiler
// Type: StencilBufferRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[RequireComponent(typeof (Camera))]
public class StencilBufferRender : BaseMonoBehaviour
{
  public CommandBuffer _cmdBuffer;
  public static RenderTexture _stencilRenderTexture;
  public Material _postProcessMat;
  public Resolution _currentScreenRes;
  public const string cmdBufferName = "StencilBufferRT";
  public CameraEvent camInsertPoint = CameraEvent.AfterForwardOpaque;
  public static Mesh s_Quad;

  public Camera _cam => this.gameObject.GetComponent<Camera>();

  public static Mesh quad
  {
    get
    {
      if ((Object) StencilBufferRender.s_Quad != (Object) null)
        return StencilBufferRender.s_Quad;
      Vector3[] vector3Array = new Vector3[4]
      {
        new Vector3(-0.5f, -0.5f, 0.0f),
        new Vector3(0.5f, 0.5f, 0.0f),
        new Vector3(0.5f, -0.5f, 0.0f),
        new Vector3(-0.5f, 0.5f, 0.0f)
      };
      Vector2[] vector2Array = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0.0f),
        new Vector2(0.0f, 1f)
      };
      int[] numArray = new int[6]{ 0, 1, 2, 1, 0, 3 };
      StencilBufferRender.s_Quad = new Mesh()
      {
        vertices = vector3Array,
        uv = vector2Array,
        triangles = numArray
      };
      StencilBufferRender.s_Quad.RecalculateNormals();
      StencilBufferRender.s_Quad.RecalculateBounds();
      return StencilBufferRender.s_Quad;
    }
  }

  public void OnPreRender()
  {
    if (this._currentScreenRes.height != this._cam.pixelHeight || this._currentScreenRes.width != this._cam.pixelWidth)
    {
      this.SetCurrentDim();
      this.CreateCommandBuffer();
    }
    this.SetCurrentDim();
  }

  public void SetCurrentDim()
  {
    this._currentScreenRes.height = this._cam.pixelHeight;
    this._currentScreenRes.width = this._cam.pixelWidth;
  }

  public void CreateCommandBuffer()
  {
    this.DestroyCommandBuffer();
    this._cmdBuffer = new CommandBuffer();
    this._cmdBuffer.name = "StencilBufferRT";
    RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
    renderTexture.antiAliasing = 1;
    renderTexture.autoGenerateMips = false;
    renderTexture.filterMode = FilterMode.Point;
    renderTexture.name = "StencilRT" + Time.frameCount.ToString();
    StencilBufferRender._stencilRenderTexture = renderTexture;
    this._cmdBuffer.SetRenderTarget((RenderTargetIdentifier) (Texture) StencilBufferRender._stencilRenderTexture, (RenderTargetIdentifier) BuiltinRenderTextureType.CameraTarget);
    this._cmdBuffer.ClearRenderTarget(false, true, Color.black);
    this._postProcessMat = new Material(Shader.Find("Hidden/StencilToRT"));
    Matrix4x4 cameraToWorldMatrix = this._cam.cameraToWorldMatrix;
    this._cmdBuffer.DrawMesh(StencilBufferRender.quad, Matrix4x4.identity, this._postProcessMat, 0, 0);
    this._cmdBuffer.SetGlobalTexture("_StencilBufferGlobalTex", (RenderTargetIdentifier) (Texture) StencilBufferRender._stencilRenderTexture);
    this._cam.AddCommandBuffer(this.camInsertPoint, this._cmdBuffer);
  }

  public void DestroyCommandBuffer()
  {
    if (this._cmdBuffer != null)
    {
      this._cam.RemoveCommandBuffer(this.camInsertPoint, this._cmdBuffer);
      this._cmdBuffer.Clear();
      this._cmdBuffer.Dispose();
      this._cmdBuffer = (CommandBuffer) null;
    }
    foreach (CommandBuffer commandBuffer in this._cam.GetCommandBuffers(this.camInsertPoint))
    {
      if (commandBuffer.name == "StencilBufferRT")
      {
        this._cam.RemoveCommandBuffer(this.camInsertPoint, commandBuffer);
        commandBuffer.Clear();
        commandBuffer.Dispose();
      }
    }
    if (!((Object) StencilBufferRender._stencilRenderTexture != (Object) null))
      return;
    StencilBufferRender._stencilRenderTexture.Release();
    StencilBufferRender._stencilRenderTexture.DiscardContents();
    Object.Destroy((Object) StencilBufferRender._stencilRenderTexture);
    StencilBufferRender._stencilRenderTexture = (RenderTexture) null;
  }

  public void OnDestroy() => this.DestroyCommandBuffer();
}
