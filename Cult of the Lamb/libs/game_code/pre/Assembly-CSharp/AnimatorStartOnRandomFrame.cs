// Decompiled with JetBrains decompiler
// Type: AnimatorStartOnRandomFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnimatorStartOnRandomFrame : BaseMonoBehaviour
{
  private Animator animator;

  private void Start()
  {
    this.animator = this.GetComponent<Animator>();
    this.animator.Play(this.currentAnimationName(), 0, Random.value);
    this.animator.cullingMode = AnimatorCullingMode.CullCompletely;
  }

  private string currentAnimationName()
  {
    string str = "";
    foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
    {
      if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationClip.name))
        str = animationClip.name.ToString();
    }
    return str;
  }
}
