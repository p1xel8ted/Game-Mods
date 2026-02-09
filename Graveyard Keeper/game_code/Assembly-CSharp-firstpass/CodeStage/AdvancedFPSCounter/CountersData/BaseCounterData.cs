// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.CountersData.BaseCounterData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using CodeStage.AdvancedFPSCounter.Labels;
using System;
using System.Text;
using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.CountersData;

[AddComponentMenu("")]
[Serializable]
public abstract class BaseCounterData
{
  public const string BOLD_START = "<b>";
  public const string BOLD_END = "</b>";
  public const string ITALIC_START = "<i>";
  public const string ITALIC_END = "</i>";
  public StringBuilder text;
  public bool dirty;
  public AFPSCounter main;
  public string colorCached;
  public bool inited;
  [SerializeField]
  public bool enabled = true;
  [Tooltip("Current anchoring label for the counter output.\nRefreshes both previous and specified label when switching anchor.")]
  [SerializeField]
  public LabelAnchor anchor;
  [Tooltip("Regular color of the counter output.")]
  [SerializeField]
  public Color color;
  [Tooltip("Controls text style.")]
  [SerializeField]
  public FontStyle style;
  [Tooltip("Additional text to append to the end of the counter in normal Operation Mode.")]
  public string extraText;

  public bool Enabled
  {
    get => this.enabled;
    set
    {
      if (this.enabled == value || !Application.isPlaying)
        return;
      this.enabled = value;
      if (this.enabled)
        this.Activate();
      else
        this.Deactivate();
      this.main.UpdateTexts();
    }
  }

  public LabelAnchor Anchor
  {
    get => this.anchor;
    set
    {
      if (this.anchor == value || !Application.isPlaying)
        return;
      LabelAnchor anchor = this.anchor;
      this.anchor = value;
      if (!this.enabled)
        return;
      this.dirty = true;
      this.main.MakeDrawableLabelDirty(anchor);
      this.main.UpdateTexts();
    }
  }

  public Color Color
  {
    get => this.color;
    set
    {
      if (this.color == value || !Application.isPlaying)
        return;
      this.color = value;
      if (!this.enabled)
        return;
      this.CacheCurrentColor();
      this.Refresh();
    }
  }

  public FontStyle Style
  {
    get => this.style;
    set
    {
      if (this.style == value || !Application.isPlaying)
        return;
      this.style = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public string ExtraText
  {
    get => this.extraText;
    set
    {
      if (this.extraText == value || !Application.isPlaying)
        return;
      this.extraText = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public void Refresh()
  {
    if (!this.enabled || !Application.isPlaying)
      return;
    this.UpdateValue(true);
    this.main.UpdateTexts();
  }

  public virtual void UpdateValue() => this.UpdateValue(false);

  public abstract void UpdateValue(bool force);

  public void Init(AFPSCounter reference) => this.main = reference;

  public void Dispose()
  {
    this.main = (AFPSCounter) null;
    if (this.text == null)
      return;
    this.text.Remove(0, this.text.Length);
    this.text = (StringBuilder) null;
  }

  public virtual void Activate()
  {
    if (!this.enabled || this.main.OperationMode == OperationMode.Disabled || !this.HasData())
      return;
    if (this.text == null)
      this.text = new StringBuilder(500);
    else
      this.text.Length = 0;
    if (this.main.OperationMode == OperationMode.Normal && this.colorCached == null)
      this.CacheCurrentColor();
    this.PerformActivationActions();
    if (!this.inited)
    {
      this.PerformInitActions();
      this.inited = true;
    }
    this.UpdateValue();
  }

  public virtual void Deactivate()
  {
    if (!this.inited)
      return;
    if (this.text != null)
      this.text.Remove(0, this.text.Length);
    this.main.MakeDrawableLabelDirty(this.anchor);
    this.PerformDeActivationActions();
    this.inited = false;
  }

  public virtual void PerformInitActions()
  {
  }

  public virtual void PerformActivationActions()
  {
  }

  public virtual void PerformDeActivationActions()
  {
  }

  public abstract bool HasData();

  public abstract void CacheCurrentColor();

  public void ApplyTextStyles()
  {
    if (this.text.Length > 0)
    {
      switch (this.style)
      {
        case FontStyle.Normal:
          break;
        case FontStyle.Bold:
          this.text.Insert(0, "<b>");
          this.text.Append("</b>");
          break;
        case FontStyle.Italic:
          this.text.Insert(0, "<i>");
          this.text.Append("</i>");
          break;
        case FontStyle.BoldAndItalic:
          this.text.Insert(0, "<b>");
          this.text.Append("</b>");
          this.text.Insert(0, "<i>");
          this.text.Append("</i>");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    if (string.IsNullOrEmpty(this.extraText))
      return;
    this.text.Append('\n').Append(this.extraText);
  }
}
