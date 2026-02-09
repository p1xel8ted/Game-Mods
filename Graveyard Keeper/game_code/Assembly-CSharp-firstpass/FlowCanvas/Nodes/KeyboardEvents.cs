// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.KeyboardEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Input")]
[Name("Keyboard Key", 0)]
[Description("Calls respective outputs when the defined keyboard key is pressed down, held down or released")]
public class KeyboardEvents : EventNode, IUpdatable
{
  public BBParameter<KeyCode> keyCode = (BBParameter<KeyCode>) KeyCode.Space;
  public FlowOutput down;
  public FlowOutput up;
  public FlowOutput pressed;

  public override string name => $"{base.name} [{this.keyCode}]";

  public override void RegisterPorts()
  {
    this.down = this.AddFlowOutput("Down");
    this.pressed = this.AddFlowOutput("Pressed");
    this.up = this.AddFlowOutput("Up");
  }

  public void Update()
  {
    int key = (int) this.keyCode.value;
    if (Input.GetKeyDown((KeyCode) key))
      this.down.Call(new Flow());
    if (Input.GetKey((KeyCode) key))
      this.pressed.Call(new Flow());
    if (!Input.GetKeyUp((KeyCode) key))
      return;
    this.up.Call(new Flow());
  }
}
