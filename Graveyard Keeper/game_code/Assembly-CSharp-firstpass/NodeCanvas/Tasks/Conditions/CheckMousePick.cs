// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckMousePick
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Input")]
public class CheckMousePick : ConditionTask
{
  public ButtonKeys buttonKey;
  [LayerField]
  public int layer;
  [BlackboardOnly]
  public BBParameter<GameObject> saveGoAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePosAs;
  public RaycastHit hit;

  public override string info
  {
    get
    {
      string info = this.buttonKey.ToString() + " Click";
      if (!string.IsNullOrEmpty(this.savePosAs.name))
        info += $"\n<i>(SavePos As {this.savePosAs})</i>";
      if (!string.IsNullOrEmpty(this.saveGoAs.name))
        info += $"\n<i>(SaveGo As {this.saveGoAs})</i>";
      return info;
    }
  }

  public override bool OnCheck()
  {
    if (!Input.GetMouseButtonDown((int) this.buttonKey) || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity, 1 << this.layer))
      return false;
    this.saveGoAs.value = this.hit.collider.gameObject;
    this.saveDistanceAs.value = this.hit.distance;
    this.savePosAs.value = this.hit.point;
    return true;
  }
}
