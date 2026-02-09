// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("System Events")]
[Task.EventReceiver(new string[] {"OnTriggerEnter", "OnTriggerExit"})]
public class CheckTrigger : ConditionTask<Collider>
{
  public TriggerTypes checkType;
  public bool specifiedTagOnly;
  [TagField]
  public string objectTag = "Untagged";
  [BlackboardOnly]
  public BBParameter<GameObject> saveGameObjectAs;
  public bool stay;

  public override string info
  {
    get => this.checkType.ToString() + (this.specifiedTagOnly ? $" '{this.objectTag}' tag" : "");
  }

  public override bool OnCheck() => this.checkType == TriggerTypes.TriggerStay && this.stay;

  public void OnTriggerEnter(Collider other)
  {
    if (this.specifiedTagOnly && !(other.gameObject.tag == this.objectTag))
      return;
    this.stay = true;
    if (this.checkType != TriggerTypes.TriggerEnter && this.checkType != TriggerTypes.TriggerStay)
      return;
    this.saveGameObjectAs.value = other.gameObject;
    this.YieldReturn(true);
  }

  public void OnTriggerExit(Collider other)
  {
    if (this.specifiedTagOnly && !(other.gameObject.tag == this.objectTag))
      return;
    this.stay = false;
    if (this.checkType != TriggerTypes.TriggerExit)
      return;
    this.saveGameObjectAs.value = other.gameObject;
    this.YieldReturn(true);
  }
}
