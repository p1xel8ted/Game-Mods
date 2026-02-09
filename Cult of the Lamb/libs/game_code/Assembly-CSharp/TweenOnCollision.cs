// Decompiled with JetBrains decompiler
// Type: TweenOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using MMTools;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TweenOnCollision : MonoBehaviour
{
  public bool Activated;
  public float ActivatedTimer = 3f;
  public float Progress;
  public UnitObject player;
  public Collider2D PlayerCollision;
  [SerializeField]
  public bool pushPlayer;
  [SerializeField]
  public TweenOnCollision.tweenType _tweenType;
  [SerializeField]
  public float pushIntensity = 0.1f;
  [EventRef]
  public string SFXtoPlay;
  public GameObject p;
  public float ActivateDistance = 0.666f;
  public bool UseCollider;
  public bool UsePlayerPrisoner = true;
  public UnityEvent Callback;
  public float Distance;
  public bool foundPlayer;

  public void PushPlayer()
  {
    AudioManager.Instance.PlayOneShot(this.SFXtoPlay, this.gameObject);
    if (this.UsePlayerPrisoner)
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    this.transform.DOKill();
    switch (this._tweenType)
    {
      case TweenOnCollision.tweenType.shakeScale:
        this.transform.DOKill();
        this.transform.DOShakeScale(0.5f, new Vector3(0.2f, 0.1f, 0.1f)).SetEase<Tweener>(Ease.OutQuad);
        break;
    }
    if ((bool) (UnityEngine.Object) this.player && this.pushPlayer)
      this.player.DoKnockBack(Utils.GetAngle(this.gameObject.transform.position, this.player.gameObject.transform.position) * ((float) Math.PI / 180f), this.pushIntensity, 0.33f);
    this.Callback?.Invoke();
  }

  public void Start() => this.FindPlayer();

  public void FindPlayer()
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

  public void Update()
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!this.UseCollider || !collision.gameObject.CompareTag("Player") || !this.enabled)
      return;
    this.Progress = 0.0f;
    this.Activated = true;
    this.PushPlayer();
  }

  public enum tweenType
  {
    none,
    shakeScale,
  }
}
