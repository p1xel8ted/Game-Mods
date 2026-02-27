// Decompiled with JetBrains decompiler
// Type: AnimateOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMTools;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class AnimateOnCollision : MonoBehaviour
{
  private bool Activated;
  public float ActivatedTimer = 3f;
  private float Progress;
  public SkeletonAnimation Spine;
  private string DefaultAnimation;
  private UnitObject player;
  public float PushForceMultiplier = 1f;
  private Collider2D PlayerCollision;
  [EventRef]
  public string VOtoPlay = "event:/enemy/vocals/humanoid/warning";
  private GameObject p;
  public float ActivateDistance = 0.666f;
  public bool UseCollider;
  public bool OnlyIfPlayerAttacked;
  public bool UsePlayerPrisoner = true;
  [SerializeField]
  [SpineAnimation("", "", true, false)]
  private string AnimationToChangTo = "jeer";
  [SerializeField]
  [SpineAnimation("", "", true, false)]
  private string AnimationToChangToIfUp = "jeer-up";
  public UnityEvent Callback;
  public float Distance;
  private bool foundPlayer;

  public void PushPlayer()
  {
    AudioManager.Instance.PlayOneShot(this.VOtoPlay, this.gameObject);
    if (this.UsePlayerPrisoner)
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
    {
      if (this.DefaultAnimation == "idle-up")
        this.Spine.AnimationState.SetAnimation(0, this.AnimationToChangToIfUp, false);
      else
        this.Spine.AnimationState.SetAnimation(0, this.AnimationToChangTo, false);
      if (!string.IsNullOrEmpty(this.DefaultAnimation))
        this.Spine.AnimationState.AddAnimation(0, this.DefaultAnimation, true, 0.0f);
      if ((bool) (UnityEngine.Object) this.player)
        this.player.DoKnockBack(Utils.GetAngle(this.gameObject.transform.position, this.player.gameObject.transform.position) * ((float) Math.PI / 180f), 0.1f, 0.33f);
    }
    this.Callback?.Invoke();
  }

  private void Start()
  {
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
      this.DefaultAnimation = this.Spine.state.GetCurrent(0).ToString();
    this.FindPlayer();
  }

  private void FindPlayer()
  {
    if (!((UnityEngine.Object) this.p == (UnityEngine.Object) null))
      return;
    if (this.UsePlayerPrisoner)
    {
      if (!((UnityEngine.Object) PlayerPrisonerController.Instance != (UnityEngine.Object) null))
        return;
      this.p = PlayerPrisonerController.Instance.gameObject;
      this.player = this.p.GetComponent<UnitObject>();
      this.foundPlayer = true;
    }
    else
    {
      if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
        return;
      this.p = PlayerFarming.Instance.gameObject;
      this.player = this.p.GetComponent<UnitObject>();
      this.foundPlayer = true;
    }
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.player == (UnityEngine.Object) null)
    {
      if (this.foundPlayer)
        return;
      this.FindPlayer();
    }
    else
    {
      if (MMConversation.isPlaying)
        return;
      this.Distance = Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position);
      if ((double) Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position) < (double) this.ActivateDistance && !this.Activated && !this.UseCollider)
      {
        this.Progress = 0.0f;
        this.Activated = true;
        this.PushPlayer();
      }
      if (!this.Activated)
        return;
      if ((double) this.Progress < (double) this.ActivatedTimer)
        this.Progress += Time.deltaTime;
      else
        this.Activated = false;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!this.UseCollider || !(collision.gameObject.tag == "Player") || !this.enabled)
      return;
    this.Progress = 0.0f;
    this.Activated = true;
    this.PushPlayer();
  }
}
