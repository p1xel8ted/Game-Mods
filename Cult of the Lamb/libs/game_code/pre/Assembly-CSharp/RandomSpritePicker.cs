// Decompiled with JetBrains decompiler
// Type: RandomSpritePicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private float randomScaleFloat;
  private Vector3 randomScale3 = new Vector3(-40.1f, -40.1f, -40.1f);
  public bool Position;
  public float OFFSETLow = -2f;
  public float OFFSETHigh = 2f;
  public bool Rotation;
  public Vector2 MinMaxValueSlider = new Vector2(-2f, 2f);
  public bool Colour;
  public bool tintOnDistance;
  public Color[] Colours;
  [Space]
  private bool hasRandomised;
  [Space]
  public bool lockIn;
  private int randomInt;
  private float dist;
  public bool TURN_OFF_ON_LOW_QUALITY = true;

  private void Randomise()
  {
    this.hasRandomised = false;
    this.randomise_();
  }

  private void LockItIn() => this.lockIn = !this.lockIn;

  private void UnlockIt() => this.lockIn = !this.lockIn;

  private void Start()
  {
    int num = this.TURN_OFF_ON_LOW_QUALITY ? 1 : 0;
    this.randomise_();
  }

  private void randomise_()
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
    }
    if (this.randomFlipX)
    {
      this.randomInt = Random.Range(0, 2);
      if (this.randomInt == 1)
        this.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    if (this.randomFlipY)
    {
      this.randomInt = Random.Range(0, 2);
      if (this.randomInt == 1)
        this.transform.localScale = new Vector3(1f, -1f, 1f);
    }
    if (this.Rotation)
      this.transform.eulerAngles = this.transform.eulerAngles with
      {
        z = Random.Range(this.MinMaxValueSlider.x, this.MinMaxValueSlider.y)
      };
    if (this.Scale)
    {
      this.randomScaleFloat = Random.Range(this.scaleLow, this.scaleHigh);
      this.randomScale3 = new Vector3(this.randomScaleFloat, this.randomScaleFloat, this.randomScaleFloat);
      this.transform.localScale = this.randomScale3;
    }
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
