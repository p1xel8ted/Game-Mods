// Decompiled with JetBrains decompiler
// Type: SpineRandomAnimationPickerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpineRandomAnimationPickerUI : BaseMonoBehaviour
{
  public List<SpineAnimationsUI> spineAnims = new List<SpineAnimationsUI>();
  public bool randomTimeScale = true;
  public SkeletonGraphic Spine;
  public float minTimeScale = 0.8f;
  public float maxTimeScale = 1.2f;

  public void Start() => this.pickRandomAnimation();

  public void OnEnable()
  {
  }

  public void pickRandomAnimation()
  {
    for (int index1 = 0; index1 < this.spineAnims.Count; ++index1)
    {
      this.Spine = this.gameObject.GetComponent<SkeletonGraphic>();
      int index2 = Random.Range(0, this.spineAnims.Count);
      if (this.spineAnims[index2].TriggeredAnimation != null)
        this.Spine.AnimationState.SetAnimation(0, this.spineAnims[index2].TriggeredAnimation, true);
      if (this.randomTimeScale)
      {
        float num = Random.Range(0.8f, 1.2f);
        this.spineAnims[index1].Spine.timeScale = num;
      }
    }
  }
}
