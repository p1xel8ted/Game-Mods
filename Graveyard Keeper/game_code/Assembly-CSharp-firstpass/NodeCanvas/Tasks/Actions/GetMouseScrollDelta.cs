// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetMouseScrollDelta
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Input")]
public class GetMouseScrollDelta : ActionTask
{
  [BlackboardOnly]
  public BBParameter<float> saveAs;
  public bool repeat;

  public override string info => "Get Scroll Delta as " + this.saveAs?.ToString();

  public override void OnExecute() => this.Do();

  public override void OnUpdate() => this.Do();

  public void Do()
  {
    this.saveAs.value = Input.GetAxis("Mouse ScrollWheel");
    if (this.repeat)
      return;
    this.EndAction();
  }
}
