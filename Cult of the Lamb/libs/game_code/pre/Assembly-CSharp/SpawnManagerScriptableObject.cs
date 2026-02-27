// Decompiled with JetBrains decompiler
// Type: SpawnManagerScriptableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
  [Header("Moon Phase Default")]
  public Texture2D DayLutDefault;
  public Texture2D NightLutDefault;
  [Header("Moon Phase 0")]
  public Texture2D DayLut_0;
  public Texture2D NightLut_0;
  [Header("Moon Phase 1")]
  public Texture2D DayLut_1;
  public Texture2D NightLut_1;
  [Header("Moon Phase 2 - Half Moon")]
  public Texture2D DayLut_2;
  public Texture2D NightLut_2;
  [Header("Moon Phase 3 ")]
  public Texture2D DayLut_3;
  public Texture2D NightLut_3;
  [Header("Moon Phase 4")]
  public Texture2D DayLut_4;
  public Texture2D NightLut_4;
  [Header("Moon Phase 5 - Full Moon")]
  public Texture2D DayLut_5;
  public Texture2D NightLut_5;
  [Header("Moon Sprites")]
  public Sprite[] moonSprites;
  [Header("Day Colours")]
  public Gradient DayColors;
  [Header("Weather Settings")]
  public string[] weatherType;
}
