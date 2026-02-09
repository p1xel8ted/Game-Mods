// Decompiled with JetBrains decompiler
// Type: TavernEventDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TavernEventDefinition : BalanceBaseObject
{
  public const string SMILE_HAPPY = "(:-))";
  public const string SMILE_NORMAL = "(:-|)";
  public const string SMILE_UNHAPPY = "(:-()";
  public const string ITEM_STAR_1 = "(s1)";
  public const string ITEM_STAR_2 = "(s2)";
  public const string ITEM_STAR_3 = "(s3)";
  public const bool GOOD_EVENT_BY_MONEY = false;
  public const float ACHIEVEMENT_MONEY_ALCOFEST = 38.2f;
  public const float ACHIEVEMENT_MONEY_STAND_UP = 42.7f;
  public const float ACHIEVEMENT_MONEY_SHARMEL_SONG = 114f;
  public const float ACHIEVEMENT_MONEY_RAT_RACE = 59.5f;
  public const float ACHIEVEMENT_COEFF_ALCOFEST = 60f;
  public const float ACHIEVEMENT_COEFF_STAND_UP = 80f;
  public const float ACHIEVEMENT_COEFF_SHARMEL_SONG = 120f;
  public const float ACHIEVEMENT_COEFF_RAT_RACE = 100f;
  public const float UNHAPPY_IF_LESS = 0.65f;
  public const float HAPPY_IF_MORE = 0.85f;
  public SmartExpression visitors_count;
  public SmartExpression viewers_count;
  public SmartExpression max_food_sold;
  public List<string> idle_points_blacklist;
  public List<string> idle_points_whitelist;
  public List<string> viewers_points;
  public TavernEventDefinition.TavernEventType type;
  public float unconditional_income;

  public enum TavernEventType
  {
    Aclofest,
    Standup,
    Sharmel_song,
    RatRace,
  }
}
