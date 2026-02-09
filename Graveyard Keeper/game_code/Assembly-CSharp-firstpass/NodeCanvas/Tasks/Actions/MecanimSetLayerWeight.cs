// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimSetLayerWeight
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Layer Weight", 0)]
[Category("Animator")]
public class MecanimSetLayerWeight : ActionTask<Animator>
{
  public BBParameter<int> layerIndex;
  [SliderField(0, 1)]
  public BBParameter<float> layerWeight;
  [SliderField(0, 1)]
  public float transitTime;
  public float currentValue;

  public override string info
  {
    get => $"Set Layer {this.layerIndex?.ToString()}, weight {this.layerWeight?.ToString()}";
  }

  public override void OnExecute()
  {
    this.currentValue = this.agent.GetLayerWeight(this.layerIndex.value);
  }

  public override void OnUpdate()
  {
    this.agent.SetLayerWeight(this.layerIndex.value, Mathf.Lerp(this.currentValue, this.layerWeight.value, this.elapsedTime / this.transitTime));
    if ((double) this.elapsedTime < (double) this.transitTime)
      return;
    this.EndAction(true);
  }
}
