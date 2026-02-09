// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckTrigger2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Task.EventReceiver(new string[] {"OnTriggerEnter2D", "OnTriggerExit2D"})]
[Name("Check Trigger 2D", 0)]
[Category("System Events")]
public class CheckTrigger2D : ConditionTask<Collider2D>
{
  public TriggerTypes CheckType;
  public bool specifiedTagOnly;
  [TagField]
  public string objectTag = "Untagged";
  [BlackboardOnly]
  public BBParameter<GameObject> saveGameObjectAs;
  public bool stay;

  public override string info
  {
    get => this.CheckType.ToString() + (this.specifiedTagOnly ? $" '{this.objectTag}' tag" : "");
  }

  public override bool OnCheck() => this.CheckType == TriggerTypes.TriggerStay && this.stay;

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this.specifiedTagOnly && !(other.gameObject.tag == this.objectTag))
      return;
    this.stay = true;
    if (this.CheckType != TriggerTypes.TriggerEnter && this.CheckType != TriggerTypes.TriggerStay)
      return;
    this.saveGameObjectAs.value = other.gameObject;
    this.YieldReturn(true);
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    if (this.specifiedTagOnly && !(other.gameObject.tag == this.objectTag))
      return;
    this.stay = false;
    if (this.CheckType != TriggerTypes.TriggerExit)
      return;
    this.saveGameObjectAs.value = other.gameObject;
    this.YieldReturn(true);
  }
}
