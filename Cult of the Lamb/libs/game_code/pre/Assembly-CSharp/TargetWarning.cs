// Decompiled with JetBrains decompiler
// Type: TargetWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TargetWarning : BaseMonoBehaviour
{
  private Vector3 TargetScale;
  public SpriteRenderer TargetSprite;
  public SpriteRenderer TargetWarningSprite;
  private bool SizeSet;
  private float Scale;
  private float Rotation;
  private float RotationSpeed = 50f;
  public Color Color1;
  public Color Color2;
  private Color CurrentColor;

  private void OnEnable()
  {
    if (!this.SizeSet)
      this.TargetScale = this.TargetSprite.transform.localScale;
    this.SizeSet = true;
    this.Scale = 0.0f;
    this.TargetSprite.transform.localScale = Vector3.one * this.Scale;
    this.TargetWarningSprite.transform.localScale = Vector3.one * this.Scale;
  }

  private void Update()
  {
    if ((double) (this.Scale += Time.deltaTime * 20f) <= (double) this.TargetScale.x)
    {
      this.TargetSprite.transform.localScale = Vector3.one * this.Scale;
      this.TargetWarningSprite.transform.localScale = Vector3.one * this.Scale;
    }
    this.TargetSprite.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation += Time.deltaTime * this.RotationSpeed);
    if (Time.frameCount % 5 != 0)
      return;
    this.ToggleColor();
  }

  private void ToggleColor()
  {
    this.TargetSprite.material.SetColor("_Color", this.CurrentColor = this.CurrentColor == this.Color1 ? this.Color2 : this.Color1);
    this.TargetWarningSprite.material.SetColor("_Color", this.CurrentColor);
  }
}
