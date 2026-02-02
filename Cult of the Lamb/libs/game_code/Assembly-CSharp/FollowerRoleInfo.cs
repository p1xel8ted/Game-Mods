// Decompiled with JetBrains decompiler
// Type: FollowerRoleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;

#nullable disable
[MessagePackObject(false)]
public class FollowerRoleInfo
{
  public static string GetLocalizedName(FollowerRole FollowerRole)
  {
    return LocalizationManager.GetTranslation($"Traits/{FollowerRole}");
  }

  public static string GetLocalizedDescription(FollowerRole FollowerRole)
  {
    return LocalizationManager.GetTranslation($"Traits/{FollowerRole}/Description");
  }
}
