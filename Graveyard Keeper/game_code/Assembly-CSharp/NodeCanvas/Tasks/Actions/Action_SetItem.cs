// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_SetItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Player")]
[Name("Set Item", 0)]
public class Action_SetItem : WGOBehaviourAction
{
  public BBParameter<ItemDefinition.ItemType> item = new BBParameter<ItemDefinition.ItemType>(ItemDefinition.ItemType.None);

  public override void OnExecute()
  {
    this.self_wgo.SetCurrentItem(this.item.value);
    this.EndAction(true);
  }
}
