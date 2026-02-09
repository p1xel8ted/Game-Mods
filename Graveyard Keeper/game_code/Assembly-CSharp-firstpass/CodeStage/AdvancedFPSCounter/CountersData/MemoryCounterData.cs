// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.CountersData.MemoryCounterData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.CountersData;

[AddComponentMenu("")]
[Serializable]
public class MemoryCounterData : UpdatebleCounterData
{
  public const long MEMORY_DIVIDER = 1048576 /*0x100000*/;
  public const string TEXT_START = "<color=#{0}>";
  public const string LINE_START_TOTAL = "MEM TOTAL: ";
  public const string LINE_START_ALLOCATED = "MEM ALLOC: ";
  public const string LINE_START_MONO = "MEM MONO: ";
  public const string LINE_END = " MB";
  public const string TEXT_END = "</color>";
  [Tooltip("Allows to output memory usage more precisely thus using a bit more system resources.")]
  [SerializeField]
  public bool precise = true;
  [SerializeField]
  [Tooltip("Allows to see private memory amount reserved for application. This memory can’t be used by other applications.")]
  public bool total = true;
  [SerializeField]
  [Tooltip("Allows to see amount of memory, currently allocated by application.")]
  public bool allocated = true;
  [Tooltip("Allows to see amount of memory, allocated by managed Mono objects, such as UnityEngine.Object and everything derived from it for example.")]
  [SerializeField]
  public bool monoUsage;
  [CompilerGenerated]
  public long \u003CLastTotalValue\u003Ek__BackingField;
  [CompilerGenerated]
  public long \u003CLastAllocatedValue\u003Ek__BackingField;
  [CompilerGenerated]
  public long \u003CLastMonoValue\u003Ek__BackingField;

  public bool Precise
  {
    get => this.precise;
    set
    {
      if (this.precise == value || !Application.isPlaying)
        return;
      this.precise = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool Total
  {
    get => this.total;
    set
    {
      if (this.total == value || !Application.isPlaying)
        return;
      this.total = value;
      if (!this.total)
        this.LastTotalValue = 0L;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool Allocated
  {
    get => this.allocated;
    set
    {
      if (this.allocated == value || !Application.isPlaying)
        return;
      this.allocated = value;
      if (!this.allocated)
        this.LastAllocatedValue = 0L;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool MonoUsage
  {
    get => this.monoUsage;
    set
    {
      if (this.monoUsage == value || !Application.isPlaying)
        return;
      this.monoUsage = value;
      if (!this.monoUsage)
        this.LastMonoValue = 0L;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public long LastTotalValue
  {
    get => this.\u003CLastTotalValue\u003Ek__BackingField;
    set => this.\u003CLastTotalValue\u003Ek__BackingField = value;
  }

  public long LastAllocatedValue
  {
    get => this.\u003CLastAllocatedValue\u003Ek__BackingField;
    set => this.\u003CLastAllocatedValue\u003Ek__BackingField = value;
  }

  public long LastMonoValue
  {
    get => this.\u003CLastMonoValue\u003Ek__BackingField;
    set => this.\u003CLastMonoValue\u003Ek__BackingField = value;
  }

  public MemoryCounterData()
  {
    this.color = (Color) new Color32((byte) 234, (byte) 238, (byte) 101, byte.MaxValue);
    this.style = FontStyle.Bold;
  }

  public override void UpdateValue(bool force)
  {
    if (!this.enabled)
      return;
    if (force)
    {
      if (!this.inited && this.HasData())
      {
        this.Activate();
        return;
      }
      if (this.inited && !this.HasData())
      {
        this.Deactivate();
        return;
      }
    }
    if (this.total)
    {
      long reservedMemoryLong = Profiler.GetTotalReservedMemoryLong();
      long num = 0;
      bool flag;
      if (this.precise)
      {
        flag = this.LastTotalValue != reservedMemoryLong;
      }
      else
      {
        num = reservedMemoryLong / 1048576L /*0x100000*/;
        flag = this.LastTotalValue != num;
      }
      if (flag | force)
      {
        this.LastTotalValue = this.precise ? reservedMemoryLong : num;
        this.dirty = true;
      }
    }
    if (this.allocated)
    {
      long allocatedMemoryLong = Profiler.GetTotalAllocatedMemoryLong();
      long num = 0;
      bool flag;
      if (this.precise)
      {
        flag = this.LastAllocatedValue != allocatedMemoryLong;
      }
      else
      {
        num = allocatedMemoryLong / 1048576L /*0x100000*/;
        flag = this.LastAllocatedValue != num;
      }
      if (flag | force)
      {
        this.LastAllocatedValue = this.precise ? allocatedMemoryLong : num;
        this.dirty = true;
      }
    }
    if (this.monoUsage)
    {
      long totalMemory = GC.GetTotalMemory(false);
      long num = 0;
      bool flag;
      if (this.precise)
      {
        flag = this.LastMonoValue != totalMemory;
      }
      else
      {
        num = totalMemory / 1048576L /*0x100000*/;
        flag = this.LastMonoValue != num;
      }
      if (flag | force)
      {
        this.LastMonoValue = this.precise ? totalMemory : num;
        this.dirty = true;
      }
    }
    if (!this.dirty || this.main.OperationMode != OperationMode.Normal)
      return;
    bool flag1 = false;
    this.text.Length = 0;
    this.text.Append(this.colorCached);
    float num1;
    if (this.total)
    {
      this.text.Append("MEM TOTAL: ");
      if (this.precise)
      {
        StringBuilder text = this.text;
        num1 = (float) this.LastTotalValue / 1048576f;
        string str = num1.ToString("F");
        text.Append(str);
      }
      else
        this.text.Append(this.LastTotalValue);
      this.text.Append(" MB");
      flag1 = true;
    }
    if (this.allocated)
    {
      if (flag1)
        this.text.Append('\n');
      this.text.Append("MEM ALLOC: ");
      if (this.precise)
      {
        StringBuilder text = this.text;
        num1 = (float) this.LastAllocatedValue / 1048576f;
        string str = num1.ToString("F");
        text.Append(str);
      }
      else
        this.text.Append(this.LastAllocatedValue);
      this.text.Append(" MB");
      flag1 = true;
    }
    if (this.monoUsage)
    {
      if (flag1)
        this.text.Append('\n');
      this.text.Append("MEM MONO: ");
      if (this.precise)
      {
        StringBuilder text = this.text;
        num1 = (float) this.LastMonoValue / 1048576f;
        string str = num1.ToString("F");
        text.Append(str);
      }
      else
        this.text.Append(this.LastMonoValue);
      this.text.Append(" MB");
    }
    this.text.Append("</color>");
    this.ApplyTextStyles();
  }

  public override void PerformActivationActions()
  {
    base.PerformActivationActions();
    if (!this.HasData())
      return;
    this.LastTotalValue = 0L;
    this.LastAllocatedValue = 0L;
    this.LastMonoValue = 0L;
    if (this.main.OperationMode != OperationMode.Normal)
      return;
    if (this.colorCached == null)
      this.colorCached = $"<color=#{AFPSCounter.Color32ToHex((Color32) this.color)}>";
    this.text.Append(this.colorCached);
    if (this.total)
    {
      if (this.precise)
        this.text.Append("MEM TOTAL: ").Append("0.00").Append(" MB");
      else
        this.text.Append("MEM TOTAL: ").Append(0).Append(" MB");
    }
    if (this.allocated)
    {
      if (this.text.Length > 0)
        this.text.Append('\n');
      if (this.precise)
        this.text.Append("MEM ALLOC: ").Append("0.00").Append(" MB");
      else
        this.text.Append("MEM ALLOC: ").Append(0).Append(" MB");
    }
    if (this.monoUsage)
    {
      if (this.text.Length > 0)
        this.text.Append('\n');
      if (this.precise)
        this.text.Append("MEM MONO: ").Append("0.00").Append(" MB");
      else
        this.text.Append("MEM MONO: ").Append(0).Append(" MB");
    }
    this.text.Append("</color>");
    this.ApplyTextStyles();
    this.dirty = true;
  }

  public override void PerformDeActivationActions()
  {
    base.PerformDeActivationActions();
    if (this.text != null)
      this.text.Length = 0;
    this.main.MakeDrawableLabelDirty(this.anchor);
  }

  public override IEnumerator UpdateCounter()
  {
    MemoryCounterData memoryCounterData = this;
    while (true)
    {
      memoryCounterData.UpdateValue();
      memoryCounterData.main.UpdateTexts();
      memoryCounterData.cachedWaitForSecondsUnscaled.Reset();
      yield return (object) memoryCounterData.cachedWaitForSecondsUnscaled;
    }
  }

  public override bool HasData() => this.total || this.allocated || this.monoUsage;

  public override void CacheCurrentColor()
  {
    this.colorCached = $"<color=#{AFPSCounter.Color32ToHex((Color32) this.color)}>";
  }
}
