// Decompiled with JetBrains decompiler
// Type: FlickerSpriteColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FlickerSpriteColor : BaseMonoBehaviour
{
  public float flickerAmt = 0.15f;
  public float lightStrength = 0.8f;
  public float flickerSpeed = 0.4f;
  public Color baseLight;
  public SpriteRenderer Light;
  private float timeOffset;

  public void Start()
  {
    this.Light = this.GetComponent<SpriteRenderer>();
    this.timeOffset = Random.value * 7f;
  }

  private void Update()
  {
    float num1 = (Time.time + this.timeOffset) * this.flickerSpeed;
    float num2 = (float) ((double) Mathf.Sin(num1 * 7f) * (double) this.flickerAmt + (double) Mathf.Sin(num1 * 3f) * (double) this.flickerAmt) + this.lightStrength;
    this.Light.color = new Color(this.baseLight.r * num2, this.baseLight.g * num2, this.baseLight.b * num2, 1f);
  }
}
