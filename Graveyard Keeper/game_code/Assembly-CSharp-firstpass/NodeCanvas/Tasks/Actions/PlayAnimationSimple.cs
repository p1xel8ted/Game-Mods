// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.PlayAnimationSimple
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Animation")]
public class PlayAnimationSimple : ActionTask<Animation>
{
  [RequiredField]
  public BBParameter<AnimationClip> animationClip;
  [SliderField(0, 1)]
  public float crossFadeTime = 0.25f;
  public WrapMode animationWrap = WrapMode.Loop;
  public bool waitActionFinish = true;
  public static Dictionary<Animation, AnimationClip> lastPlayedClips = new Dictionary<Animation, AnimationClip>();

  public override string info => "Anim " + this.animationClip.ToString();

  public override string OnInit()
  {
    this.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
    this.animationClip.value.legacy = true;
    return (string) null;
  }

  public override void OnExecute()
  {
    if (PlayAnimationSimple.lastPlayedClips.ContainsKey(this.agent) && (Object) PlayAnimationSimple.lastPlayedClips[this.agent] == (Object) this.animationClip.value)
    {
      this.EndAction(true);
    }
    else
    {
      PlayAnimationSimple.lastPlayedClips[this.agent] = this.animationClip.value;
      this.agent[this.animationClip.value.name].wrapMode = this.animationWrap;
      this.agent.CrossFade(this.animationClip.value.name, this.crossFadeTime);
      if (this.waitActionFinish)
        return;
      this.EndAction(true);
    }
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.animationClip.value.length - (double) this.crossFadeTime)
      return;
    this.EndAction(true);
  }
}
