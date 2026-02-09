// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.WaitMousePick
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Input")]
public class WaitMousePick : ActionTask
{
  public WaitMousePick.ButtonKeys buttonKey;
  public LayerMask mask = (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<GameObject> saveObjectAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePositionAs;
  public int buttonID;
  public RaycastHit hit;

  public override string info
  {
    get => $"Wait Object '{this.buttonKey}' Click. Save As {this.saveObjectAs}";
  }

  public override void OnUpdate()
  {
    this.buttonID = (int) this.buttonKey;
    if (!Input.GetMouseButtonDown(this.buttonID) || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity, (int) this.mask))
      return;
    this.savePositionAs.value = this.hit.point;
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
