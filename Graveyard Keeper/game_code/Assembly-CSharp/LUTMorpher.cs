// Decompiled with JetBrains decompiler
// Type: LUTMorpher
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LUTMorpher : MonoBehaviour
{
  public AmplifyColorEffect lut;
  public float speed = 0.2f;
  public int _cur_morph_dir;
  public Texture _lut_tx;
  public bool _lut;
  public Texture _lut_override_tx;
  public bool _lut_override;
  public Texture _next_lut_tx;
  public bool _next_lut;
  public const float DEFAULT_SPEED = 0.2f;

  public void SetMainLUT(Texture texture, float morph_speed = 0.2f)
  {
    Debug.Log((object) $"SetMainLUT, texture = {((Object) texture == (Object) null ? "null" : texture.name)}, morph_speed = {morph_speed.ToString()}");
    this._lut_tx = texture;
    this.speed = morph_speed;
    if (this._lut_override)
      return;
    this.TryStartMorph(texture);
  }

  public void SetOverrideLUT(Texture texture, float morph_speed = 0.2f)
  {
    Debug.Log((object) $"SetOverrideLUT, texture = {((Object) texture == (Object) null ? "null" : texture.name)}, morph_speed = {morph_speed.ToString()}");
    this._lut_override_tx = texture;
    this._lut_override = (Object) texture != (Object) null;
    if (this._cur_morph_dir == 0 && (Object) texture == (Object) null)
      texture = this._lut_tx;
    this.speed = morph_speed;
    this.TryStartMorph(texture);
  }

  public void TryStartMorph(Texture texture)
  {
    if (this._cur_morph_dir == 0)
    {
      this.StartMorph(texture, (double) this.lut.BlendAmount > 0.5 ? -1 : 1);
    }
    else
    {
      this._next_lut_tx = texture;
      this._next_lut = true;
    }
  }

  public void Update()
  {
    if (this._cur_morph_dir == 0)
      return;
    float num = this.lut.BlendAmount + this.speed * Time.deltaTime * (float) this._cur_morph_dir;
    if ((double) num > 1.0)
    {
      num = 1f;
      this._cur_morph_dir = 0;
      if (this._next_lut)
      {
        this._next_lut = false;
        this.StartMorph(this._next_lut_tx, -1);
        this._next_lut_tx = (Texture) null;
      }
    }
    else if ((double) num < 0.0)
    {
      num = 0.0f;
      this._cur_morph_dir = 0;
      if (this._next_lut)
      {
        this._next_lut = false;
        this.StartMorph(this._next_lut_tx, 1);
        this._next_lut_tx = (Texture) null;
      }
    }
    this.lut.BlendAmount = num;
  }

  public void StartMorph(Texture target_tx, int dir)
  {
    Debug.Log((object) $"StartMorph {((Object) target_tx == (Object) null ? "null" : target_tx.name)}, dir = {dir.ToString()}");
    if (this._cur_morph_dir != 0)
    {
      Debug.LogError((object) "LUTMorpher: Can't start morph when previous is not over");
    }
    else
    {
      this._cur_morph_dir = dir;
      if (dir > 0)
        this.lut.LutBlendTexture = target_tx;
      else
        this.lut.LutTexture = target_tx;
    }
  }
}
