// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_WGOsInRangeCount
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("WGOs In Range Count", 0)]
[Category("Mob")]
public class Condition_WGOsInRangeCount : WGOBehaviourCondition
{
  public BBParameter<float> range = new BBParameter<float>(1f);
  public BBParameter<int> count = new BBParameter<int>(1);
  public BBParameter<string> custom_tag = new BBParameter<string>();

  public override string info
  {
    get
    {
      return $"WGOs count in\nrange {this.range.value.ToString()} less than {this.count.value.ToString()}";
    }
  }

  public override bool OnCheck()
  {
    if (string.IsNullOrEmpty(this.custom_tag.value))
      return true;
    if (this.count.value <= 0)
      return false;
    if ((double) this.range.value < 0.0)
      return true;
    List<WorldGameObject> objectsByCustomTag = WorldMap.GetWorldGameObjectsByCustomTag(this.custom_tag.value);
    if (objectsByCustomTag == null || objectsByCustomTag.Count < this.count.value)
      return true;
    int num = 0;
    foreach (WorldGameObject worldGameObject in objectsByCustomTag)
    {
      if (!((Object) worldGameObject == (Object) null) && worldGameObject.IsInRange(this.self_wgo, this.range.value))
        ++num;
    }
    return num < this.count.value;
  }
}
