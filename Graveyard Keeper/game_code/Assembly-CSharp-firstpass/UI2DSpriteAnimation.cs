// Decompiled with JetBrains decompiler
// Type: UI2DSpriteAnimation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class UI2DSpriteAnimation : MonoBehaviour
{
  public int frameIndex;
  [SerializeField]
  public int framerate = 20;
  public bool ignoreTimeScale = true;
  public bool loop = true;
  public UnityEngine.Sprite[] frames;
  public SpriteRenderer mUnitySprite;
  public UI2DSprite mNguiSprite;
  public float mUpdate;

  public bool isPlaying => this.enabled;

  public int framesPerSecond
  {
    get => this.framerate;
    set => this.framerate = value;
  }

  public void Play()
  {
    if (this.frames == null || this.frames.Length == 0)
      return;
    if (!this.enabled && !this.loop)
    {
      int num = this.framerate > 0 ? this.frameIndex + 1 : this.frameIndex - 1;
      if (num < 0 || num >= this.frames.Length)
        this.frameIndex = this.framerate < 0 ? this.frames.Length - 1 : 0;
    }
    this.enabled = true;
    this.UpdateSprite();
  }

  public void Pause() => this.enabled = false;

  public void ResetToBeginning()
  {
    this.frameIndex = this.framerate < 0 ? this.frames.Length - 1 : 0;
    this.UpdateSprite();
  }

  public void Start() => this.Play();

  public void Update()
  {
    if (this.frames == null || this.frames.Length == 0)
    {
      this.enabled = false;
    }
    else
    {
      if (this.framerate == 0)
        return;
      float num = this.ignoreTimeScale ? RealTime.time : Time.time;
      if ((double) this.mUpdate >= (double) num)
        return;
      this.mUpdate = num;
      int val = this.framerate > 0 ? this.frameIndex + 1 : this.frameIndex - 1;
      if (!this.loop && (val < 0 || val >= this.frames.Length))
      {
        this.enabled = false;
      }
      else
      {
        this.frameIndex = NGUIMath.RepeatIndex(val, this.frames.Length);
        this.UpdateSprite();
      }
    }
  }

  public void UpdateSprite()
  {
    if ((Object) this.mUnitySprite == (Object) null && (Object) this.mNguiSprite == (Object) null)
    {
      this.mUnitySprite = this.GetComponent<SpriteRenderer>();
      this.mNguiSprite = this.GetComponent<UI2DSprite>();
      if ((Object) this.mUnitySprite == (Object) null && (Object) this.mNguiSprite == (Object) null)
      {
        this.enabled = false;
        return;
      }
    }
    float num = this.ignoreTimeScale ? RealTime.time : Time.time;
    if (this.framerate != 0)
      this.mUpdate = num + Mathf.Abs(1f / (float) this.framerate);
    if ((Object) this.mUnitySprite != (Object) null)
    {
      this.mUnitySprite.sprite = this.frames[this.frameIndex];
    }
    else
    {
      if (!((Object) this.mNguiSprite != (Object) null))
        return;
      this.mNguiSprite.nextSprite = this.frames[this.frameIndex];
    }
  }
}
