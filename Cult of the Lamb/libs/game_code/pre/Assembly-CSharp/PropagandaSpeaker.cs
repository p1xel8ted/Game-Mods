// Decompiled with JetBrains decompiler
// Type: PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PropagandaSpeaker : Interaction_AddFuel
{
  public SkeletonAnimation Spine;
  public SpriteRenderer RangeSprite;
  private static List<PropagandaSpeaker> PropagandaSpeakers = new List<PropagandaSpeaker>();
  private LayerMask playerMask;
  private Collider2D[] colliders;
  private ContactFilter2D filter;
  private Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  private float DistanceRadius = 1f;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private bool distanceChanged;
  private Vector3 _updatePos;
  private EventInstance loopedInstance;
  private bool VOPlaying;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.fuelKey = "propaganda_fuel";
    PropagandaSpeaker.PropagandaSpeakers.Add(this);
    if (!((Object) this.GetComponentInParent<PlacementObject>() == (Object) null))
      return;
    this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    PropagandaSpeaker.PropagandaSpeakers.Remove(this);
  }

  private void Start()
  {
    this.RangeSprite.size = (Vector2) (Vector3.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE);
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
    this.filter = new ContactFilter2D();
  }

  protected override void Update()
  {
    base.Update();
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (Object) PlayerFarming.Instance == (Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
    }
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.RangeSprite.gameObject.SetActive(true);
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else if (this.distanceChanged)
    {
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
    this.FollowersInRange();
    if (this.Structure.Structure_Info != null && this.Structure.Structure_Info.FullyFueled && this.Spine.AnimationState.GetCurrent(0).Animation.Name != "on")
    {
      this.Spine.AnimationState.SetAnimation(0, "on", true);
      if (this.VOPlaying)
        return;
      this.loopedInstance = AudioManager.Instance.CreateLoop("event:/player/lamb_megaphone", this.gameObject, true);
      this.VOPlaying = true;
    }
    else
    {
      if (this.Structure.Structure_Info == null || this.Structure.Structure_Info.FullyFueled || !(this.Spine.AnimationState.GetCurrent(0).Animation.Name != "off"))
        return;
      this.Spine.AnimationState.SetAnimation(0, "off", true);
      if (!this.VOPlaying)
        return;
      AudioManager.Instance.StopLoop(this.loopedInstance);
      this.VOPlaying = false;
    }
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    if (!this.VOPlaying)
      return;
    AudioManager.Instance.StopLoop(this.loopedInstance);
    this.VOPlaying = false;
  }

  private bool FollowersInRange()
  {
    if (this.Structure.Brain.Data.Fuel > 0)
    {
      BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
      if ((Object) boxCollider2D == (Object) null)
      {
        boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
      }
      boxCollider2D.size = Vector2.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE;
      boxCollider2D.transform.position = this.Structure.Brain.Data.Position;
      boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && boxCollider2D.OverlapPoint((Vector2) follower.transform.position))
          return true;
      }
    }
    return false;
  }
}
