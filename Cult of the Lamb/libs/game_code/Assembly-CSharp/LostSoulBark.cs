// Decompiled with JetBrains decompiler
// Type: LostSoulBark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LostSoulBark : MonoBehaviour
{
  public SimpleBarkRepeating simpleBarkRepeating;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "Spine", true, false)]
  [SerializeField]
  public string TalkingAnimation;
  [SpineAnimation("", "Spine", true, false)]
  [SerializeField]
  public string IdleAnimation;
  public UnitObject UnitObject;
  public float MaxSpeed;
  public bool Activating;

  public void Awake()
  {
    this.simpleBarkRepeating = this.GetComponentInChildren<SimpleBarkRepeating>();
    if ((Object) this.simpleBarkRepeating != (Object) null && this.simpleBarkRepeating.Entries != null)
      this.simpleBarkRepeating.Entries.Clear();
    if ((Object) this.simpleBarkRepeating != (Object) null)
    {
      if (this.simpleBarkRepeating.Entries == null)
        this.simpleBarkRepeating.Entries = new List<ConversationEntry>();
      this.simpleBarkRepeating.Entries.Add(new ConversationEntry(this.gameObject, "Conversation_NPC/LostSoul/Bark/" + DataManager.Instance.LostSoulsBark.ToString()));
      this.simpleBarkRepeating.Entries[0].Offset = new Vector3(0.0f, 0.0f, -1f);
    }
    ++DataManager.Instance.LostSoulsBark;
    if (DataManager.Instance.LostSoulsBark > 11)
      DataManager.Instance.LostSoulsBark = 0;
    this.UnitObject = this.GetComponent<UnitObject>();
    if (!((Object) this.UnitObject != (Object) null))
      return;
    this.MaxSpeed = this.UnitObject.maxSpeed;
  }

  public void ActivateGhost()
  {
    this.Activating = true;
    this.Spine.AnimationState.SetAnimation(0, "activate", false);
  }

  public void Update()
  {
    if ((Object) this.UnitObject == (Object) null)
      return;
    if (this.Activating)
    {
      this.UnitObject.maxSpeed = 0.0f;
    }
    else
    {
      this.UnitObject.maxSpeed = this.simpleBarkRepeating.IsSpeaking ? 0.0f : this.MaxSpeed;
      if (!((Object) this.Spine != (Object) null) || this.Spine.AnimationState == null)
        return;
      if (this.simpleBarkRepeating.IsSpeaking)
      {
        this.Spine.skeleton.ScaleX = (double) this.simpleBarkRepeating.Player.transform.position.x > (double) this.transform.position.x ? 1f : -1f;
        if (!(this.Spine.AnimationState.GetCurrent(0).Animation.Name != this.TalkingAnimation))
          return;
        this.Spine.AnimationState.SetAnimation(0, this.TalkingAnimation, true);
      }
      else
      {
        this.Spine.skeleton.ScaleX = (double) this.UnitObject.state.LookAngle <= 90.0 || (double) this.UnitObject.state.LookAngle >= 270.0 ? 1f : -1f;
        if (!(this.Spine.AnimationState.GetCurrent(0).Animation.Name != this.IdleAnimation))
          return;
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
      }
    }
  }
}
