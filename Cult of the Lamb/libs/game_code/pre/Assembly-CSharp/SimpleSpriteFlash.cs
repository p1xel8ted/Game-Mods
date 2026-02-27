// Decompiled with JetBrains decompiler
// Type: SimpleSpriteFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleSpriteFlash : BaseMonoBehaviour
{
  private MaterialPropertyBlock BlockFlash;
  private SpriteRenderer _spriteRenderer;
  private int fillAlpha;
  private int fillColor;
  private Color WarningColour = Color.white;
  [HideInInspector]
  public bool isFillWhite;
  private int NumColorToApply;
  private Color ColorToApply;

  private SpriteRenderer spriteRenderer
  {
    get
    {
      if ((Object) this._spriteRenderer == (Object) null)
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
      return this._spriteRenderer;
    }
  }

  private void Start()
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

  private void LateUpdate()
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
