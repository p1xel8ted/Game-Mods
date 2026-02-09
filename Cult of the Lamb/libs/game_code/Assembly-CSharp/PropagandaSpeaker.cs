// Decompiled with JetBrains decompiler
// Type: PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public static List<PropagandaSpeaker> PropagandaSpeakers = new List<PropagandaSpeaker>();
  public LayerMask playerMask;
  public Collider2D[] colliders;
  public ContactFilter2D filter;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public float DistanceRadius = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public EventInstance loopedInstance;
  public bool VOPlaying;

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

  public void Start()
  {
    this.RangeSprite.size = (Vector2) (Vector3.one * Structures_PropagandaSpeaker.EFFECTIVE_DISTANCE);
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
    this.filter = new ContactFilter2D();
  }

  public override void Update()
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

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.Structure.Structure_Info == null || !this.Structure.Structure_Info.FullyFueled || !(this.Spine.AnimationState.GetCurrent(0).Animation.Name == "on") || this.VOPlaying)
      return;
    this.loopedInstance = AudioManager.Instance.CreateLoop("event:/player/lamb_megaphone", this.gameObject, true);
    this.VOPlaying = true;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!this.VOPlaying)
      return;
    AudioManager.Instance.StopLoop(this.loopedInstance);
    this.VOPlaying = false;
  }

  public bool FollowersInRange()
  {
    if ((Object) this.Structure != (Object) null && this.Structure.Brain != null && this.Structure.Brain.Data != null && this.Structure.Brain.Data.Fuel > 0 && !this.Structure.Brain.Data.IsCollapsed)
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
