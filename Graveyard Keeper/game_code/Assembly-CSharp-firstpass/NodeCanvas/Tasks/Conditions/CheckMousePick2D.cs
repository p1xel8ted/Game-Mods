// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckMousePick2D
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
public class CheckMousePick2D : ConditionTask
{
  public ButtonKeys buttonKey;
  public LayerMask mask = (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<GameObject> saveGoAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePosAs;
  public int buttonID;
  public RaycastHit2D hit;

  public override string info
  {
    get
    {
      string info = this.buttonKey.ToString() + " Click";
      if (!this.savePosAs.isNone)
        info = $"{info}\nSavePos As {this.savePosAs?.ToString()}";
      if (!this.saveGoAs.isNone)
        info = $"{info}\nSaveGo As {this.saveGoAs?.ToString()}";
      return info;
    }
  }

  public override bool OnCheck()
  {
    this.buttonID = (int) this.buttonKey;
    if (Input.GetMouseButtonDown(this.buttonID))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      this.hit = Physics2D.Raycast((Vector2) ray.origin, (Vector2) ray.direction, float.PositiveInfinity, (int) this.mask);
      if ((Object) this.hit.collider != (Object) null)
      {
        this.savePosAs.value = (Vector3) this.hit.point;
        this.saveGoAs.value = this.hit.collider.gameObject;
        this.saveDistanceAs.value = this.hit.distance;
        return true;
      }
    }
    return false;
  }
}
