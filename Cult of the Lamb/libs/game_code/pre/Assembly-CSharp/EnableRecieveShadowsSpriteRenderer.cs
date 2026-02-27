// Decompiled with JetBrains decompiler
// Type: EnableRecieveShadowsSpriteRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class EnableRecieveShadowsSpriteRenderer : BaseMonoBehaviour
{
  public static EnableRecieveShadowsSpriteRenderer Instance;

  private void OnEnable()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.UpdateSpriteRenderers);
  }

  private void OnDisable()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.UpdateSpriteRenderers);
  }

  public void UpdateSpriteRenderers()
  {
    foreach (Renderer renderer in UnityEngine.Object.FindObjectsOfType((System.Type) typeof (SpriteRenderer)) as SpriteRenderer[])
      renderer.receiveShadows = true;
    foreach (Renderer renderer in UnityEngine.Object.FindObjectsOfType((System.Type) typeof (SpriteShapeRenderer)) as SpriteShapeRenderer[])
      renderer.receiveShadows = true;
  }

  public static void UpdateSpriteShadows()
  {
    EnableRecieveShadowsSpriteRenderer.Instance.UpdateSpriteRenderers();
  }

  private void Start()
  {
    EnableRecieveShadowsSpriteRenderer.Instance = this;
    this.UpdateSpriteRenderers();
  }
}
