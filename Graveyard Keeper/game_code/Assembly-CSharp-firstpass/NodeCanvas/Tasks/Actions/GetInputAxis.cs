// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetInputAxis
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Input")]
public class GetInputAxis : ActionTask
{
  public BBParameter<string> xAxisName = (BBParameter<string>) "Horizontal";
  public BBParameter<string> yAxisName;
  public BBParameter<string> zAxisName = (BBParameter<string>) "Vertical";
  public BBParameter<float> multiplier = (BBParameter<float>) 1f;
  [BlackboardOnly]
  public BBParameter<Vector3> saveAs;
  [BlackboardOnly]
  public BBParameter<float> saveXAs;
  [BlackboardOnly]
  public BBParameter<float> saveYAs;
  [BlackboardOnly]
  public BBParameter<float> saveZAs;
  public bool repeat;

  public override void OnExecute() => this.Do();

  public override void OnUpdate() => this.Do();

  public void Do()
  {
    float x = string.IsNullOrEmpty(this.xAxisName.value) ? 0.0f : Input.GetAxis(this.xAxisName.value);
    float y = string.IsNullOrEmpty(this.yAxisName.value) ? 0.0f : Input.GetAxis(this.yAxisName.value);
    float z = string.IsNullOrEmpty(this.zAxisName.value) ? 0.0f : Input.GetAxis(this.zAxisName.value);
    this.saveXAs.value = x * this.multiplier.value;
    this.saveYAs.value = y * this.multiplier.value;
    this.saveZAs.value = z * this.multiplier.value;
    this.saveAs.value = new Vector3(x, y, z) * this.multiplier.value;
    if (this.repeat)
      return;
    this.EndAction();
  }
}
