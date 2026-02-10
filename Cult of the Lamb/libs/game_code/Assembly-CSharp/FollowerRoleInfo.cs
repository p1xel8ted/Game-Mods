// Decompiled with JetBrains decompiler
// Type: FollowerRoleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
