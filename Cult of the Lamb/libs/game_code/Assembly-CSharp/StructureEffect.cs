// Decompiled with JetBrains decompiler
// Type: StructureEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class StructureEffect
{
  [Key(0)]
  public float TimeStarted;
  [Key(1)]
  public float DurationInGameMinutes;
  [Key(2)]
  public float CoolDownInGameMinutes;
  [Key(3)]
  public int StructureID;
  [Key(4)]
  public bool CoolingDown;
  [Key(5)]
  public StructureEffectManager.EffectType Type;

  [IgnoreMember]
  public float CoolDownProgress
  {
    get
    {
      return (TimeManager.TotalElapsedGameTime - this.TimeStarted - this.DurationInGameMinutes) / this.CoolDownInGameMinutes;
    }
  }

  public void Create(int StructureID, StructureEffectManager.EffectType Type)
  {
    this.TimeStarted = TimeManager.TotalElapsedGameTime;
    this.StructureID = StructureID;
    this.DurationInGameMinutes = 1440f;
    this.CoolDownInGameMinutes = 1440f;
    this.Type = Type;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/Structure/StructureEffectStarted", StructureEffectManager.GetLocalizedKey(Type));
  }

  public void BeginCooldown()
  {
    this.CoolingDown = true;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/Structure/StructureEffectEnded", StructureEffectManager.GetLocalizedKey(this.Type));
  }
}
