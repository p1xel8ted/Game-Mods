// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.CountersData.DeviceInfoCounterData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using CodeStage.AdvancedFPSCounter.Labels;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.CountersData;

[AddComponentMenu("")]
[Serializable]
public class DeviceInfoCounterData : BaseCounterData
{
  [SerializeField]
  [Tooltip("Shows operating system & platform info.")]
  public bool platform = true;
  [SerializeField]
  [Tooltip("CPU model and cores (including virtual cores from Intel's Hyper Threading) count.")]
  public bool cpuModel = true;
  [SerializeField]
  [Tooltip("Shows GPU model name.")]
  public bool gpuModel = true;
  [Tooltip("Shows graphics API version and type (if possible).")]
  [SerializeField]
  public bool gpuApi = true;
  [SerializeField]
  [Tooltip("Shows graphics supported shader model (if possible), approximate pixel fill-rate (if possible) and total Video RAM size (if possible).")]
  public bool gpuSpec = true;
  [SerializeField]
  [Tooltip("Shows total RAM size.")]
  public bool ramSize = true;
  [SerializeField]
  [Tooltip("Shows screen resolution, size and DPI (if possible).")]
  public bool screenData = true;
  [SerializeField]
  [Tooltip("Shows device model. Actual for mobile devices.")]
  public bool deviceModel;
  [CompilerGenerated]
  public string \u003CLastValue\u003Ek__BackingField;

  public bool Platform
  {
    get => this.platform;
    set
    {
      if (this.platform == value || !Application.isPlaying)
        return;
      this.platform = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool CpuModel
  {
    get => this.cpuModel;
    set
    {
      if (this.cpuModel == value || !Application.isPlaying)
        return;
      this.cpuModel = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool GpuModel
  {
    get => this.gpuModel;
    set
    {
      if (this.gpuModel == value || !Application.isPlaying)
        return;
      this.gpuModel = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool GpuApi
  {
    get => this.gpuApi;
    set
    {
      if (this.gpuApi == value || !Application.isPlaying)
        return;
      this.gpuApi = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool GpuSpec
  {
    get => this.gpuSpec;
    set
    {
      if (this.gpuSpec == value || !Application.isPlaying)
        return;
      this.gpuSpec = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool RamSize
  {
    get => this.ramSize;
    set
    {
      if (this.ramSize == value || !Application.isPlaying)
        return;
      this.ramSize = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool ScreenData
  {
    get => this.screenData;
    set
    {
      if (this.screenData == value || !Application.isPlaying)
        return;
      this.screenData = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public bool DeviceModel
  {
    get => this.deviceModel;
    set
    {
      if (this.deviceModel == value || !Application.isPlaying)
        return;
      this.deviceModel = value;
      if (!this.enabled)
        return;
      this.Refresh();
    }
  }

  public string LastValue
  {
    get => this.\u003CLastValue\u003Ek__BackingField;
    set => this.\u003CLastValue\u003Ek__BackingField = value;
  }

  public DeviceInfoCounterData()
  {
    this.color = (Color) new Color32((byte) 172, (byte) 172, (byte) 172, byte.MaxValue);
    this.anchor = LabelAnchor.LowerLeft;
  }

  public override void UpdateValue(bool force)
  {
    if (!this.inited && this.HasData())
      this.Activate();
    else if (this.inited && !this.HasData())
    {
      this.Deactivate();
    }
    else
    {
      if (!this.enabled)
        return;
      bool flag = false;
      if (this.text == null)
        this.text = new StringBuilder(500);
      else
        this.text.Length = 0;
      if (this.platform)
      {
        this.text.Append("OS: ").Append(SystemInfo.operatingSystem).Append(" [").Append((object) Application.platform).Append("]");
        flag = true;
      }
      if (this.cpuModel)
      {
        if (flag)
          this.text.Append('\n');
        this.text.Append("CPU: ").Append(SystemInfo.processorType).Append(" [").Append(SystemInfo.processorCount).Append(" cores]");
        flag = true;
      }
      if (this.gpuModel)
      {
        if (flag)
          this.text.Append('\n');
        this.text.Append("GPU: ").Append(SystemInfo.graphicsDeviceName);
        flag = true;
      }
      if (this.gpuApi)
      {
        if (flag)
          this.text.Append('\n');
        this.text.Append("GPU: ").Append(SystemInfo.graphicsDeviceVersion);
        this.text.Append(" [").Append((object) SystemInfo.graphicsDeviceType).Append("]");
        flag = true;
      }
      if (this.gpuSpec)
      {
        if (flag)
          this.text.Append('\n');
        this.text.Append("GPU: SM: ");
        int graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
        if (graphicsShaderLevel >= 10 && graphicsShaderLevel <= 99)
        {
          int num;
          this.text.Append(num = graphicsShaderLevel / 10).Append('.').Append(num / 10);
        }
        else
          this.text.Append("N/A");
        this.text.Append(", VRAM: ");
        int graphicsMemorySize = SystemInfo.graphicsMemorySize;
        if (graphicsMemorySize > 0)
          this.text.Append(graphicsMemorySize).Append(" MB");
        else
          this.text.Append("N/A");
        flag = true;
      }
      if (this.ramSize)
      {
        if (flag)
          this.text.Append('\n');
        int systemMemorySize = SystemInfo.systemMemorySize;
        if (systemMemorySize > 0)
        {
          this.text.Append("RAM: ").Append(systemMemorySize).Append(" MB");
          flag = true;
        }
        else
          flag = false;
      }
      if (this.screenData)
      {
        if (flag)
          this.text.Append('\n');
        Resolution currentResolution = Screen.currentResolution;
        this.text.Append("SCR: ").Append(currentResolution.width).Append("x").Append(currentResolution.height).Append("@").Append(currentResolution.refreshRate).Append("Hz [window size: ").Append(Screen.width).Append("x").Append(Screen.height);
        float dpi = Screen.dpi;
        if ((double) dpi > 0.0)
          this.text.Append(", DPI: ").Append(dpi).Append("]");
        else
          this.text.Append("]");
        flag = true;
      }
      if (this.deviceModel)
      {
        if (flag)
          this.text.Append('\n');
        this.text.Append("Model: ").Append(SystemInfo.deviceModel);
      }
      this.LastValue = this.text.ToString();
      if (this.main.OperationMode == OperationMode.Normal)
      {
        this.text.Insert(0, this.colorCached);
        this.text.Append("</color>");
        this.ApplyTextStyles();
      }
      else
        this.text.Length = 0;
      this.dirty = true;
    }
  }

  public override bool HasData()
  {
    return this.cpuModel || this.gpuModel || this.ramSize || this.screenData;
  }

  public override void CacheCurrentColor()
  {
    this.colorCached = $"<color=#{AFPSCounter.Color32ToHex((Color32) this.color)}>";
  }
}
