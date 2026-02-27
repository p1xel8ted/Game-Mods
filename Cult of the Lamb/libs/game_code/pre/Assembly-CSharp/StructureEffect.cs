// Decompiled with JetBrains decompiler
// Type: StructureEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class StructureEffect
{
  public float TimeStarted;
  public float DurationInGameMinutes;
  public float CoolDownInGameMinutes;
  public int StructureID;
  public bool CoolingDown;
  public StructureEffectManager.EffectType Type;

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
