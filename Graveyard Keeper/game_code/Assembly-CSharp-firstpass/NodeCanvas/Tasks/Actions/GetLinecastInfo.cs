// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetLinecastInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Physics")]
public class GetLinecastInfo : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public BBParameter<LayerMask> layerMask = (BBParameter<LayerMask>) (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<GameObject> saveHitGameObjectAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePointAs;
  [BlackboardOnly]
  public BBParameter<Vector3> saveNormalAs;
  public RaycastHit hit;

  public override void OnExecute()
  {
    if (Physics.Linecast(this.agent.position, this.target.value.transform.position, out this.hit, (int) this.layerMask.value))
    {
      this.saveHitGameObjectAs.value = this.hit.collider.gameObject;
      this.saveDistanceAs.value = this.hit.distance;
      this.savePointAs.value = this.hit.point;
      this.saveNormalAs.value = this.hit.normal;
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (Object) this.agent || !(bool) (Object) this.target.value)
      return;
    Gizmos.DrawLine(this.agent.position, this.target.value.transform.position);
  }
}
