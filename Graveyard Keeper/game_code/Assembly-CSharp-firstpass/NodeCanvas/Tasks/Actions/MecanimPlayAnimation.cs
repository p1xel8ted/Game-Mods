// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MecanimPlayAnimation
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Play Animation", 0)]
[Category("Animator")]
public class MecanimPlayAnimation : ActionTask<Animator>
{
  public BBParameter<int> layerIndex;
  [RequiredField]
  public BBParameter<string> stateName;
  [SliderField(0, 1)]
  public float transitTime = 0.25f;
  public bool waitUntilFinish;
  public AnimatorStateInfo stateInfo;
  public bool played;

  public override string info => $"Anim '{this.stateName.ToString()}'";

  public override void OnExecute()
  {
    if (string.IsNullOrEmpty(this.stateName.value))
    {
      this.EndAction();
    }
    else
    {
      this.played = false;
      this.agent.CrossFade(this.stateName.value, this.transitTime / this.agent.GetCurrentAnimatorStateInfo(this.layerIndex.value).length, this.layerIndex.value);
    }
  }

  public override void OnUpdate()
  {
    this.stateInfo = this.agent.GetCurrentAnimatorStateInfo(this.layerIndex.value);
    if (this.waitUntilFinish)
    {
      if (this.stateInfo.IsName(this.stateName.value))
      {
        this.played = true;
        if ((double) this.elapsedTime < (double) this.stateInfo.length / (double) this.agent.speed)
          return;
        this.EndAction();
      }
      else
      {
        if (!this.played)
          return;
        this.EndAction();
      }
    }
    else
    {
      if ((double) this.elapsedTime < (double) this.transitTime)
        return;
      this.EndAction();
    }
  }
}
