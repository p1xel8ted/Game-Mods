// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_HasAnchor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("Has An Anchor", 0)]
[Category("Mob")]
public class Condition_HasAnchor : WGOBehaviourCondition
{
  public override string info => "Has an anchor";

  public override bool OnCheck()
  {
    if ((Object) this.self_wgo == (Object) null)
    {
      Debug.LogError((object) "self_wgo is null");
      return false;
    }
    return !((Object) this.self_wgo.components.character.anchor_obj == (Object) null);
  }
}
