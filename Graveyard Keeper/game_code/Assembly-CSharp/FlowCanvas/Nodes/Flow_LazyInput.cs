// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LazyInput
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Input")]
[Name("Lazy Input", 0)]
public class Flow_LazyInput : EventNode, IUpdatable
{
  public BBParameter<GameKey> keyCode = (BBParameter<GameKey>) GameKey.Down;
  public FlowOutput down;
  public FlowOutput pressed;

  public override string name => $"{base.name} [{this.keyCode}]";

  public override void RegisterPorts()
  {
    this.down = this.AddFlowOutput("Down");
    this.pressed = this.AddFlowOutput("Pressed");
  }

  public void Update()
  {
    int key = (int) this.keyCode.value;
    if (LazyInput.GetKeyDown((GameKey) key))
      this.down.Call(new Flow());
    if (!LazyInput.GetKey((GameKey) key))
      return;
    this.pressed.Call(new Flow());
  }
}
