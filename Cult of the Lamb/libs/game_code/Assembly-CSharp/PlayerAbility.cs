// Decompiled with JetBrains decompiler
// Type: PlayerAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerAbility : BaseMonoBehaviour
{
  public float TimeScaleDelay;
  public PlayerFarming playerFarming;
  public StateMachine state;
  public Health health;
  public float KeyDown;
  public bool KeyReleased;
  public GameObject trailPrefab;
  public float DelayBetweenTrails = 0.2f;
  public List<GameObject> trail = new List<GameObject>();
  public float TrailsTimer;
  public GameObject t;
  public Vector3 previousSpawnPosition;
  public System.Action onEndBurrow;
  public Coroutine cBurrowRoutine;
  public EventInstance? loopingSoundInstance;
  public string LoopSound = "event:/unlock_building/unlock_hold";
  public Coroutine cHealRoutine;
  public Coroutine cZoom;
  public EventInstance burrowLoopInstance;

  public void Start()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDie(
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
    if (this.cZoom != null)
      this.StopCoroutine(this.cZoom);
    if (!this.loopingSoundInstance.HasValue)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance.Value);
    this.loopingSoundInstance = new EventInstance?();
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraResetTargetZoom();
    if (this.cHealRoutine != null)
      this.StopCoroutine(this.cHealRoutine);
    if (this.cZoom != null)
      this.StopCoroutine(this.cZoom);
    if (!this.loopingSoundInstance.HasValue)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance.Value);
    this.loopingSoundInstance = new EventInstance?();
  }

  public void Update()
  {
    if ((double) Time.timeScale <= 0.0 || this.playerFarming.GoToAndStopping)
      return;
    int num = this.playerFarming.IsKnockedOut ? 1 : 0;
  }

  public IEnumerator DoZoom()
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

  public void DoHealRoutine()
  {
    this.cHealRoutine = this.StartCoroutine((IEnumerator) this.DoHeal());
  }

  public IEnumerator DoHeal()
  {
    PlayerAbility playerAbility = this;
    if (!playerAbility.loopingSoundInstance.HasValue)
      playerAbility.loopingSoundInstance = new EventInstance?(AudioManager.Instance.CreateLoop(playerAbility.LoopSound, true));
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
        if (!InputManager.Gameplay.GetFleeceAbilityButtonHeld(playerAbility.playerFarming))
        {
          playerAbility.playerFarming.AbilityKeyDown = false;
          break;
        }
        playerAbility.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerAbility.playerFarming.playerSpells.AmmoCost);
        int healing = Mathf.RoundToInt((float) (1 * TrinketManager.GetHealthAmountMultiplier(playerAbility.playerFarming)));
        playerAbility.health.Heal((float) healing);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        playerAbility.playerFarming.growAndFade.Play();
        BiomeConstants.Instance.EmitHeartPickUpVFX(playerAbility.playerFarming.CameraBone.transform.position, 0.0f, "red", "burst_big");
        BiomeConstants.Instance.EmitBloodImpact(playerAbility.transform.position, 0.0f, "red", "BloodImpact_Large_0");
        AudioManager.Instance.PlayOneShot("event:/player/collect_blue_heart", playerAbility.gameObject);
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerAbility.gameObject);
        yield return (object) new WaitForSeconds(1f);
      }
      playerAbility.StartCoroutine((IEnumerator) playerAbility.EndHeal());
    }
  }

  public bool CanHeal()
  {
    return this.playerFarming.playerSpells.faithAmmo.CanAfford((float) this.playerFarming.playerSpells.AmmoCost) && (double) this.health.HP + (double) this.health.SpiritHearts < (double) this.health.totalHP + (double) this.health.TotalSpiritHearts && PlayerFleeceManager.BleatToHeal() && !this.playerFarming.IsKnockedOut;
  }

  public void DoEndHealRoutine()
  {
    if (this.cHealRoutine != null)
      this.StopCoroutine(this.cHealRoutine);
    this.StartCoroutine((IEnumerator) this.EndHeal());
  }

  public IEnumerator EndHeal()
  {
    PlayerAbility playerAbility = this;
    if (playerAbility.loopingSoundInstance.HasValue)
    {
      AudioManager.Instance.StopLoop(playerAbility.loopingSoundInstance.Value);
      playerAbility.loopingSoundInstance = new EventInstance?();
    }
    if (playerAbility.cZoom != null)
      playerAbility.StopCoroutine(playerAbility.cZoom);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraResetTargetZoom();
    playerAbility.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public bool CanBurrow()
  {
    return this.playerFarming.playerSpells.faithAmmo.CanAfford((float) this.playerFarming.playerSpells.AmmoCost) && PlayerFleeceManager.BleatToBurrow() && !this.playerFarming.IsBurrowing;
  }

  public void DoBurrowRoutine()
  {
    this.cBurrowRoutine = this.StartCoroutine((IEnumerator) this.BurrowRoutine());
  }

  public IEnumerator BurrowRoutine()
  {
    PlayerAbility playerAbility = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", playerAbility.gameObject);
    playerAbility.playerFarming.playerWeapon.enabled = false;
    playerAbility.playerFarming.playerSpells.enabled = false;
    playerAbility.playerFarming.AllowDodging = false;
    playerAbility.playerFarming.playerSpells.faithAmmo.UseAmmo((float) playerAbility.playerFarming.playerSpells.AmmoCost);
    playerAbility.onEndBurrow = new System.Action(playerAbility.\u003CBurrowRoutine\u003Eb__31_0);
    playerAbility.playerFarming.CustomAnimationWithCallback("burrow-in", false, new System.Action(playerAbility.\u003CBurrowRoutine\u003Eb__31_1));
    while (playerAbility.playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = 4f;
    playerAbility.playerFarming.playerController.MakeInvincible(Duration);
    while ((double) (Progress += Time.deltaTime * playerAbility.playerFarming.Spine.timeScale) < (double) Duration)
    {
      if (playerAbility.playerFarming.GoToAndStopping || playerAbility.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle && playerAbility.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving || MMConversation.isPlaying)
      {
        System.Action onEndBurrow = playerAbility.onEndBurrow;
        if (onEndBurrow == null)
          yield break;
        onEndBurrow();
        yield break;
      }
      playerAbility.SpawnTrails();
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", playerAbility.gameObject);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(playerAbility.transform.position);
    playerAbility.playerFarming.CustomAnimationWithCallback("burrow-out", false, new System.Action(playerAbility.\u003CBurrowRoutine\u003Eb__31_2));
  }

  public void GiveSkin(PlayerFarming playerFarming)
  {
    if (DataManager.GetFollowerSkinUnlocked("Wombat"))
      return;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(playerFarming.Spine.transform.position, playerFarming.Spine.transform.position, 2f, "Wombat", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
    DataManager.SetFollowerSkinUnlocked("Wombat");
  }

  public void SpawnTrails()
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) this.DelayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousSpawnPosition) <= 0.10000000149011612)
      return;
    this.TrailsTimer = 0.0f;
    this.t = (GameObject) null;
    if (this.trail.Count > 0)
    {
      foreach (GameObject gameObject in this.trail)
      {
        if (!gameObject.activeSelf)
        {
          this.t = gameObject;
          this.t.transform.position = this.transform.position;
          this.t.SetActive(true);
          this.t.GetComponentInChildren<SimpleSpineSkineRandomiser>().Start();
          break;
        }
      }
    }
    if (!((UnityEngine.Object) this.t == (UnityEngine.Object) null))
      return;
    this.t = UnityEngine.Object.Instantiate<GameObject>(this.trailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
    this.trail.Add(this.t);
    this.previousSpawnPosition = this.t.transform.position;
  }

  [CompilerGenerated]
  public void \u003CBurrowRoutine\u003Eb__31_0()
  {
    this.playerFarming.playerWeapon.enabled = true;
    this.playerFarming.playerSpells.enabled = true;
    this.playerFarming.AllowDodging = true;
    this.onEndBurrow = (System.Action) null;
    this.playerFarming.playerController.ResetSpecificMovementAnimations();
    this.playerFarming.IsBurrowing = false;
    this.GiveSkin(this.playerFarming);
    AudioManager.Instance.StopLoop(this.burrowLoopInstance);
  }

  [CompilerGenerated]
  public void \u003CBurrowRoutine\u003Eb__31_1()
  {
    this.SpawnTrails();
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    this.playerFarming.IsBurrowing = true;
    string str = "burrowed";
    this.playerFarming.playerController.SetSpecificMovementAnimations(str, str, str, str, str, str);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    AudioManager.Instance.StopLoop(this.burrowLoopInstance);
    this.burrowLoopInstance = AudioManager.Instance.CreateLoop("event:/dlc/player/fleece_ratau_burrow_loop", true, (bool) (UnityEngine.Object) this.playerFarming.gameObject);
  }

  [CompilerGenerated]
  public void \u003CBurrowRoutine\u003Eb__31_2()
  {
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action onEndBurrow = this.onEndBurrow;
    if (onEndBurrow == null)
      return;
    onEndBurrow();
  }
}
