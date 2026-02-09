// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.EdgeDetection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
public class EdgeDetection : PostEffectsBase
{
  public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;
  public float sensitivityDepth = 1f;
  public float sensitivityNormals = 1f;
  public float lumThreshold = 0.2f;
  public float edgeExp = 1f;
  public float sampleDist = 1f;
  public float edgesOnly;
  public Color edgesOnlyBgColor = Color.white;
  public Shader edgeDetectShader;
  public Material edgeDetectMaterial;
  public EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.edgeDetectMaterial = this.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
    if (this.mode != this.oldMode)
      this.SetCameraFlag();
    this.oldMode = this.mode;
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public new void Start() => this.oldMode = this.mode;

  public void SetCameraFlag()
  {
    if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
    {
      this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }
    else
    {
      if (this.mode != EdgeDetection.EdgeDetectMode.TriangleDepthNormals && this.mode != EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
        return;
      this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }
  }

  public new void OnEnable() => this.SetCameraFlag();

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      Vector2 vector2 = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
      this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector2.x, vector2.y, 1f, vector2.y));
      this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
      this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
      this.edgeDetectMaterial.SetVector("_BgColor", (Vector4) this.edgesOnlyBgColor);
      this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
      this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
      Graphics.Blit((Texture) source, destination, this.edgeDetectMaterial, (int) this.mode);
    }
  }

  public enum EdgeDetectMode
  {
    TriangleDepthNormals,
    RobertsCrossDepthNormals,
    SobelDepth,
    SobelDepthThin,
    TriangleLuminance,
  }
}
