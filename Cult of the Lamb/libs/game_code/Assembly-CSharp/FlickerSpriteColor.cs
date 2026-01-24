// Decompiled with JetBrains decompiler
// Type: FlickerSpriteColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FlickerSpriteColor : BaseMonoBehaviour
{
  public float flickerAmt = 0.15f;
  public float lightStrength = 0.8f;
  public float flickerSpeed = 0.4f;
  public Color baseLight;
  public SpriteRenderer Light;
  public float timeOffset;

  public void Start()
  {
    this.Light = this.GetComponent<SpriteRenderer>();
    this.timeOffset = Random.value * 7f;
  }

  public void Update()
  {
    float num1 = (Time.time + this.timeOffset) * this.flickerSpeed;
    float num2 = (float) ((double) Mathf.Sin(num1 * 7f) * (double) this.flickerAmt + (double) Mathf.Sin(num1 * 3f) * (double) this.flickerAmt) + this.lightStrength;
    this.Light.color = new Color(this.baseLight.r * num2, this.baseLight.g * num2, this.baseLight.b * num2, 1f);
  }
}
