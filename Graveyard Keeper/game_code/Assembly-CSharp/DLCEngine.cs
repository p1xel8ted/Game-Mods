// Decompiled with JetBrains decompiler
// Type: DLCEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public static class DLCEngine
{
  public const DLCEngine.DLCVersion LAST_DLC = DLCEngine.DLCVersion.Souls;
  public const string _DLC_STORIES_CHECK_FILE_REL_PATH = "/gamedata_2.dat";
  public const string _DLC_REFUGEES_CHECK_FILE_REL_PATH = "/gamedata_3.dat";
  public const string _DLC_SOULS_CHECK_FILE_REL_PATH = "/gamedata_4.dat";

  public static bool IsDLCAvailable(DLCEngine.DLCVersion dlc_version)
  {
    switch (dlc_version)
    {
      case DLCEngine.DLCVersion.None:
        return true;
      case DLCEngine.DLCVersion.Stories:
        return DLCEngine.IsDLCStoriesAvailable();
      case DLCEngine.DLCVersion.Refugees:
        return DLCEngine.IsDLCRefugeesAvailable();
      case DLCEngine.DLCVersion.Souls:
        return DLCEngine.IsDLCSoulsAvailable();
      default:
        return true;
    }
  }

  public static int DLCAvailableCount()
  {
    int num = 0;
    for (int dlc_version = 1; dlc_version <= 4; ++dlc_version)
    {
      if (DLCEngine.IsDLCAvailable((DLCEngine.DLCVersion) dlc_version))
        ++num;
    }
    return num;
  }

  public static bool IsDLCStoriesAvailable()
  {
    return File.Exists(Application.dataPath + "/gamedata_2.dat");
  }

  public static bool IsDLCRefugeesAvailable()
  {
    string path = Application.dataPath + "/gamedata_3.dat";
    Debug.Log((object) Application.dataPath);
    return File.Exists(path);
  }

  public static bool IsDLCSoulsAvailable()
  {
    string path = Application.dataPath + "/gamedata_4.dat";
    Debug.Log((object) Application.dataPath);
    return File.Exists(path);
  }

  public enum DLCVersion
  {
    None,
    BreakingDead,
    Stories,
    Refugees,
    Souls,
  }
}
