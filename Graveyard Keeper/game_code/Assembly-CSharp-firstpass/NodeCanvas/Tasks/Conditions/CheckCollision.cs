// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckCollision
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Task.EventReceiver(new string[] {"OnCollisionEnter", "OnCollisionExit"})]
[Category("System Events")]
public class CheckCollision : ConditionTask<Collider>
{
  public CollisionTypes checkType;
  public bool specifiedTagOnly;
  [TagField]
  public string objectTag = "Untagged";
  [BlackboardOnly]
  public BBParameter<GameObject> saveGameObjectAs;
  [BlackboardOnly]
  public BBParameter<Vector3> saveContactPoint;
  [BlackboardOnly]
  public BBParameter<Vector3> saveContactNormal;
  public bool stay;

  public override string info
  {
    get => this.checkType.ToString() + (this.specifiedTagOnly ? $" '{this.objectTag}' tag" : "");
  }

  public override bool OnCheck() => this.checkType == CollisionTypes.CollisionStay && this.stay;

  public void OnCollisionEnter(Collision info)
  {
    if (this.specifiedTagOnly && !(info.gameObject.tag == this.objectTag))
      return;
    this.stay = true;
    if (this.checkType != CollisionTypes.CollisionEnter && this.checkType != CollisionTypes.CollisionStay)
      return;
    this.saveGameObjectAs.value = info.gameObject;
    this.saveContactPoint.value = info.contacts[0].point;
    this.saveContactNormal.value = info.contacts[0].normal;
    this.YieldReturn(true);
  }

  public void OnCollisionExit(Collision info)
  {
    if (this.specifiedTagOnly && !(info.gameObject.tag == this.objectTag))
      return;
    this.stay = false;
    if (this.checkType != CollisionTypes.CollisionExit)
      return;
    this.saveGameObjectAs.value = info.gameObject;
    this.YieldReturn(true);
  }
}
