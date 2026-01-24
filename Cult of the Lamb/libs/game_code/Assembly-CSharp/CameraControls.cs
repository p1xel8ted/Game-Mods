// Decompiled with JetBrains decompiler
// Type: CameraControls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class CameraControls : BaseMonoBehaviour
{
  public AmplifyColorEffect lightingColor;
  public AmplifyColorEffect shadowColor;
  public Material material;
  public RenderTexture renderTexture1;
  public RenderTexture renderTexture2;
  public Material AddDepthMaterial;
  public bool invertTexture = true;
  public bool addDepth = true;

  public void OnEnable()
  {
    if (!(bool) (Object) Camera.main)
      return;
    if (this.addDepth)
    {
      if (SettingsManager.Settings.Game.PerformanceMode)
        return;
      Camera.main.depthTextureMode |= DepthTextureMode.Depth;
    }
    else
      Camera.main.depthTextureMode = DepthTextureMode.None;
  }

  public void OnDisable()
  {
    if (!(bool) (Object) Camera.main)
      return;
    Camera.main.depthTextureMode = DepthTextureMode.None;
  }

  public void OnPreRender()
  {
    if (!this.invertTexture || !((Object) this.material != (Object) null))
      return;
    this.material.SetTexture("_RenderTex_1", (Texture) this.renderTexture1);
    this.material.SetTexture("_RenderTex_2", (Texture) this.renderTexture2);
    Graphics.Blit((Texture) this.renderTexture2, this.renderTexture2, this.material, -1);
  }

  public void Start() => this.StartCoroutine((IEnumerator) this.FindPlayer());

  public void Update()
  {
  }

  public IEnumerator FindPlayer()
  {
    if (SceneManager.GetActiveScene().name == "Game - Possess Followers")
    {
      while ((Object) PlayerSpirit.Instance == (Object) null)
        yield return (object) new WaitForSeconds(1f);
    }
    else
    {
      while ((Object) PlayerFarming.Instance == (Object) null)
        yield return (object) new WaitForSeconds(1f);
    }
  }
}
