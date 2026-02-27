// Decompiled with JetBrains decompiler
// Type: AnimateOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMTools;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class AnimateOnCollision : MonoBehaviour
{
  public bool Activated;
  public float ActivatedTimer = 3f;
  public float Progress;
  public SkeletonAnimation Spine;
  public string DefaultAnimation;
  public UnitObject player;
  public float PushForceMultiplier = 1f;
  public Collider2D PlayerCollision;
  [EventRef]
  public string VOtoPlay = "event:/enemy/vocals/humanoid/warning";
  public GameObject p;
  public float ActivateDistance = 0.666f;
  public bool UseCollider;
  public bool OnlyIfPlayerAttacked;
  public bool UsePlayerPrisoner = true;
  public float Force = 0.1f;
  public float Duration = 0.33f;
  [SerializeField]
  [SpineAnimation("", "", true, false)]
  public string AnimationToChangTo = "jeer";
  [SerializeField]
  [SpineAnimation("", "", true, false)]
  public string AnimationToChangToIfUp = "jeer-up";
  public UnityEvent Callback;
  public float Distance;
  public bool foundPlayer;

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
        this.player.DoKnockBack(Utils.GetAngle(this.gameObject.transform.position, this.player.gameObject.transform.position) * ((float) Math.PI / 180f), this.Force, this.Duration);
    }
    this.Callback?.Invoke();
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
      this.DefaultAnimation = this.Spine.state.GetCurrent(0).ToString();
    this.FindPlayer();
  }

  public void FindPlayer()
  {
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
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((double) Vector3.Distance(this.gameObject.transform.position, player.gameObject.transform.position) < (double) this.ActivateDistance && !this.Activated && !this.UseCollider)
        {
          this.p = player.gameObject;
          this.player = player.unitObject;
          this.foundPlayer = true;
        }
      }
    }
  }

  public void Update()
  {
    if (MMConversation.isPlaying)
      return;
    this.FindPlayer();
    if ((UnityEngine.Object) this.player == (UnityEngine.Object) null)
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!this.UseCollider || !collision.gameObject.CompareTag("Player") || !this.enabled)
      return;
    this.Progress = 0.0f;
    this.Activated = true;
    this.PushPlayer();
  }
}
