// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetListCount
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class GetListCount : ActionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<IList> targetList;
  [BlackboardOnly]
  public BBParameter<int> saveAs;

  public override string info
  {
    get => string.Format("{0} = {0}.Count", (object) this.saveAs, (object) this.targetList);
  }

  public override void OnExecute()
  {
    this.saveAs.value = this.targetList.value.Count;
    this.EndAction(true);
  }
}
