// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_SpawnWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Spawn WGO", 0)]
[Category("Player")]
public class Action_SpawnWGO : WGOBehaviourAction
{
  public BBParameter<string> obj_id = new BBParameter<string>();
  public BBParameter<string> custom_tag = new BBParameter<string>();
  public BBParameter<bool> set_yourself_as_anchor = new BBParameter<bool>(true);
  public BBParameter<bool> return_true = new BBParameter<bool>(true);

  public override string info => "Spawn WGO " + this.obj_id?.ToString();

  public override void OnExecute()
  {
    if (string.IsNullOrEmpty(this.obj_id.value))
      return;
    WorldGameObject worldGameObject = GS.Spawn(this.obj_id.value, this.self_wgo.tf, this.custom_tag.value);
    if (this.set_yourself_as_anchor.value && (Object) worldGameObject != (Object) null && worldGameObject.components != null && worldGameObject.components.character.enabled && (Object) this.self_wgo != (Object) null && (Object) this.self_wgo.gameObject != (Object) null)
    {
      if (this.self_wgo.components != null && this.self_wgo.components.character.enabled && (Object) this.self_wgo.components.character.anchor_obj != (Object) null)
        worldGameObject.components.character.SetAnchor(this.self_wgo.components.character.anchor_obj);
      else
        worldGameObject.components.character.SetAnchor(this.self_wgo.gameObject);
    }
    this.EndAction(this.return_true.value);
  }
}
