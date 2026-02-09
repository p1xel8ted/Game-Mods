// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MousePickEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Mouse Pick", 0)]
[Category("Events/Input")]
[Description("Called when any collider is clicked with the specified mouse button. PickInfo contains the information of the raycast event")]
public class MousePickEvent : EventNode, IUpdatable
{
  public BBParameter<MousePickEvent.ButtonKeys> buttonKey;
  public BBParameter<LayerMask> mask = new BBParameter<LayerMask>((LayerMask) -1);
  public FlowOutput o;
  public RaycastHit hit;

  public override string name => $"{base.name} [{this.buttonKey}]";

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Object Picked");
    this.AddValueOutput<RaycastHit>("Pick Info", (ValueHandler<RaycastHit>) (() => this.hit));
  }

  public void Update()
  {
    if (!Input.GetMouseButtonDown((int) this.buttonKey.value) || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity, (int) this.mask.value))
      return;
    this.o.Call(new Flow());
  }

  [CompilerGenerated]
  public RaycastHit \u003CRegisterPorts\u003Eb__7_0() => this.hit;

  public enum ButtonKeys
  {
    Left,
    Right,
    Middle,
  }
}
