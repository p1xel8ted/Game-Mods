// Decompiled with JetBrains decompiler
// Type: TargetMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class TargetMarker : BaseMonoBehaviour
{
  private SpriteRenderer spriterenderer;
  private float xScale = 1f;
  private float yScale = 1f;
  private float xScaleSpeed;
  private float yScaleSpeed;
  private float zOffset;

  private void Start()
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

  private void Update()
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
