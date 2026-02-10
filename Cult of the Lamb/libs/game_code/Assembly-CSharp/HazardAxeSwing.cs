// Decompiled with JetBrains decompiler
// Type: HazardAxeSwing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class HazardAxeSwing : BaseMonoBehaviour
{
  [SerializeField]
  public Transform swingingAxeTransform;
  [SerializeField]
  public Transform swingingAxeShadow;
  [SerializeField]
  public Transform shadowFollowTransform;
  [SerializeField]
  public float swingDist = 30f;
  [SerializeField]
  public float swingSpeed = 1f;
  [SerializeField]
  public int enemyDamage = 3;
  [SerializeField]
  public ParticleSystem movementParticles;
  [SerializeField]
  public ParticleSystem impactParticles;
  [SerializeField]
  public float axeSwingDangerousAngle = 10f;
  public Vector3 axeRotation;
  public float axeSwingTime;
  public bool axeSwingDangerous;

  public void FixedUpdate()
  {
    if (PlayerRelic.TimeFrozen)
      return;
    this.axeSwingTime += Time.deltaTime;
    if ((Object) this.swingingAxeTransform != (Object) null)
    {
      this.axeRotation = this.swingingAxeTransform.rotation.eulerAngles;
      this.axeRotation.y = Mathf.Sin(this.axeSwingTime * this.swingSpeed) * this.swingDist;
      if ((double) Mathf.Abs(this.axeRotation.y) < (double) this.axeSwingDangerousAngle)
      {
        if (!this.axeSwingDangerous)
        {
          AudioManager.Instance.PlayOneShot("event:/weapon/melee_swing_slow", AudioManager.Instance.Listener);
          this.axeSwingDangerous = true;
        }
        if ((Object) this.movementParticles != (Object) null)
          this.movementParticles.Play();
      }
      else
        this.axeSwingDangerous = false;
      this.swingingAxeTransform.rotation = Quaternion.Euler(this.axeRotation);
    }
    if (!((Object) this.swingingAxeShadow != (Object) null))
      return;
    Vector3 position = this.swingingAxeShadow.position with
    {
      x = this.shadowFollowTransform.position.x
    };
    if (!((Object) this.swingingAxeShadow != (Object) null))
      return;
    this.swingingAxeShadow.position = position;
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    if ((Object) this.swingingAxeTransform != (Object) null)
    {
      this.swingingAxeTransform.DOKill();
      this.swingingAxeTransform.DOMoveZ(this.swingingAxeTransform.position.z - 10f, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
    }
    if (!((Object) this.swingingAxeShadow != (Object) null))
      return;
    this.swingingAxeShadow.DOScale(Vector3.zero, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuart);
  }

  public void OnTriggerStay2D(Collider2D collision)
  {
    Debug.Log((object) "Collider made contact");
    if (!this.axeSwingDangerous)
      return;
    Health component = collision.GetComponent<Health>();
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collision.gameObject);
    if (!((Object) component != (Object) null) || component.team == Health.Team.PlayerTeam && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent) || component.ImmuneToTraps || component.isPlayer && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent))
      return;
    component.DealDamage(component.team == Health.Team.Team2 ? (float) this.enemyDamage : 1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 2f), AttackType: Health.AttackTypes.NoHitStop);
    if (!((Object) this.impactParticles != (Object) null))
      return;
    this.impactParticles.Play();
  }

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__15_0()
  {
    this.gameObject.SetActive(false);
  }
}
