// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.WaitMousePick2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Input")]
public class WaitMousePick2D : ActionTask
{
  public WaitMousePick2D.ButtonKeys buttonKey;
  public LayerMask mask = (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<GameObject> saveObjectAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePositionAs;
  public int buttonID;
  public RaycastHit2D hit;

  public override string info
  {
    get => $"Wait Object '{this.buttonKey}' Click. Save As {this.saveObjectAs}";
  }

  public override void OnUpdate()
  {
    this.buttonID = (int) this.buttonKey;
    if (!Input.GetMouseButtonDown(this.buttonID))
      return;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    this.hit = Physics2D.Raycast((Vector2) ray.origin, (Vector2) ray.direction, float.PositiveInfinity, (int) this.mask);
    if (!((Object) this.hit.collider != (Object) null))
      return;
    this.savePositionAs.value = (Vector3) this.hit.point;
    this.saveObjectAs.value = this.hit.collider.gameObject;
    this.saveDistanceAs.value = this.hit.distance;
    this.EndAction(true);
  }

  public enum ButtonKeys
  {
    Left,
    Right,
    Middle,
  }
}
