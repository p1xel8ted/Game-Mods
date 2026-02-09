// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MouseEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Mouse Button", 0)]
[Category("Events/Input")]
[Description("Called when the specified mouse button is clicked down, held or released")]
public class MouseEvents : EventNode, IUpdatable
{
  public BBParameter<MouseEvents.ButtonKeys> buttonKey;
  public FlowOutput down;
  public FlowOutput pressed;
  public FlowOutput up;

  public override string name => $"{base.name} [{this.buttonKey}]";

  public override void RegisterPorts()
  {
    this.down = this.AddFlowOutput("Down");
    this.pressed = this.AddFlowOutput("Pressed");
    this.up = this.AddFlowOutput("Up");
  }

  public void Update()
  {
    int button = (int) this.buttonKey.value;
    if (Input.GetMouseButtonDown(button))
      this.down.Call(new Flow());
    if (Input.GetMouseButton(button))
      this.pressed.Call(new Flow());
    if (!Input.GetMouseButtonUp(button))
      return;
    this.up.Call(new Flow());
  }

  public enum ButtonKeys
  {
    Left,
    Right,
    Middle,
  }
}
