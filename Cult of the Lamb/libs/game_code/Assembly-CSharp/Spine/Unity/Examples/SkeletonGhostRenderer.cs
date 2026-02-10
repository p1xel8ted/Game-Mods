// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SkeletonGhostRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Spine.Unity.Examples;

public class SkeletonGhostRenderer : MonoBehaviour
{
  public static Color32 TransparentBlack = new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 0);
  public const string colorPropertyName = "_Color";
  public float fadeSpeed = 10f;
  public Color32 startColor;
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  public MaterialPropertyBlock mpb;
  public int colorId;

  public void Awake()
  {
    this.meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    this.meshFilter = this.gameObject.GetComponent<MeshFilter>();
    this.colorId = Shader.PropertyToID("_Color");
    this.mpb = new MaterialPropertyBlock();
  }

  public void Initialize(
    Mesh mesh,
    Material[] materials,
    Color32 color,
    bool additive,
    float speed,
    int sortingLayerID,
    int sortingOrder)
  {
    this.StopAllCoroutines();
    this.gameObject.SetActive(true);
    this.meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    this.meshFilter = this.gameObject.GetComponent<MeshFilter>();
    this.meshRenderer.sharedMaterials = materials;
    this.meshRenderer.sortingLayerID = sortingLayerID;
    this.meshRenderer.sortingOrder = sortingOrder;
    this.meshFilter.sharedMesh = Object.Instantiate<Mesh>(mesh);
    this.startColor = color;
    this.mpb.SetColor(this.colorId, (Color) color);
    this.meshRenderer.SetPropertyBlock(this.mpb);
    this.fadeSpeed = speed;
    if (additive)
      this.StartCoroutine((IEnumerator) this.FadeAdditive());
    else
      this.StartCoroutine((IEnumerator) this.Fade());
  }

  public IEnumerator Fade()
  {
    SkeletonGhostRenderer skeletonGhostRenderer = this;
    Color32 startColor = skeletonGhostRenderer.startColor;
    Color32 black = SkeletonGhostRenderer.TransparentBlack;
    float t = 1f;
    for (float hardTimeLimit = 5f; (double) hardTimeLimit > 0.0; hardTimeLimit -= Time.deltaTime)
    {
      Color32 color32 = Color32.Lerp(black, skeletonGhostRenderer.startColor, t);
      skeletonGhostRenderer.mpb.SetColor(skeletonGhostRenderer.colorId, (Color) color32);
      skeletonGhostRenderer.meshRenderer.SetPropertyBlock(skeletonGhostRenderer.mpb);
      t = Mathf.Lerp(t, 0.0f, Time.deltaTime * skeletonGhostRenderer.fadeSpeed);
      if ((double) t > 0.0)
        yield return (object) null;
      else
        break;
    }
    Object.Destroy((Object) skeletonGhostRenderer.meshFilter.sharedMesh);
    skeletonGhostRenderer.gameObject.SetActive(false);
  }

  public IEnumerator FadeAdditive()
  {
    SkeletonGhostRenderer skeletonGhostRenderer = this;
    Color32 startColor = skeletonGhostRenderer.startColor;
    Color32 black = SkeletonGhostRenderer.TransparentBlack;
    float t = 1f;
    for (float hardTimeLimit = 5f; (double) hardTimeLimit > 0.0; hardTimeLimit -= Time.deltaTime)
    {
      Color32 color32 = Color32.Lerp(black, skeletonGhostRenderer.startColor, t);
      skeletonGhostRenderer.mpb.SetColor(skeletonGhostRenderer.colorId, (Color) color32);
      skeletonGhostRenderer.meshRenderer.SetPropertyBlock(skeletonGhostRenderer.mpb);
      t = Mathf.Lerp(t, 0.0f, Time.deltaTime * skeletonGhostRenderer.fadeSpeed);
      if ((double) t > 0.0)
        yield return (object) null;
      else
        break;
    }
    Object.Destroy((Object) skeletonGhostRenderer.meshFilter.sharedMesh);
    skeletonGhostRenderer.gameObject.SetActive(false);
  }

  public void Cleanup()
  {
    if ((Object) this.meshFilter != (Object) null && (Object) this.meshFilter.sharedMesh != (Object) null)
      Object.Destroy((Object) this.meshFilter.sharedMesh);
    Object.Destroy((Object) this.gameObject);
  }
}
