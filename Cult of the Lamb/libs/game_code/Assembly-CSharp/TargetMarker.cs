// Decompiled with JetBrains decompiler
// Type: TargetMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class TargetMarker : BaseMonoBehaviour
{
  public SpriteRenderer spriterenderer;
  public float xScale = 1f;
  public float yScale = 1f;
  public float xScaleSpeed;
  public float yScaleSpeed;
  public float zOffset;

  public void Start()
  {
    this.spriterenderer = this.GetComponent<SpriteRenderer>();
    this.spriterenderer.color -= new Color(0.0f, 0.0f, 0.0f, 1f);
  }

  public void reveal(Vector3 position)
  {
    this.spriterenderer.color = Color.white;
    this.xScale = 2f;
    this.yScale = 1.5f;
    this.transform.position = position;
  }

  public void Update()
  {
    this.xScale += (float) ((1.0 - (double) this.xScale) / 10.0);
    this.yScale += (float) ((1.0 - (double) this.yScale) / 5.0);
    this.gameObject.transform.localScale = new Vector3(this.xScale, this.yScale, 1f);
    if ((double) this.spriterenderer.color.a <= 0.0)
      return;
    this.spriterenderer.color -= new Color(0.0f, 0.0f, 0.0f, 0.05f);
    if ((double) this.spriterenderer.color.a > 0.0)
      return;
    this.spriterenderer.color -= new Color(1f, 1f, 1f, 0.0f);
  }
}
