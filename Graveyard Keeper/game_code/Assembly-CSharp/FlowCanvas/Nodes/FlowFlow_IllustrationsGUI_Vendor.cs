// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.FlowFlow_IllustrationsGUI_Vendor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Illustrations GUI", 0)]
public class FlowFlow_IllustrationsGUI_Vendor : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> in_do_open = this.AddValueInput<bool>("do open");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_do_open.value)
      {
        if (GUIElements.me.illustrations_gui.is_open)
          Debug.LogError((object) "Can not open IllustrationsGUI: window is already open");
        else
          GUIElements.me.illustrations_gui.Open(true);
      }
      else if (GUIElements.me.illustrations_gui.is_open)
        GUIElements.me.illustrations_gui.Hide();
      else
        Debug.LogError((object) "Can not close IllustrationsGUI: window is already closed");
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("do open").value ? "Open Illustrations GUI" : "Close Illustrations GUI";
    }
  }
}
