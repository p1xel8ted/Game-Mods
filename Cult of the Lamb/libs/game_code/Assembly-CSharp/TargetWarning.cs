// Decompiled with JetBrains decompiler
// Type: TargetWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TargetWarning : BaseMonoBehaviour
{
  public Vector3 TargetScale;
  public SpriteRenderer TargetSprite;
  public SpriteRenderer TargetWarningSprite;
  public bool SizeSet;
  public float Scale;
  public float Rotation;
  public float RotationSpeed = 50f;
  public Color Color1;
  public Color Color2;
  public Color CurrentColor;
  public float flashTickTimer;
  public float flashTickMultiplier = 1f;

  public void OnEnable()
  {
    if (!this.SizeSet)
      this.TargetScale = this.TargetSprite.transform.localScale;
    this.SizeSet = true;
    this.Scale = 0.0f;
    this.TargetSprite.transform.localScale = Vector3.one * this.Scale;
    this.TargetWarningSprite.transform.localScale = Vector3.one * this.Scale;
    this.flashTickMultiplier = 1f;
  }

  public void OnDisable() => this.flashTickMultiplier = 1f;

  public void Update()
  {
    if ((double) (this.Scale += Time.deltaTime * 20f) <= (double) this.TargetScale.x)
    {
      this.TargetSprite.transform.localScale = Vector3.one * this.Scale;
      this.TargetWarningSprite.transform.localScale = Vector3.one * this.Scale;
    }
    this.TargetSprite.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.Rotation += Time.deltaTime * this.RotationSpeed);
    if ((double) this.flashTickTimer >= 0.11999999731779099 * (double) this.flashTickMultiplier && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.ToggleColor();
      this.flashTickTimer = 0.0f;
    }
    this.flashTickTimer += Time.deltaTime;
  }

  public void ToggleColor()
  {
    this.TargetSprite.material.SetColor("_Color", this.CurrentColor = this.CurrentColor == this.Color1 ? this.Color2 : this.Color1);
    this.TargetWarningSprite.material.SetColor("_Color", this.CurrentColor);
  }

  public void SetFlashTickMultiplier(float multiplier) => this.flashTickMultiplier = multiplier;
}
