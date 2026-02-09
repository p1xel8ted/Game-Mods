// Decompiled with JetBrains decompiler
// Type: UISpriteAnimation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
public class UISpriteAnimation : MonoBehaviour
{
  public int frameIndex;
  [HideInInspector]
  [SerializeField]
  public int mFPS = 30;
  [SerializeField]
  [HideInInspector]
  public string mPrefix = "";
  [HideInInspector]
  [SerializeField]
  public bool mLoop = true;
  [HideInInspector]
  [SerializeField]
  public bool mSnap = true;
  public UISprite mSprite;
  public float mDelta;
  public bool mActive = true;
  public List<string> mSpriteNames = new List<string>();

  public int frames => this.mSpriteNames.Count;

  public int framesPerSecond
  {
    get => this.mFPS;
    set => this.mFPS = value;
  }

  public string namePrefix
  {
    get => this.mPrefix;
    set
    {
      if (!(this.mPrefix != value))
        return;
      this.mPrefix = value;
      this.RebuildSpriteList();
    }
  }

  public bool loop
  {
    get => this.mLoop;
    set => this.mLoop = value;
  }

  public bool isPlaying => this.mActive;

  public virtual void Start() => this.RebuildSpriteList();

  public virtual void Update()
  {
    if (!this.mActive || this.mSpriteNames.Count <= 1 || !Application.isPlaying || this.mFPS <= 0)
      return;
    this.mDelta += Mathf.Min(1f, RealTime.deltaTime);
    float num = 1f / (float) this.mFPS;
    while ((double) num < (double) this.mDelta)
    {
      this.mDelta = (double) num > 0.0 ? this.mDelta - num : 0.0f;
      if (++this.frameIndex >= this.mSpriteNames.Count)
      {
        this.frameIndex = 0;
        this.mActive = this.mLoop;
      }
      if (this.mActive)
      {
        this.mSprite.spriteName = this.mSpriteNames[this.frameIndex];
        if (this.mSnap)
          this.mSprite.MakePixelPerfect();
      }
    }
  }

  public void RebuildSpriteList()
  {
    if ((Object) this.mSprite == (Object) null)
      this.mSprite = this.GetComponent<UISprite>();
    this.mSpriteNames.Clear();
    if (!((Object) this.mSprite != (Object) null) || !((Object) this.mSprite.atlas != (Object) null))
      return;
    List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
    int index = 0;
    for (int count = spriteList.Count; index < count; ++index)
    {
      UISpriteData uiSpriteData = spriteList[index];
      if (string.IsNullOrEmpty(this.mPrefix) || uiSpriteData.name.StartsWith(this.mPrefix))
        this.mSpriteNames.Add(uiSpriteData.name);
    }
    this.mSpriteNames.Sort();
  }

  public void Play() => this.mActive = true;

  public void Pause() => this.mActive = false;

  public void ResetToBeginning()
  {
    this.mActive = true;
    this.frameIndex = 0;
    if (!((Object) this.mSprite != (Object) null) || this.mSpriteNames.Count <= 0)
      return;
    this.mSprite.spriteName = this.mSpriteNames[this.frameIndex];
    if (!this.mSnap)
      return;
    this.mSprite.MakePixelPerfect();
  }
}
