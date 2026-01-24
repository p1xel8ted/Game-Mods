// Decompiled with JetBrains decompiler
// Type: SimpleSpriteFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleSpriteFlash : BaseMonoBehaviour
{
  public MaterialPropertyBlock BlockFlash;
  public SpriteRenderer _spriteRenderer;
  public int fillAlpha;
  public int fillColor;
  public Color WarningColour = Color.white;
  [HideInInspector]
  public bool isFillWhite;
  public int NumColorToApply;
  public Color ColorToApply;

  public SpriteRenderer spriteRenderer
  {
    get
    {
      if ((Object) this._spriteRenderer == (Object) null)
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
      return this._spriteRenderer;
    }
  }

  public void Start()
  {
    this.fillColor = Shader.PropertyToID("_FillColor");
    this.fillAlpha = Shader.PropertyToID("_FillAlpha");
    this.BlockFlash = new MaterialPropertyBlock();
  }

  public void FlashMeWhite()
  {
    if (Time.frameCount % 5 != 0)
      return;
    this.FlashWhite(!this.isFillWhite);
  }

  public void FlashWhite(bool toggle)
  {
    this.SetColor(this.WarningColour with
    {
      a = toggle ? 0.5f : 0.0f
    });
    this.isFillWhite = toggle;
  }

  public void FlashWhite(float amt)
  {
    this.isFillWhite = (double) amt > 0.0;
    this.SetColor(this.WarningColour with
    {
      a = Mathf.Lerp(0.0f, 1f, amt)
    });
  }

  public void SetColor(Color color)
  {
    this.ColorToApply += color;
    ++this.NumColorToApply;
  }

  public void LateUpdate()
  {
    if (this.NumColorToApply <= 0)
      return;
    if (this.BlockFlash == null)
      this.BlockFlash = new MaterialPropertyBlock();
    this.BlockFlash.SetTexture("_MainTex", (Texture) this.spriteRenderer.sprite.texture);
    this.BlockFlash.SetColor(this.fillColor, this.ColorToApply / (float) this.NumColorToApply);
    this.BlockFlash.SetFloat(this.fillAlpha, this.ColorToApply.a / (float) this.NumColorToApply);
    this.spriteRenderer.SetPropertyBlock(this.BlockFlash);
    this.NumColorToApply = 0;
    this.ColorToApply = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  }
}
