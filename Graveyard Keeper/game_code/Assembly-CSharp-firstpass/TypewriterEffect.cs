// Decompiled with JetBrains decompiler
// Type: TypewriterEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UILabel))]
[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
  public static TypewriterEffect current;
  public int charsPerSecond = 20;
  public float fadeInTime;
  public float delayOnPeriod;
  public float delayOnNewLine;
  public UIScrollView scrollView;
  public bool keepFullDimensions;
  public List<EventDelegate> onFinished = new List<EventDelegate>();
  public UILabel mLabel;
  public string mFullText = "";
  public int mCurrentOffset;
  public float mNextChar;
  public bool mReset = true;
  public bool mActive;
  public BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

  public bool isActive => this.mActive;

  public void ResetToBeginning()
  {
    this.Finish();
    this.mReset = true;
    this.mActive = true;
    this.mNextChar = 0.0f;
    this.mCurrentOffset = 0;
    this.Update();
  }

  public void Finish()
  {
    if (!this.mActive)
      return;
    this.mActive = false;
    if (!this.mReset)
    {
      this.mCurrentOffset = this.mFullText.Length;
      this.mFade.Clear();
      this.mLabel.text = this.mFullText;
    }
    if (this.keepFullDimensions && (Object) this.scrollView != (Object) null)
      this.scrollView.UpdatePosition();
    TypewriterEffect.current = this;
    EventDelegate.Execute(this.onFinished);
    TypewriterEffect.current = (TypewriterEffect) null;
  }

  public void OnEnable()
  {
    this.mReset = true;
    this.mActive = true;
  }

  public void OnDisable() => this.Finish();

  public void Update()
  {
    if (!this.mActive)
      return;
    if (this.mReset)
    {
      this.mCurrentOffset = 0;
      this.mReset = false;
      this.mLabel = this.GetComponent<UILabel>();
      this.mFullText = this.mLabel.processedText;
      this.mFade.Clear();
      if (this.keepFullDimensions && (Object) this.scrollView != (Object) null)
        this.scrollView.UpdatePosition();
    }
    if (string.IsNullOrEmpty(this.mFullText))
      return;
    while (this.mCurrentOffset < this.mFullText.Length && (double) this.mNextChar <= (double) RealTime.time)
    {
      int mCurrentOffset = this.mCurrentOffset;
      this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
      if (this.mLabel.supportEncoding)
      {
        while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
          ;
      }
      ++this.mCurrentOffset;
      if (this.mCurrentOffset <= this.mFullText.Length)
      {
        float num = 1f / (float) this.charsPerSecond;
        char ch = mCurrentOffset < this.mFullText.Length ? this.mFullText[mCurrentOffset] : '\n';
        if (ch == '\n')
          num += this.delayOnNewLine;
        else if (mCurrentOffset + 1 == this.mFullText.Length || this.mFullText[mCurrentOffset + 1] <= ' ')
        {
          switch (ch)
          {
            case '!':
            case '?':
              num += this.delayOnPeriod;
              break;
            case '.':
              if (mCurrentOffset + 2 < this.mFullText.Length && this.mFullText[mCurrentOffset + 1] == '.' && this.mFullText[mCurrentOffset + 2] == '.')
              {
                num += this.delayOnPeriod * 3f;
                mCurrentOffset += 2;
                break;
              }
              num += this.delayOnPeriod;
              break;
          }
        }
        if ((double) this.mNextChar == 0.0)
          this.mNextChar = RealTime.time + num;
        else
          this.mNextChar += num;
        if ((double) this.fadeInTime != 0.0)
        {
          this.mFade.Add(new TypewriterEffect.FadeEntry()
          {
            index = mCurrentOffset,
            alpha = 0.0f,
            text = this.mFullText.Substring(mCurrentOffset, this.mCurrentOffset - mCurrentOffset)
          });
        }
        else
        {
          this.mLabel.text = this.keepFullDimensions ? $"{this.mFullText.Substring(0, this.mCurrentOffset)}[00]{this.mFullText.Substring(this.mCurrentOffset)}" : this.mFullText.Substring(0, this.mCurrentOffset);
          if (!this.keepFullDimensions && (Object) this.scrollView != (Object) null)
            this.scrollView.UpdatePosition();
        }
      }
      else
        break;
    }
    if (this.mCurrentOffset >= this.mFullText.Length)
    {
      this.mLabel.text = this.mFullText;
      TypewriterEffect.current = this;
      EventDelegate.Execute(this.onFinished);
      TypewriterEffect.current = (TypewriterEffect) null;
      this.mActive = false;
    }
    else
    {
      if (this.mFade.size == 0)
        return;
      int num = 0;
      while (num < this.mFade.size)
      {
        TypewriterEffect.FadeEntry fadeEntry = this.mFade[num];
        fadeEntry.alpha += RealTime.deltaTime / this.fadeInTime;
        if ((double) fadeEntry.alpha < 1.0)
        {
          this.mFade[num] = fadeEntry;
          ++num;
        }
        else
          this.mFade.RemoveAt(num);
      }
      if (this.mFade.size == 0)
      {
        if (this.keepFullDimensions)
          this.mLabel.text = $"{this.mFullText.Substring(0, this.mCurrentOffset)}[00]{this.mFullText.Substring(this.mCurrentOffset)}";
        else
          this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < this.mFade.size; ++i)
        {
          TypewriterEffect.FadeEntry fadeEntry = this.mFade[i];
          if (i == 0)
            stringBuilder.Append(this.mFullText.Substring(0, fadeEntry.index));
          stringBuilder.Append('[');
          stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry.alpha));
          stringBuilder.Append(']');
          stringBuilder.Append(fadeEntry.text);
        }
        if (this.keepFullDimensions)
        {
          stringBuilder.Append("[00]");
          stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
        }
        this.mLabel.text = stringBuilder.ToString();
      }
    }
  }

  public struct FadeEntry
  {
    public int index;
    public string text;
    public float alpha;
  }
}
