// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckLOS
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("GameObject")]
[Name("Target In Line Of Sight", 0)]
[Description("Check of agent is in line of sight with target by doing a linecast and optionaly save the distance")]
public class CheckLOS : ConditionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> LOSTarget;
  public BBParameter<LayerMask> layerMask = (BBParameter<LayerMask>) (LayerMask) -1;
  public Vector3 offset;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  public RaycastHit hit;

  public override string info => "LOS with " + this.LOSTarget.ToString();

  public override bool OnCheck()
  {
    Transform transform = this.LOSTarget.value.transform;
    if (Physics.Linecast(this.agent.position + this.offset, transform.position + this.offset, out this.hit, (int) this.layerMask.value))
    {
      Collider component = transform.GetComponent<Collider>();
      if ((Object) component == (Object) null || (Object) this.hit.collider != (Object) component)
      {
        this.saveDistanceAs.value = this.hit.distance;
        return false;
      }
    }
    return true;
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (Object) this.agent || !(bool) (Object) this.LOSTarget.value)
      return;
    Gizmos.DrawLine(this.agent.position + this.offset, this.LOSTarget.value.transform.position + this.offset);
  }
}
