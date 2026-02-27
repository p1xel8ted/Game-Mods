// Decompiled with JetBrains decompiler
// Type: SimpleSpineActivateComponentAfterPlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleSpineActivateComponentAfterPlay : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Animation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnimationToQueue;
  public List<BaseMonoBehaviour> ComponentsToEnable;
  public List<GameObject> GameObjectToEnable;
  [EventRef]
  public string onStartSfx = "";
  [EventRef]
  public string onQueSfx = "";

  private void Start()
  {
    foreach (Behaviour behaviour in this.ComponentsToEnable)
      behaviour.enabled = false;
    foreach (GameObject gameObject in this.GameObjectToEnable)
      gameObject.SetActive(false);
    this.Spine.AnimationState.SetAnimation(0, this.Animation, false);
    if (this.onStartSfx != "")
      AudioManager.Instance.PlayOneShot(this.onStartSfx, this.transform.position);
    this.Spine.AnimationState.AddAnimation(0, this.AnimationToQueue, true, 0.0f);
    this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }

  private void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    if (this.onQueSfx != "")
      AudioManager.Instance.PlayOneShot(this.onQueSfx, this.transform.position);
    foreach (Behaviour behaviour in this.ComponentsToEnable)
      behaviour.enabled = true;
    foreach (GameObject gameObject in this.GameObjectToEnable)
      gameObject.SetActive(true);
  }

  private void OnDisable()
  {
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }
}
