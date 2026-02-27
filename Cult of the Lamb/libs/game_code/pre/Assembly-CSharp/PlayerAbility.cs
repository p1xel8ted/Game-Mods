// Decompiled with JetBrains decompiler
// Type: PlayerAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class PlayerAbility : BaseMonoBehaviour
{
  private float TimeScaleDelay;
  private PlayerFarming playerFarming;
  private StateMachine state;
  private Health health;
  private float KeyDown;
  private bool KeyReleased;
  private Coroutine cHealRoutine;
  private Coroutine cZoom;

  private void Start()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraResetTargetZoom();
    if (this.cHealRoutine != null)
      this.StopCoroutine(this.cHealRoutine);
    if (this.cZoom == null)
      return;
    this.StopCoroutine(this.cZoom);
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraResetTargetZoom();
    if (this.cHealRoutine != null)
      this.StopCoroutine(this.cHealRoutine);
    if (this.cZoom == null)
      return;
    this.StopCoroutine(this.cZoom);
  }

  private void Update()
  {
    if ((double) Time.timeScale < 1.0)
      this.TimeScaleDelay = 0.3f;
    if ((double) (this.TimeScaleDelay -= Time.deltaTime) > 0.0 || (double) Time.timeScale <= 0.0 || this.playerFarming.GoToAndStopping)
      return;
    if (InputManager.Gameplay.GetBleatButtonHeld())
      this.KeyReleased = false;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        if (InputManager.Gameplay.GetBleatButtonHeld() && !this.KeyReleased && (double) (this.KeyDown += Time.deltaTime) > 0.20000000298023224 && TrinketManager.HasTrinket(TarotCards.Card.HoldToHeal))
        {
          if (this.CanHeal())
          {
            this.cHealRoutine = this.StartCoroutine((IEnumerator) this.DoHeal());
            break;
          }
          FaithAmmo.Flash();
          break;
        }
        break;
      case StateMachine.State.Heal:
        if (!InputManager.Gameplay.GetBleatButtonHeld())
        {
          this.StopCoroutine(this.cHealRoutine);
          this.StartCoroutine((IEnumerator) this.EndHeal());
          break;
        }
        break;
    }
    if (InputManager.Gameplay.GetBleatButtonHeld())
      return;
    this.KeyDown = 0.0f;
    this.KeyReleased = true;
  }

  private IEnumerator DoZoom()
  {
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    float Progress = 0.0f;
    float Duration = 1f;
    float StartZoom = GameManager.GetInstance().CamFollowTarget.targetDistance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetTargetZoom(Mathf.Lerp(StartZoom, 8f, Mathf.SmoothStep(0.0f, 1f, Progress / Duration)));
      yield return (object) null;
    }
  }

  private IEnumerator DoHeal()
  {
    PlayerAbility playerAbility = this;
    playerAbility.KeyReleased = false;
    playerAbility.state.CURRENT_STATE = StateMachine.State.Heal;
    playerAbility.cZoom = playerAbility.StartCoroutine((IEnumerator) playerAbility.DoZoom());
    yield return (object) new WaitForSeconds(1f);
    if (!playerAbility.CanHeal())
    {
      playerAbility.StartCoroutine((IEnumerator) playerAbility.EndHeal());
    }
    else
    {
      while (playerAbility.CanHeal())
      {
        FaithAmmo.UseAmmo(FaithAmmo.Total);
        int healing = Mathf.RoundToInt((float) (1 * TrinketManager.GetHealthAmountMultiplier()));
        playerAbility.health.Heal((float) healing);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        playerAbility.playerFarming.growAndFade.Play();
        BiomeConstants.Instance.EmitHeartPickUpVFX(playerAbility.playerFarming.CameraBone.transform.position, 0.0f, "red", "burst_big");
        BiomeConstants.Instance.EmitBloodImpact(playerAbility.transform.position, 0.0f, "red", "BloodImpact_Large_0");
        yield return (object) new WaitForSeconds(1f);
      }
      playerAbility.StartCoroutine((IEnumerator) playerAbility.EndHeal());
    }
  }

  private bool CanHeal()
  {
    return FaithAmmo.CanAfford(FaithAmmo.Total) && (double) this.health.HP + (double) this.health.SpiritHearts < (double) this.health.totalHP + (double) this.health.TotalSpiritHearts;
  }

  private IEnumerator EndHeal()
  {
    PlayerAbility playerAbility = this;
    if (playerAbility.cZoom != null)
      playerAbility.StopCoroutine(playerAbility.cZoom);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraResetTargetZoom();
    playerAbility.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
