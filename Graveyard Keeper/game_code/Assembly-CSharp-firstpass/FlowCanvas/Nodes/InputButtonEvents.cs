// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.InputButtonEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Input Button", 0)]
[Description("Calls respective outputs when the defined Button is pressed down, held down or released.\nButtons are configured in Unity Input Manager.")]
[Category("Events/Input")]
public class InputButtonEvents : EventNode, IUpdatable
{
  [RequiredField]
  public BBParameter<string> buttonName = (BBParameter<string>) "Fire1";
  public FlowOutput down;
  public FlowOutput up;
  public FlowOutput pressed;

  public override string name => $"{base.name} [{this.buttonName}]";

  public override void RegisterPorts()
  {
    this.down = this.AddFlowOutput("Down");
    this.pressed = this.AddFlowOutput("Pressed");
    this.up = this.AddFlowOutput("Up");
  }

  public void Update()
  {
    string buttonName = this.buttonName.value;
    if (Input.GetButtonDown(buttonName))
      this.down.Call(new Flow());
    if (Input.GetButton(buttonName))
      this.pressed.Call(new Flow());
    if (!Input.GetButtonUp(buttonName))
      return;
    this.up.Call(new Flow());
  }
}
