// Decompiled with JetBrains decompiler
// Type: ScenarioData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Map;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Scenario Data")]
public class ScenarioData : ScriptableObject
{
  public DungeonSandboxManager.ScenarioType ScenarioType;
  public int ScenarioIndex;
  public DungeonSandboxManager.ScenarioModifier ScenarioModifiers;
  public MapConfig MapConfig;
  [CompilerGenerated]
  public int \u003CFleeceType\u003Ek__BackingField;

  public int FleeceType
  {
    get => this.\u003CFleeceType\u003Ek__BackingField;
    set => this.\u003CFleeceType\u003Ek__BackingField = value;
  }
}
