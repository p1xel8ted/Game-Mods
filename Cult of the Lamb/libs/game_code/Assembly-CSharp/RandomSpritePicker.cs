// Decompiled with JetBrains decompiler
// Type: RandomSpritePicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RandomSpritePicker : BaseMonoBehaviour
{
  public bool sprites;
  public Sprite[] Sprites;
  public bool Scale;
  public bool randomFlipX;
  public bool randomFlipY;
  public float scaleLow = 0.8f;
  public float scaleHigh = 1.1f;
  public float randomScaleFloat;
  public Vector3 randomScale3 = new Vector3(-40.1f, -40.1f, -40.1f);
  public bool Position;
  public float OFFSETLow = -2f;
  public float OFFSETHigh = 2f;
  public bool Rotation;
  public Vector2 MinMaxValueSlider = new Vector2(-2f, 2f);
  public bool Colour;
  public bool tintOnDistance;
  public Color[] Colours;
  [Space]
  public bool hasRandomised;
  [Space]
  public bool lockIn;
  public int randomInt;
  public float dist;
  public bool TURN_OFF_ON_LOW_QUALITY = true;

  public void Randomise()
  {
    this.hasRandomised = false;
    this.randomise_();
  }

  public void LockItIn() => this.lockIn = !this.lockIn;

  public void UnlockIt() => this.lockIn = !this.lockIn;

  public void Start()
  {
    int num = this.TURN_OFF_ON_LOW_QUALITY ? 1 : 0;
    this.randomise_();
  }

  public void randomise_()
  {
    if (this.lockIn || this.hasRandomised)
      return;
    SpriteRenderer component = this.gameObject.GetComponent<SpriteRenderer>();
    if (!((Object) component != (Object) null))
      return;
    if (this.Sprites.Length != 0)
    {
      int index = Random.Range(0, this.Sprites.Length);
      component.sprite = this.Sprites[index];
      if ((Object) component.sprite == (Object) null)
      {
        Object.Destroy((Object) this.gameObject);
        return;
      }
    }
    if (this.Scale)
    {
      this.randomScaleFloat = Random.Range(this.scaleLow, this.scaleHigh);
      this.randomScale3 = new Vector3(this.randomScaleFloat, this.randomScaleFloat, this.randomScaleFloat);
      this.transform.localScale = this.randomScale3;
    }
    if (this.randomFlipX && (double) Random.value < 0.5)
      this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    if (this.randomFlipY && (double) Random.value < 0.5)
      this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z);
    if (this.Rotation)
      this.transform.eulerAngles = this.transform.eulerAngles with
      {
        z = Random.Range(this.MinMaxValueSlider.x, this.MinMaxValueSlider.y)
      };
    if (this.Colour && this.Colours.Length != 0)
    {
      int index = Random.Range(0, this.Colours.Length);
      component.color = this.Colours[index];
    }
    if (this.Position)
      this.transform.position = this.transform.position + new Vector3(Random.Range(this.OFFSETLow, this.OFFSETHigh), Random.Range(this.OFFSETLow, this.OFFSETHigh), 0.0f);
    this.hasRandomised = true;
  }
}
