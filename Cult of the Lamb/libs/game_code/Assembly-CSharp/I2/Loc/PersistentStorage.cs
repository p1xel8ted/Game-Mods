// Decompiled with JetBrains decompiler
// Type: I2.Loc.PersistentStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace I2.Loc;

public static class PersistentStorage
{
  public static I2CustomPersistentStorage mStorage;

  public static void SetSetting_String(string key, string value)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    PersistentStorage.mStorage.SetSetting_String(key, value);
  }

  public static string GetSetting_String(string key, string defaultValue)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
  }

  public static void DeleteSetting(string key)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    PersistentStorage.mStorage.DeleteSetting(key);
  }

  public static bool HasSetting(string key)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.HasSetting(key);
  }

  public static void ForceSaveSettings()
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    PersistentStorage.mStorage.ForceSaveSettings();
  }

  public static bool CanAccessFiles()
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.CanAccessFiles();
  }

  public static bool SaveFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    string data,
    bool logExceptions = true)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
  }

  public static string LoadFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
  }

  public static bool DeleteFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
  }

  public static bool HasFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (PersistentStorage.mStorage == null)
      PersistentStorage.mStorage = new I2CustomPersistentStorage();
    return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
  }

  public enum eFileType
  {
    Raw,
    Persistent,
    Temporal,
    Streaming,
  }
}
