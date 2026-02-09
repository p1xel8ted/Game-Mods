// Decompiled with JetBrains decompiler
// Type: ResolutionConfig
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResolutionConfig
{
  public static List<ResolutionConfig> _available_resolutions = new List<ResolutionConfig>();
  public int x;
  public int y;
  public int pixel_size = 2;
  public float large_gui_scale = 1f;

  public ResolutionConfig(int w, int h)
  {
    this.x = w;
    this.y = h;
  }

  public static ResolutionConfig GetResolutionConfigOrNull(int width, int height)
  {
    ResolutionConfig resolutionConfigOrNull = new ResolutionConfig(width, height);
    if (height < 900 || width < 1280 /*0x0500*/)
    {
      resolutionConfigOrNull.large_gui_scale = (float) height / 900f;
      return resolutionConfigOrNull;
    }
    return height <= 1440 && width <= 2560 /*0x0A00*/ ? resolutionConfigOrNull : (ResolutionConfig) null;
  }

  public bool IsHardwareSupported()
  {
    foreach (ResolutionConfig availableResolution in ResolutionConfig._available_resolutions)
    {
      if (availableResolution.x == this.x && availableResolution.y == this.y)
        return true;
    }
    Debug.LogWarning((object) $"Resolution {this.x}x{this.y} is not supported.");
    return false;
  }

  public static void InitResolutions()
  {
    Debug.Log((object) nameof (InitResolutions));
    Resolution[] resolutions = Screen.resolutions;
    ResolutionConfig._available_resolutions.Clear();
    foreach (Resolution resolution in resolutions)
    {
      ResolutionConfig resolutionConfigOrNull = ResolutionConfig.GetResolutionConfigOrNull(resolution.width, resolution.height);
      if (resolutionConfigOrNull == null)
      {
        Debug.Log((object) $"Skipping: {resolution.width}x{resolution.height}");
      }
      else
      {
        bool flag = false;
        foreach (ResolutionConfig availableResolution in ResolutionConfig._available_resolutions)
        {
          if (availableResolution.x == resolutionConfigOrNull.x && availableResolution.y == resolutionConfigOrNull.y)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          ResolutionConfig._available_resolutions.Add(resolutionConfigOrNull);
          Debug.Log((object) $"Available: {resolutionConfigOrNull.x}x{resolutionConfigOrNull.y}, scale = {resolutionConfigOrNull.pixel_size}");
        }
      }
    }
    if (ResolutionConfig._available_resolutions.Count != 0)
      return;
    Debug.LogError((object) "No available resolutions were found!");
  }

  public override string ToString()
  {
    return $"ResolutionConfig [{this.x}x{this.y}, pixel_size={this.pixel_size}]";
  }

  public string GetResolutionName()
  {
    string resolutionName = $"{this.x}x{this.y}";
    if ((double) this.large_gui_scale < 0.99)
      resolutionName = $"[ff5050]{resolutionName}[/c]";
    return resolutionName;
  }

  public static string[] GetResolutionsStringArray()
  {
    List<string> stringList = new List<string>();
    foreach (ResolutionConfig availableResolution in ResolutionConfig._available_resolutions)
      stringList.Add(availableResolution.GetResolutionName());
    return stringList.ToArray();
  }

  public static ResolutionConfig GetResolutionByIndex(int i)
  {
    return ResolutionConfig._available_resolutions[i];
  }
}
