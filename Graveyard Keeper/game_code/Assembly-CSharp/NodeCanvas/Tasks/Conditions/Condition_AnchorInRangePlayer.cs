// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_AnchorInRangePlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Player")]
[Name("Anchor In Range Player", 0)]
public class Condition_AnchorInRangePlayer : WGOBehaviourCondition
{
  public BBParameter<float> range = new BBParameter<float>(1f);

  public override string info => "Anchor in range Player " + this.range?.ToString();

  public override bool OnCheck()
  {
    if ((Object) this.self_wgo == (Object) null)
    {
      Debug.LogError((object) "self_wgo is null");
      return false;
    }
    GameObject anchorObj = this.self_wgo.components.character.anchor_obj;
    if (!((Object) anchorObj == (Object) null))
      return this.player_wgo.IsInRange(anchorObj, this.range.value);
    Debug.LogWarning((object) "Anchor is null", (Object) this.self_wgo);
    return false;
  }
}
