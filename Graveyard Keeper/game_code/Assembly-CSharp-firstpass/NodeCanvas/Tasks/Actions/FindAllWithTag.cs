// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindAllWithTag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Action will end in Failure if no objects are found")]
public class FindAllWithTag : ActionTask
{
  [RequiredField]
  [TagField]
  public string searchTag = "Untagged";
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveAs;

  public override string info => $"GetObjects '{this.searchTag}' as {this.saveAs?.ToString()}";

  public override void OnExecute()
  {
    this.saveAs.value = ((IEnumerable<GameObject>) GameObject.FindGameObjectsWithTag(this.searchTag)).ToList<GameObject>();
    this.EndAction(this.saveAs.value.Count != 0);
  }
}
