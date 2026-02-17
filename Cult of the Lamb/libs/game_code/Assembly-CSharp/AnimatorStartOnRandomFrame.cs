// Decompiled with JetBrains decompiler
// Type: AnimatorStartOnRandomFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnimatorStartOnRandomFrame : BaseMonoBehaviour
{
  public Animator animator;

  public void Start()
  {
    this.animator = this.GetComponent<Animator>();
    this.animator.Play(this.currentAnimationName(), 0, Random.value);
    this.animator.cullingMode = AnimatorCullingMode.CullCompletely;
  }

  public string currentAnimationName()
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
