// Decompiled with JetBrains decompiler
// Type: EnableRecieveShadowsSpriteRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
[ExecuteAlways]
[DisallowMultipleComponent]
public class EnableRecieveShadowsSpriteRenderer : BaseMonoBehaviour
{
  public static EnableRecieveShadowsSpriteRenderer Instance;

  public void UpdateSpriteRenderers()
  {
    SpriteRenderer[] srs;
    SpriteShapeRenderer[] ssrs;
    EnableRecieveShadowsSpriteRenderer.GetAllTargetRenderers(out srs, out ssrs);
    int num1 = 0;
    int num2 = 0;
    if (srs != null)
    {
      foreach (SpriteRenderer spriteRenderer in srs)
      {
        if ((bool) (Object) spriteRenderer)
        {
          spriteRenderer.receiveShadows = true;
          ++num1;
        }
      }
    }
    if (ssrs == null)
      return;
    foreach (SpriteShapeRenderer spriteShapeRenderer in ssrs)
    {
      if ((bool) (Object) spriteShapeRenderer)
      {
        spriteShapeRenderer.receiveShadows = true;
        ++num2;
      }
    }
  }

  public static void UpdateSpriteShadows()
  {
    if (!((Object) EnableRecieveShadowsSpriteRenderer.Instance != (Object) null))
      return;
    EnableRecieveShadowsSpriteRenderer.Instance.UpdateSpriteRenderers();
  }

  public void OnEnable()
  {
    EnableRecieveShadowsSpriteRenderer.Instance = this;
    try
    {
      BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.UpdateSpriteRenderers);
    }
    catch
    {
    }
    if (Application.isPlaying)
      return;
    this.UpdateSpriteRenderers();
  }

  public void OnDisable()
  {
    try
    {
      BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.UpdateSpriteRenderers);
    }
    catch
    {
    }
  }

  public void Start()
  {
    if (!Application.isPlaying)
      return;
    this.UpdateSpriteRenderers();
  }

  public static T[] FindAllInOpenScenes<T>() where T : Component
  {
    return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
  }

  public static void GetAllTargetRenderers(out SpriteRenderer[] srs, out SpriteShapeRenderer[] ssrs)
  {
    srs = EnableRecieveShadowsSpriteRenderer.FindAllInOpenScenes<SpriteRenderer>();
    ssrs = EnableRecieveShadowsSpriteRenderer.FindAllInOpenScenes<SpriteShapeRenderer>();
  }
}
