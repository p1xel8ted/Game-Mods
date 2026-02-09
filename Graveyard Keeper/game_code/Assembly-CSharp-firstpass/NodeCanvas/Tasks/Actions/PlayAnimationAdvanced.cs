// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.PlayAnimationAdvanced
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Animation")]
public class PlayAnimationAdvanced : ActionTask<Animation>
{
  [RequiredField]
  public BBParameter<AnimationClip> animationClip;
  public WrapMode animationWrap;
  public AnimationBlendMode blendMode;
  [SliderField(0, 2)]
  public float playbackSpeed = 1f;
  [SliderField(0, 1)]
  public float crossFadeTime = 0.25f;
  public PlayDirections playDirection;
  public BBParameter<string> mixTransformName;
  public BBParameter<int> animationLayer;
  public bool queueAnimation;
  public bool waitActionFinish = true;
  public string animationToPlay = string.Empty;
  public int dir = -1;
  public Transform mixTransform;

  public override string info => "Anim " + this.animationClip.ToString();

  public override string OnInit()
  {
    this.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
    this.animationClip.value.legacy = true;
    return (string) null;
  }

  public override void OnExecute()
  {
    if (this.playDirection == PlayDirections.Toggle)
      this.dir = -this.dir;
    if (this.playDirection == PlayDirections.Backward)
      this.dir = -1;
    if (this.playDirection == PlayDirections.Forward)
      this.dir = 1;
    this.agent.AddClip(this.animationClip.value, this.animationClip.value.name);
    this.animationToPlay = this.animationClip.value.name;
    if (!string.IsNullOrEmpty(this.mixTransformName.value))
    {
      this.mixTransform = this.FindTransform(this.agent.transform, this.mixTransformName.value);
      if (!(bool) (Object) this.mixTransform)
        Debug.LogWarning((object) $"Cant find transform with name '{this.mixTransformName.value}' for PlayAnimation Action");
    }
    else
      this.mixTransform = (Transform) null;
    this.animationToPlay = this.animationClip.value.name;
    if ((bool) (Object) this.mixTransform)
      this.agent[this.animationToPlay].AddMixingTransform(this.mixTransform, true);
    this.agent[this.animationToPlay].layer = this.animationLayer.value;
    this.agent[this.animationToPlay].speed = (float) this.dir * this.playbackSpeed;
    this.agent[this.animationToPlay].normalizedTime = Mathf.Clamp01((float) -this.dir);
    this.agent[this.animationToPlay].wrapMode = this.animationWrap;
    this.agent[this.animationToPlay].blendMode = this.blendMode;
    if (this.queueAnimation)
      this.agent.CrossFadeQueued(this.animationToPlay, this.crossFadeTime);
    else
      this.agent.CrossFade(this.animationToPlay, this.crossFadeTime);
    if (this.waitActionFinish)
      return;
    this.EndAction(true);
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.agent[this.animationToPlay].length / (double) this.playbackSpeed - (double) this.crossFadeTime)
      return;
    this.EndAction(true);
  }

  public Transform FindTransform(Transform parent, string name)
  {
    if (parent.name == name)
      return parent;
    foreach (Transform componentsInChild in parent.GetComponentsInChildren<Transform>())
    {
      if (componentsInChild.name == name)
        return componentsInChild;
    }
    return (Transform) null;
  }
}
