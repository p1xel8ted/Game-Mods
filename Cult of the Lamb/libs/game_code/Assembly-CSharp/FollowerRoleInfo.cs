// Decompiled with JetBrains decompiler
// Type: FollowerRoleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
