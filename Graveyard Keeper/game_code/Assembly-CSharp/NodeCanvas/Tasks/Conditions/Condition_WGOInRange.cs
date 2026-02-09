// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_WGOInRange
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("WGO In Range", 0)]
[Category("Movement")]
public class Condition_WGOInRange : WGOBehaviourCondition
{
  public BBParameter<float> range = new BBParameter<float>(1f);
  public BBParameter<WorldGameObject> target_wgo = new BBParameter<WorldGameObject>();

  public override string info => "WGO in range " + this.range?.ToString();

  public override bool OnCheck()
  {
    return !((Object) this.target_wgo.value == (Object) null) && this.self_wgo.IsInRange(this.target_wgo.value, this.range.value);
  }
}
