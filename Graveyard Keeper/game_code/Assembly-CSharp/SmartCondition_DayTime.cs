// Decompiled with JetBrains decompiler
// Type: SmartCondition_DayTime
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class SmartCondition_DayTime : SmartCondition
{
  public bool is_night = true;

  public override bool CheckCondition()
  {
    return !this.is_night ? !TimeOfDay.me.is_night : TimeOfDay.me.is_night;
  }

  public override string GetName() => !this.is_night ? "Is Day" : "Is Night";
}
