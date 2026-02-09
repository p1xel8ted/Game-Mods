// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.CountersData.FPSCounterData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using CodeStage.AdvancedFPSCounter.Utils;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.CountersData;

[AddComponentMenu("")]
[Serializable]
public class FPSCounterData : UpdatebleCounterData
{
  public const string COLOR_TEXT_START = "<color=#{0}>";
  public const string COLOR_TEXT_END = "</color>";
  public const string FPS_TEXT_START = "<color=#{0}>FPS: ";
  public const string MS_TEXT_START = " <color=#{0}>[";
  public const string MS_TEXT_END = " MS]</color>";
  public const string MIN_TEXT_START = "<color=#{0}>MIN: ";
  public const string MAX_TEXT_START = "<color=#{0}>MAX: ";
  public const string AVG_TEXT_START = "<color=#{0}>AVG: ";
  public const string RENDER_TEXT_START = "<color=#{0}>REN: ";
  public int warningLevelValue = 50;
  public int criticalLevelValue = 20;
  [Tooltip("Average FPS counter accumulative data will be reset on new scene load if enabled.")]
  public bool resetAverageOnNewScene;
  [Tooltip("Minimum and maximum FPS readouts will be reset on new scene load if enabled")]
  public bool resetMinMaxOnNewScene;
  [Range(0.0f, 10f)]
  [Tooltip("Amount of update intervals to skip before recording minimum and maximum FPS.\nUse it to skip initialization performance spikes and drops.")]
  public int minMaxIntervalsToSkip = 3;
  public float newValue;
  public string colorCachedMs;
  public string colorCachedMin;
  public string colorCachedMax;
  public string colorCachedAvg;
  public string colorCachedRender;
  public string colorWarningCached;
  public string colorWarningCachedMs;
  public string colorWarningCachedMin;
  public string colorWarningCachedMax;
  public string colorWarningCachedAvg;
  public string colorCriticalCached;
  public string colorCriticalCachedMs;
  public string colorCriticalCachedMin;
  public string colorCriticalCachedMax;
  public string colorCriticalCachedAvg;
  public int currentAverageSamples;
  public float currentAverageRaw;
  public float[] accumulatedAverageSamples;
  public int minMaxIntervalsSkipped;
  public float renderTimeBank;
  public int previousFrameCount;
  [Tooltip("Shows time in milliseconds spent to render 1 frame.")]
  [SerializeField]
  public bool milliseconds;
  [Tooltip("Shows Average FPS calculated from specified Samples amount or since game / scene start, depending on Samples value and 'Reset On Load' toggle.")]
  [SerializeField]
  public bool average;
  [Tooltip("Shows time in milliseconds for the average FPS.")]
  [SerializeField]
  public bool averageMilliseconds;
  [Tooltip("Controls placing Average on the new line.")]
  [SerializeField]
  public bool averageNewLine;
  [Tooltip("Amount of last samples to get average from. Set 0 to get average from all samples since startup or level load.\nOne Sample recorded per one Interval.")]
  [Range(0.0f, 100f)]
  [SerializeField]
  public int averageSamples = 50;
  [Tooltip("Shows minimum and maximum FPS readouts since game / scene start, depending on 'Reset On Load' toggle.")]
  [SerializeField]
  public bool minMax;
  [Tooltip("Shows time in milliseconds for the Min Max FPS.")]
  [SerializeField]
  public bool minMaxMilliseconds;
  [Tooltip("Controls placing Min Max on the new line.")]
  [SerializeField]
  public bool minMaxNewLine;
  [SerializeField]
  [Tooltip("Check to place Min Max on two separate lines. Otherwise they will be placed on a single line.")]
  public bool minMaxTwoLines;
  [Tooltip("Shows time spent on Camera.Render excluding Image Effects. Add AFPSRenderRecorder to the cameras you wish to count.")]
  [SerializeField]
  public bool render;
  [Tooltip("Controls placing Render on the new line.")]
  [SerializeField]
  public bool renderNewLine;
  [Tooltip("Check to automatically add AFPSRenderRecorder to the Main Camera if present.")]
  [SerializeField]
  public bool renderAutoAdd = true;
  [Tooltip("Color of the FPS counter while FPS is between Critical and Warning levels.")]
  [SerializeField]
  public Color colorWarning = (Color) new Color32((byte) 236, (byte) 224 /*0xE0*/, (byte) 88, byte.MaxValue);
  [SerializeField]
  [Tooltip("Color of the FPS counter while FPS is below Critical level.")]
  public Color colorCritical = (Color) new Color32((byte) 249, (byte) 91, (byte) 91, byte.MaxValue);
  [Tooltip("Color of the Render Time output.")]
  [SerializeField]
  public Color colorRender;
  [CompilerGenerated]
  public int \u003CLastValue\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CLastMillisecondsValue\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CLastRenderValue\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CLastAverageValue\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CLastAverageMillisecondsValue\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CLastMinimumValue\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CLastMaximumValue\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CLastMinMillisecondsValue\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CLastMaxMillisecondsValue\u003Ek__BackingField;
  [CompilerGenerated]
  public FPSLevel \u003CCurrentFpsLevel\u003Ek__BackingField;

  public event Action<FPSLevel> OnFPSLevelChange;

  public bool Milliseconds
  {
    get => this.milliseconds;
    set
    {
      if (this.milliseconds == value || !Application.isPlaying)
        return;
      this.milliseconds = value;
      if (!this.milliseconds)
        this.LastMillisecondsValue = 0.0f;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool Average
  {
    get => this.average;
    set
    {
      if (this.average == value || !Application.isPlaying)
        return;
      this.average = value;
      if (!this.average)
        this.ResetAverage();
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool AverageMilliseconds
  {
    get => this.averageMilliseconds;
    set
    {
      if (this.averageMilliseconds == value || !Application.isPlaying)
        return;
      this.averageMilliseconds = value;
      if (!this.averageMilliseconds)
        this.LastAverageMillisecondsValue = 0.0f;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool AverageNewLine
  {
    get => this.averageNewLine;
    set
    {
      if (this.averageNewLine == value || !Application.isPlaying)
        return;
      this.averageNewLine = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public int AverageSamples
  {
    get => this.averageSamples;
    set
    {
      if (this.averageSamples == value || !Application.isPlaying)
        return;
      this.averageSamples = value;
      if (!this.enabled)
        return;
      if (this.averageSamples > 0)
      {
        if (this.accumulatedAverageSamples == null)
          this.accumulatedAverageSamples = new float[this.averageSamples];
        else if (this.accumulatedAverageSamples.Length != this.averageSamples)
          Array.Resize<float>(ref this.accumulatedAverageSamples, this.averageSamples);
      }
      else
        this.accumulatedAverageSamples = (float[]) null;
      this.ResetAverage();
      this.Refresh();
    }
  }

  public bool MinMax
  {
    get => this.minMax;
    set
    {
      if (this.minMax == value || !Application.isPlaying)
        return;
      this.minMax = value;
      if (!this.minMax)
        this.ResetMinMax();
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool MinMaxMilliseconds
  {
    get => this.minMaxMilliseconds;
    set
    {
      if (this.minMaxMilliseconds == value || !Application.isPlaying)
        return;
      this.minMaxMilliseconds = value;
      if (!this.minMaxMilliseconds)
      {
        this.LastMinMillisecondsValue = 0.0f;
        this.LastMaxMillisecondsValue = 0.0f;
      }
      else
      {
        this.LastMinMillisecondsValue = 1000f / (float) this.LastMinimumValue;
        this.LastMaxMillisecondsValue = 1000f / (float) this.LastMaximumValue;
      }
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool MinMaxNewLine
  {
    get => this.minMaxNewLine;
    set
    {
      if (this.minMaxNewLine == value || !Application.isPlaying)
        return;
      this.minMaxNewLine = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool MinMaxTwoLines
  {
    get => this.minMaxTwoLines;
    set
    {
      if (this.minMaxTwoLines == value || !Application.isPlaying)
        return;
      this.minMaxTwoLines = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool Render
  {
    get => this.render;
    set
    {
      if (this.render == value || !Application.isPlaying)
        return;
      this.render = value;
      if (!this.render)
      {
        if (!this.renderAutoAdd)
          return;
        FPSCounterData.TryToRemoveRenderRecorder();
      }
      else
      {
        this.previousFrameCount = Time.frameCount;
        if (this.renderAutoAdd)
          FPSCounterData.TryToAddRenderRecorder();
        this.Refresh();
      }
    }
  }

  public bool RenderNewLine
  {
    get => this.renderNewLine;
    set
    {
      if (this.renderNewLine == value || !Application.isPlaying)
        return;
      this.renderNewLine = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool RenderAutoAdd
  {
    get => this.renderAutoAdd;
    set
    {
      if (this.renderAutoAdd == value || !Application.isPlaying)
        return;
      this.renderAutoAdd = value;
      if (!this.enabled)
        return;
      FPSCounterData.TryToAddRenderRecorder();
      this.Refresh();
    }
  }

  public Color ColorWarning
  {
    get => this.colorWarning;
    set
    {
      if (this.colorWarning == value || !Application.isPlaying)
        return;
      this.colorWarning = value;
      if (!this.enabled)
        return;
      this.CacheWarningColor();
      this.Refresh();
    }
  }

  public Color ColorCritical
  {
    get => this.colorCritical;
    set
    {
      if (this.colorCritical == value || !Application.isPlaying)
        return;
      this.colorCritical = value;
      if (!this.enabled)
        return;
      this.CacheCriticalColor();
      this.Refresh();
    }
  }

  public Color ColorRender
  {
    get => this.colorRender;
    set
    {
      if (this.colorRender == value || !Application.isPlaying)
        return;
      this.colorRender = value;
      if (!this.enabled)
        return;
      this.CacheCurrentColor();
      this.Refresh();
    }
  }

  public int LastValue
  {
    get => this.\u003CLastValue\u003Ek__BackingField;
    set => this.\u003CLastValue\u003Ek__BackingField = value;
  }

  public float LastMillisecondsValue
  {
    get => this.\u003CLastMillisecondsValue\u003Ek__BackingField;
    set => this.\u003CLastMillisecondsValue\u003Ek__BackingField = value;
  }

  public float LastRenderValue
  {
    get => this.\u003CLastRenderValue\u003Ek__BackingField;
    set => this.\u003CLastRenderValue\u003Ek__BackingField = value;
  }

  public int LastAverageValue
  {
    get => this.\u003CLastAverageValue\u003Ek__BackingField;
    set => this.\u003CLastAverageValue\u003Ek__BackingField = value;
  }

  public float LastAverageMillisecondsValue
  {
    get => this.\u003CLastAverageMillisecondsValue\u003Ek__BackingField;
    set => this.\u003CLastAverageMillisecondsValue\u003Ek__BackingField = value;
  }

  public int LastMinimumValue
  {
    get => this.\u003CLastMinimumValue\u003Ek__BackingField;
    set => this.\u003CLastMinimumValue\u003Ek__BackingField = value;
  }

  public int LastMaximumValue
  {
    get => this.\u003CLastMaximumValue\u003Ek__BackingField;
    set => this.\u003CLastMaximumValue\u003Ek__BackingField = value;
  }

  public float LastMinMillisecondsValue
  {
    get => this.\u003CLastMinMillisecondsValue\u003Ek__BackingField;
    set => this.\u003CLastMinMillisecondsValue\u003Ek__BackingField = value;
  }

  public float LastMaxMillisecondsValue
  {
    get => this.\u003CLastMaxMillisecondsValue\u003Ek__BackingField;
    set => this.\u003CLastMaxMillisecondsValue\u003Ek__BackingField = value;
  }

  public FPSLevel CurrentFpsLevel
  {
    get => this.\u003CCurrentFpsLevel\u003Ek__BackingField;
    set => this.\u003CCurrentFpsLevel\u003Ek__BackingField = value;
  }

  public FPSCounterData()
  {
    this.color = (Color) new Color32((byte) 85, (byte) 218, (byte) 102, byte.MaxValue);
    this.colorRender = (Color) new Color32((byte) 167, (byte) 110, (byte) 209, byte.MaxValue);
    this.style = FontStyle.Bold;
    this.milliseconds = true;
    this.render = false;
    this.renderNewLine = true;
    this.average = true;
    this.averageMilliseconds = true;
    this.averageNewLine = true;
    this.resetAverageOnNewScene = true;
    this.minMax = true;
    this.minMaxNewLine = true;
    this.resetMinMaxOnNewScene = true;
  }

  public void ResetAverage()
  {
    if (!Application.isPlaying)
      return;
    this.LastAverageValue = 0;
    this.currentAverageSamples = 0;
    this.currentAverageRaw = 0.0f;
    if (this.averageSamples <= 0 || this.accumulatedAverageSamples == null)
      return;
    Array.Clear((Array) this.accumulatedAverageSamples, 0, this.accumulatedAverageSamples.Length);
  }

  public void ResetMinMax(bool withoutUpdate = false)
  {
    if (!Application.isPlaying)
      return;
    this.LastMinimumValue = -1;
    this.LastMaximumValue = -1;
    this.minMaxIntervalsSkipped = 0;
    this.UpdateValue(true);
    this.dirty = true;
  }

  public void OnLevelLoadedCallback()
  {
    if (this.minMax && this.resetMinMaxOnNewScene)
      this.ResetMinMax();
    if (this.average && this.resetAverageOnNewScene)
      this.ResetAverage();
    if (!this.render || !this.renderAutoAdd)
      return;
    FPSCounterData.TryToAddRenderRecorder();
  }

  public void AddRenderTime(float time)
  {
    if (!this.enabled || !this.inited)
      return;
    this.renderTimeBank += time;
  }

  public override void UpdateValue(bool force)
  {
    if (!this.enabled)
      return;
    int newValue = (int) this.newValue;
    if (this.LastValue != newValue | force)
    {
      this.LastValue = newValue;
      this.dirty = true;
    }
    if (this.LastValue <= this.criticalLevelValue)
    {
      if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Critical)
      {
        this.CurrentFpsLevel = FPSLevel.Critical;
        if (this.OnFPSLevelChange != null)
          this.OnFPSLevelChange(this.CurrentFpsLevel);
      }
    }
    else if (this.LastValue < this.warningLevelValue)
    {
      if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Warning)
      {
        this.CurrentFpsLevel = FPSLevel.Warning;
        if (this.OnFPSLevelChange != null)
          this.OnFPSLevelChange(this.CurrentFpsLevel);
      }
    }
    else if (this.LastValue != 0 && this.CurrentFpsLevel != FPSLevel.Normal)
    {
      this.CurrentFpsLevel = FPSLevel.Normal;
      if (this.OnFPSLevelChange != null)
        this.OnFPSLevelChange(this.CurrentFpsLevel);
    }
    if (this.dirty && this.milliseconds)
      this.LastMillisecondsValue = 1000f / this.newValue;
    if (this.render && (double) this.renderTimeBank > 0.0)
    {
      int frameCount = Time.frameCount;
      int num1 = frameCount - this.previousFrameCount;
      if (num1 == 0)
        num1 = 1;
      float num2 = this.renderTimeBank / (float) num1;
      if ((double) num2 != (double) this.LastRenderValue | force)
      {
        this.LastRenderValue = num2;
        this.dirty = true;
      }
      this.previousFrameCount = frameCount;
      this.renderTimeBank = 0.0f;
    }
    int num3 = 0;
    if (this.average)
    {
      if (this.averageSamples == 0)
      {
        ++this.currentAverageSamples;
        this.currentAverageRaw += ((float) this.LastValue - this.currentAverageRaw) / (float) this.currentAverageSamples;
      }
      else
      {
        if (this.accumulatedAverageSamples == null)
        {
          this.accumulatedAverageSamples = new float[this.averageSamples];
          this.ResetAverage();
        }
        this.accumulatedAverageSamples[this.currentAverageSamples % this.averageSamples] = (float) this.LastValue;
        ++this.currentAverageSamples;
        this.currentAverageRaw = this.GetAverageFromAccumulatedSamples();
      }
      num3 = Mathf.RoundToInt(this.currentAverageRaw);
      if (this.LastAverageValue != num3 | force)
      {
        this.LastAverageValue = num3;
        this.dirty = true;
        if (this.averageMilliseconds)
          this.LastAverageMillisecondsValue = 1000f / (float) this.LastAverageValue;
      }
    }
    if (this.minMax)
    {
      if (this.minMaxIntervalsSkipped <= this.minMaxIntervalsToSkip)
      {
        if (!force)
          ++this.minMaxIntervalsSkipped;
      }
      else if (this.LastMinimumValue == -1)
        this.dirty = true;
      if (this.minMaxIntervalsSkipped > this.minMaxIntervalsToSkip && this.dirty)
      {
        if (this.LastMinimumValue == -1)
        {
          this.LastMinimumValue = this.LastValue;
          if (this.minMaxMilliseconds)
            this.LastMinMillisecondsValue = 1000f / (float) this.LastMinimumValue;
        }
        else if (this.LastValue < this.LastMinimumValue)
        {
          this.LastMinimumValue = this.LastValue;
          if (this.minMaxMilliseconds)
            this.LastMinMillisecondsValue = 1000f / (float) this.LastMinimumValue;
        }
        if (this.LastMaximumValue == -1)
        {
          this.LastMaximumValue = this.LastValue;
          if (this.minMaxMilliseconds)
            this.LastMaxMillisecondsValue = 1000f / (float) this.LastMaximumValue;
        }
        else if (this.LastValue > this.LastMaximumValue)
        {
          this.LastMaximumValue = this.LastValue;
          if (this.minMaxMilliseconds)
            this.LastMaxMillisecondsValue = 1000f / (float) this.LastMaximumValue;
        }
      }
    }
    if (!this.dirty || this.main.OperationMode != OperationMode.Normal)
      return;
    string str1 = this.LastValue < this.warningLevelValue ? (this.LastValue > this.criticalLevelValue ? this.colorWarningCached : this.colorCriticalCached) : this.colorCached;
    this.text.Length = 0;
    this.text.Append(str1).Append(this.LastValue).Append("</color>");
    float num4;
    if (this.milliseconds)
    {
      StringBuilder stringBuilder = this.text.Append(this.LastValue < this.warningLevelValue ? (this.LastValue > this.criticalLevelValue ? this.colorWarningCachedMs : this.colorCriticalCachedMs) : this.colorCachedMs);
      num4 = this.LastMillisecondsValue;
      string str2 = num4.ToString("F");
      stringBuilder.Append(str2).Append(" MS]</color>");
    }
    if (this.average)
    {
      this.text.Append(this.averageNewLine ? '\n' : ' ');
      this.text.Append(num3 < this.warningLevelValue ? (num3 > this.criticalLevelValue ? this.colorWarningCachedAvg : this.colorCriticalCachedAvg) : this.colorCachedAvg).Append(num3);
      if (this.averageMilliseconds)
      {
        StringBuilder stringBuilder = this.text.Append(" [");
        num4 = this.LastAverageMillisecondsValue;
        string str3 = num4.ToString("F");
        stringBuilder.Append(str3).Append(" MS]");
      }
      this.text.Append("</color>");
    }
    if (this.minMax)
    {
      this.text.Append(this.minMaxNewLine ? '\n' : ' ');
      this.text.Append(this.LastMinimumValue < this.warningLevelValue ? (this.LastMinimumValue > this.criticalLevelValue ? this.colorWarningCachedMin : this.colorCriticalCachedMin) : this.colorCachedMin).Append(this.LastMinimumValue);
      if (this.minMaxMilliseconds)
      {
        StringBuilder stringBuilder = this.text.Append(" [");
        num4 = this.LastMinMillisecondsValue;
        string str4 = num4.ToString("F");
        stringBuilder.Append(str4).Append(" MS]");
      }
      this.text.Append("</color>");
      this.text.Append(this.minMaxTwoLines ? '\n' : ' ');
      this.text.Append(this.LastMaximumValue < this.warningLevelValue ? (this.LastMaximumValue > this.criticalLevelValue ? this.colorWarningCachedMax : this.colorCriticalCachedMax) : this.colorCachedMax).Append(this.LastMaximumValue);
      if (this.minMaxMilliseconds)
      {
        StringBuilder stringBuilder = this.text.Append(" [");
        num4 = this.LastMaxMillisecondsValue;
        string str5 = num4.ToString("F");
        stringBuilder.Append(str5).Append(" MS]");
      }
      this.text.Append("</color>");
    }
    if (this.render)
    {
      StringBuilder stringBuilder = this.text.Append(this.renderNewLine ? '\n' : ' ').Append(this.colorCachedRender);
      num4 = this.LastRenderValue;
      string str6 = num4.ToString("F");
      stringBuilder.Append(str6).Append(" MS").Append("</color>");
    }
    this.ApplyTextStyles();
  }

  public override void PerformActivationActions()
  {
    base.PerformActivationActions();
    this.LastValue = 0;
    this.LastMinimumValue = -1;
    if (this.render)
    {
      this.previousFrameCount = Time.frameCount;
      if (this.renderAutoAdd)
        FPSCounterData.TryToAddRenderRecorder();
    }
    if (this.main.OperationMode != OperationMode.Normal)
      return;
    if (this.colorWarningCached == null)
      this.CacheWarningColor();
    if (this.colorCriticalCached == null)
      this.CacheCriticalColor();
    this.text.Append(this.colorCriticalCached).Append("0").Append("</color>");
    this.ApplyTextStyles();
    this.dirty = true;
  }

  public override void PerformDeActivationActions()
  {
    base.PerformDeActivationActions();
    this.ResetMinMax(true);
    this.ResetAverage();
    this.LastValue = 0;
    this.CurrentFpsLevel = FPSLevel.Normal;
  }

  public override IEnumerator UpdateCounter()
  {
    FPSCounterData fpsCounterData = this;
    while (true)
    {
      float previousUpdateTime = Time.unscaledTime;
      int previousUpdateFrames = Time.frameCount;
      fpsCounterData.cachedWaitForSecondsUnscaled.Reset();
      yield return (object) fpsCounterData.cachedWaitForSecondsUnscaled;
      float num1 = Time.unscaledTime - previousUpdateTime;
      int num2 = Time.frameCount - previousUpdateFrames;
      fpsCounterData.newValue = (float) num2 / num1;
      fpsCounterData.UpdateValue(false);
      fpsCounterData.main.UpdateTexts();
    }
  }

  public override bool HasData() => true;

  public override void CacheCurrentColor()
  {
    string hex = AFPSCounter.Color32ToHex((Color32) this.color);
    this.colorCached = $"<color=#{hex}>FPS: ";
    this.colorCachedMs = $" <color=#{hex}>[";
    this.colorCachedMin = $"<color=#{hex}>MIN: ";
    this.colorCachedMax = $"<color=#{hex}>MAX: ";
    this.colorCachedAvg = $"<color=#{hex}>AVG: ";
    this.colorCachedRender = $"<color=#{AFPSCounter.Color32ToHex((Color32) this.colorRender)}>REN: ";
  }

  public void CacheWarningColor()
  {
    string hex = AFPSCounter.Color32ToHex((Color32) this.colorWarning);
    this.colorWarningCached = $"<color=#{hex}>FPS: ";
    this.colorWarningCachedMs = $" <color=#{hex}>[";
    this.colorWarningCachedMin = $"<color=#{hex}>MIN: ";
    this.colorWarningCachedMax = $"<color=#{hex}>MAX: ";
    this.colorWarningCachedAvg = $"<color=#{hex}>AVG: ";
  }

  public void CacheCriticalColor()
  {
    string hex = AFPSCounter.Color32ToHex((Color32) this.colorCritical);
    this.colorCriticalCached = $"<color=#{hex}>FPS: ";
    this.colorCriticalCachedMs = $" <color=#{hex}>[";
    this.colorCriticalCachedMin = $"<color=#{hex}>MIN: ";
    this.colorCriticalCachedMax = $"<color=#{hex}>MAX: ";
    this.colorCriticalCachedAvg = $"<color=#{hex}>AVG: ";
  }

  public float GetAverageFromAccumulatedSamples()
  {
    float num = 0.0f;
    for (int index = 0; index < this.averageSamples; ++index)
      num += this.accumulatedAverageSamples[index];
    return this.currentAverageSamples >= this.averageSamples ? num / (float) this.averageSamples : num / (float) this.currentAverageSamples;
  }

  public static void TryToAddRenderRecorder()
  {
    Camera main = Camera.main;
    if ((UnityEngine.Object) main == (UnityEngine.Object) null || !((UnityEngine.Object) main.GetComponent<AFPSRenderRecorder>() == (UnityEngine.Object) null))
      return;
    main.gameObject.AddComponent<AFPSRenderRecorder>();
  }

  public static void TryToRemoveRenderRecorder()
  {
    Camera main = Camera.main;
    if ((UnityEngine.Object) main == (UnityEngine.Object) null)
      return;
    AFPSRenderRecorder component = main.GetComponent<AFPSRenderRecorder>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) component);
  }
}
