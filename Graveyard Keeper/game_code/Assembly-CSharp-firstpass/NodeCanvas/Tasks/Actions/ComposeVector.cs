// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ComposeVector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard")]
[Description("Create a new Vector out of 3 floats and save it to the blackboard")]
public class ComposeVector : ActionTask
{
  public BBParameter<float> x;
  public BBParameter<float> y;
  public BBParameter<float> z;
  [BlackboardOnly]
  public BBParameter<Vector3> saveAs;

  public override string info => "New Vector as " + this.saveAs?.ToString();

  public override void OnExecute()
  {
    this.saveAs.value = new Vector3(this.x.value, this.y.value, this.z.value);
    this.EndAction();
  }
}
