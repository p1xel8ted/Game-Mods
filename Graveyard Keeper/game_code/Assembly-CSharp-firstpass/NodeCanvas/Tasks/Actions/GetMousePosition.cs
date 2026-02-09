// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetMousePosition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Input")]
public class GetMousePosition : ActionTask
{
  [BlackboardOnly]
  public BBParameter<Vector3> saveAs;
  public bool repeat;

  public override void OnExecute() => this.Do();

  public override void OnUpdate() => this.Do();

  public void Do()
  {
    this.saveAs.value = Input.mousePosition;
    if (this.repeat)
      return;
    this.EndAction();
  }
}
