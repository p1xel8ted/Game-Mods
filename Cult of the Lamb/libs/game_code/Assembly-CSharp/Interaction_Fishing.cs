// Decompiled with JetBrains decompiler
// Type: Interaction_Fishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using EasyCurvedLine;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Fishing : Interaction
{
  public static Interaction_Fishing Instance;
  [SerializeField]
  public Structure structure;
  public Structures_FishingSpot fishingSpot;
  [Space]
  [SerializeField]
  public Interaction_Fishing.FishType[] fishTypes;
  [SerializeField]
  public Vector2 fishSpawnAmount;
  [SerializeField]
  public Vector2 fishSpawnRandomTime;
  [SerializeField]
  public Transform fishGroup;
  [SerializeField]
  public Fishable fishPrefab;
  [Space]
  [SerializeField]
  public Transform[] fishingHooks;
  [SerializeField]
  public Transform[] fishingLines;
  [SerializeField]
  public Transform playerPosition;
  [SerializeField]
  public BoxCollider2D boundsCollider;
  [SerializeField]
  public float castingStrengthIncrement;
  [SerializeField]
  public AnimationCurve heightCurve;
  [SerializeField]
  public float height;
  [SerializeField]
  public float castDuration;
  [SerializeField]
  public AnimationCurve castDurationCurve;
  [SerializeField]
  public Vector2 minMaxCastDistance;
  [SerializeField]
  public float reelingSmooth;
  [SerializeField]
  public float reelSlerp;
  [SerializeField]
  public float reelHorizontalMoveSpeed;
  [SerializeField]
  public float reelingMaxSpeed;
  [SerializeField]
  public float reelingShoreOffset;
  [SerializeField]
  public float amountIncreaseOverTime = 0.01f;
  [SerializeField]
  public float amountDecreaseOverTime = 0.01f;
  [CompilerGenerated]
  public bool \u003CFishChasing\u003Ek__BackingField;
  public List<Fishable> fishables = new List<Fishable>();
  public bool[] isWithinDistance = new bool[2];
  public bool isFishing;
  public float[] castingStrength = new float[2];
  public float maxReelingSpeed;
  [CompilerGenerated]
  public Vector3[] \u003CHookedFishFleePosition\u003Ek__BackingField = new Vector3[2];
  public Vector3[] hookVelocity = new Vector3[2];
  public float reelingHorizontalOffset;
  public float hookReelLerpSpeed = 2f;
  [CompilerGenerated]
  public float[] \u003CReelLerped\u003Ek__BackingField = new float[2];
  public List<UIFishingOverlayController> _fishingOverlayControllerUI = new List<UIFishingOverlayController>();
  [SerializeField]
  public GameObject splashObj;
  public Fishable[] currentFish;
  public StateMachine.State[] states = new StateMachine.State[2];
  public float zPosition = 0.6f;
  [CompilerGenerated]
  public float[] \u003CReeledAmount\u003Ek__BackingField = new float[2];
  public float fishSpawnTimer;
  public Vector3 playerDirection;
  public Vector3 playerRightDirection;
  public UnityEvent CallBackCaught;
  public UnityEvent CallBackFail;
  public System.Action OnCatchFish;
  public System.Action OnFishEscaped;
  public GameObject FishingParticles;
  public EventInstance CastLoopedSound;
  public EventInstance LoopedSound;
  public const float minTimeBetweenHookedDirectionChange = 1.5f;
  public const float maxTimeBetweenHookedDirectionChange = 2.5f;
  public float hookDirectionChangeTimer;
  public float hookDirectionSpeed = 1f;
  public int hookDirection = 1;
  public const int maxFish = 20;
  public bool startedCastLoop;
  public bool changedState;
  public int r;
  public bool Activated;
  public Coroutine waitForPlayersCoroutine;
  public float ReelDistance;
  public bool startedLoop;
  public bool waiting;
  public CurvedLinePoint[] curvedLinePoint;

  public Structures_FishingSpot FishingSpot
  {
    get
    {
      if (this.fishingSpot == null)
        this.fishingSpot = this.structure.Brain as Structures_FishingSpot;
      return this.fishingSpot;
    }
  }

  public Transform[] FishingHooks => this.fishingHooks;

  public float ReelingMaxSpeed => this.reelingMaxSpeed;

  public bool FishChasing
  {
    get => this.\u003CFishChasing\u003Ek__BackingField;
    set => this.\u003CFishChasing\u003Ek__BackingField = value;
  }

  public Vector3[] HookedFishFleePosition
  {
    get => this.\u003CHookedFishFleePosition\u003Ek__BackingField;
    set => this.\u003CHookedFishFleePosition\u003Ek__BackingField = value;
  }

  public Vector3 PlayerPosition
  {
    get => this.playerPosition.position + this.playerDirection * this.reelingShoreOffset;
  }

  public float[] ReelLerped
  {
    get => this.\u003CReelLerped\u003Ek__BackingField;
    set => this.\u003CReelLerped\u003Ek__BackingField = value;
  }

  public StateMachine.State[] States => this.states;

  public event Interaction_Fishing.FishEvent OnCasted;

  public float[] ReeledAmount
  {
    get => this.\u003CReeledAmount\u003Ek__BackingField;
    set => this.\u003CReeledAmount\u003Ek__BackingField = value;
  }

  public bool RitualActive => FollowerBrainStats.IsFishing;

  public bool FishVisible
  {
    get => !SeasonsManager.Active || SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter;
  }

  public void Awake() => Interaction_Fishing.Instance = this;

  public override void OnEnable()
  {
    base.OnEnable();
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.OnPlayerLeft);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.OnPlayerLeft);
  }

  public void Start()
  {
    this.IncreaseChanceOfFishSkin();
    if (this.RitualActive)
    {
      this.fishSpawnAmount *= 2f;
      for (int index = 0; index < this.fishTypes.Length; ++index)
      {
        if (this.IsGoodFishType(this.fishTypes[index].Type))
          this.fishTypes[index].Probability *= 2.5f;
      }
      if ((UnityEngine.Object) this.FishingParticles != (UnityEngine.Object) null)
        this.FishingParticles.SetActive(true);
    }
    else if ((UnityEngine.Object) this.FishingParticles != (UnityEngine.Object) null)
      this.FishingParticles.SetActive(false);
    for (int index = 0; index < this.ReeledAmount.Length; ++index)
      this.ReeledAmount[index] = 0.5f;
    for (int index = 0; index < this.fishTypes.Length; ++index)
    {
      if (this.fishTypes[index].Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && DataManager.GetRandomLockedSkin() == "" || this.fishTypes[index].Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && DataManager.GetRandomLockedDecoration() == StructureBrain.TYPES.NONE || this.fishTypes[index].Type == InventoryItem.ITEM_TYPE.RELIC && (DataManager.Instance.PlayerFoundRelics.Contains(RelicType.FillUpFervour) || !DataManager.Instance.OnboardedRelics))
        this.fishTypes[index].Type = DataManager.Instance.RatooFishing_FISH_CRAB ? (DataManager.Instance.RatooFishing_FISH_LOBSTER ? (DataManager.Instance.RatooFishing_FISH_OCTOPUS ? (DataManager.Instance.RatooFishing_FISH_SQUID ? InventoryItem.ITEM_TYPE.FISH_SWORDFISH : InventoryItem.ITEM_TYPE.FISH_SQUID) : InventoryItem.ITEM_TYPE.FISH_OCTOPUS) : InventoryItem.ITEM_TYPE.FISH_LOBSTER) : InventoryItem.ITEM_TYPE.FISH_CRAB;
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.fishSpawnTimer = Time.time + UnityEngine.Random.Range(this.fishSpawnRandomTime.x, this.fishSpawnRandomTime.y);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      this.SpawnAlreadySpawnedFish();
    this.fishGroup.gameObject.SetActive(this.FishVisible);
    this.currentFish = new Fishable[2];
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    this.fishGroup.gameObject.SetActive(this.FishVisible);
  }

  public void OnBrainAssigned()
  {
    this.SpawnAlreadySpawnedFish();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void SpawnAlreadySpawnedFish()
  {
    int amount = Mathf.Clamp((int) UnityEngine.Random.Range(this.fishSpawnAmount.x, this.fishSpawnAmount.y + 1f) - this.FishingSpot.SpawnedFish.Count, 0, 20);
    foreach (Interaction_Fishing.FishType fishType in this.FishingSpot.SpawnedFish)
    {
      if (this.fishables.Count < 20)
        this.SpawnFish(fishType, false);
    }
    if (amount <= 0 || this.fishables.Count >= 20)
      return;
    this.SpawnFish(amount, true);
  }

  public void IncreaseChanceOfFishSkin()
  {
    if (DataManager.GetFollowerSkinUnlocked("Fish"))
      return;
    foreach (Interaction_Fishing.FishType fishType in this.fishTypes)
    {
      if (fishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
        fishType.Probability = 0.25f;
    }
  }

  public override void Update()
  {
    base.Update();
    for (int playerIndex = 0; playerIndex < PlayerFarming.players.Count; ++playerIndex)
      this.UpdatePlayer(playerIndex);
  }

  public void UpdatePlayer(int playerIndex)
  {
    PlayerFarming playerFarming = PlayerFarming.players[playerIndex];
    if (this.Activated && InputManager.Gameplay.GetCancelFishingButtonDown(playerFarming) && this._fishingOverlayControllerUI != null && (double) Time.timeScale != 0.0)
    {
      this.StopAllCoroutines();
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if ((UnityEngine.Object) this._fishingOverlayControllerUI[index] != (UnityEngine.Object) null)
          this._fishingOverlayControllerUI[index].StopAllCoroutines();
        AudioManager.Instance.StopLoop(this.LoopedSound);
        AudioManager.Instance.StopLoop(this.CastLoopedSound);
        foreach (Fishable fishable in this.fishables)
        {
          if ((UnityEngine.Object) fishable != (UnityEngine.Object) null)
            fishable.state = StateMachine.State.Idle;
        }
        this.isFishing = false;
        this.states[index] = StateMachine.State.Idle;
        GameManager.GetInstance()?.OnConversationEnd();
        this.ReeledAmount[index] = 0.5f;
        this.fishingHooks[index].gameObject.SetActive(false);
        this.fishingLines[index].gameObject.SetActive(false);
        this.isWithinDistance[index] = false;
        this.HasChanged = true;
        this.Activated = false;
        CoopManager.Instance.UnlockAddRemovePlayer();
        if ((UnityEngine.Object) this._fishingOverlayControllerUI[index] != (UnityEngine.Object) null)
        {
          UIFishingOverlayController overlayController = this._fishingOverlayControllerUI[index];
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
          {
            overlayController.Hide();
            playerFarming.indicator.Reset();
          })));
        }
      }
    }
    if ((bool) (UnityEngine.Object) playerFarming)
    {
      if ((double) Vector3.Distance(this.transform.position, playerFarming.transform.position) < (double) this.ActivateDistance)
      {
        if (!this.isWithinDistance[playerIndex] && this.states[playerIndex] != StateMachine.State.Idle)
        {
          GameManager.GetInstance().AddToCamera(this.LockPosition.gameObject);
          this.isWithinDistance[playerIndex] = true;
        }
      }
      else if (this.isWithinDistance[playerIndex])
      {
        GameManager.GetInstance().RemoveFromCamera(this.LockPosition.gameObject);
        this.isWithinDistance[playerIndex] = false;
      }
    }
    if ((double) Time.time > (double) this.fishSpawnTimer && (double) this.fishables.Count < (double) this.fishSpawnAmount.y && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
    {
      this.SpawnFish(1, true);
      this.fishSpawnTimer = Time.time + UnityEngine.Random.Range(this.fishSpawnRandomTime.x, this.fishSpawnRandomTime.y);
    }
    bool flag1 = false;
    if ((bool) (UnityEngine.Object) playerFarming)
      flag1 = SettingsManager.Settings.Accessibility.AutoFish || InputManager.Gameplay.GetInteractButtonDown(playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked;
    if (this.states[playerIndex] == StateMachine.State.Aiming & flag1)
      this.states[playerIndex] = StateMachine.State.Charging;
    if (this.states[playerIndex] == StateMachine.State.Charging)
    {
      bool flag2 = false;
      if (!SettingsManager.Settings.Accessibility.AutoFish)
      {
        flag1 = InputManager.Gameplay.GetInteractButtonHeld(playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked;
        flag2 = InputManager.Gameplay.GetInteractButtonUp(playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked;
      }
      if (flag1)
      {
        if (!this.startedCastLoop)
        {
          this.startedCastLoop = true;
          this.CastLoopedSound = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
        }
        int num = (int) this.CastLoopedSound.setParameterByName("hold_time", this.castingStrength[playerIndex]);
        this.castingStrength[playerIndex] = Mathf.Clamp(this.castingStrength[playerIndex] + this.castingStrengthIncrement * Time.deltaTime, 0.0f, 1f);
        this._fishingOverlayControllerUI[playerIndex].UpdateCastingStrength(this.castingStrength[playerIndex]);
        if (!this.changedState)
        {
          this._fishingOverlayControllerUI[playerIndex].CastingButtonDown(true);
          this.changedState = true;
        }
        if (SettingsManager.Settings.Accessibility.AutoFish)
        {
          Vector3 vector3 = playerFarming.FishingLineBone.transform.position + Vector3.down * Mathf.Lerp(this.minMaxCastDistance.x, this.minMaxCastDistance.y, this.castingStrength[playerIndex]);
          foreach (Fishable fishable in this.fishables)
          {
            if ((double) vector3.y < (double) fishable.transform.position.y)
              this.CastLine(playerIndex);
          }
        }
        if ((double) this.castingStrength[playerIndex] >= 1.0)
          this.CastLine(playerIndex);
      }
      else if (flag2)
      {
        this.CastLine(playerIndex);
        if (this.changedState)
        {
          this._fishingOverlayControllerUI[playerIndex].CastingButtonDown(false);
          this.changedState = false;
        }
      }
    }
    if (this.states[playerIndex] == StateMachine.State.Reeling)
    {
      this.maxReelingSpeed = !InputManager.Gameplay.GetInteractButtonHeld(playerFarming) ? Mathf.Clamp(this.maxReelingSpeed - this.reelSlerp * Time.deltaTime, 0.0f, this.reelingMaxSpeed) : this.reelingMaxSpeed;
      float horizontalAxis = InputManager.Gameplay.GetHorizontalAxis(playerFarming);
      if ((double) Mathf.Abs(horizontalAxis) > 0.10000000149011612)
        this.reelingHorizontalOffset = Mathf.Clamp(this.reelingHorizontalOffset + horizontalAxis * this.reelHorizontalMoveSpeed * Time.deltaTime, -1f, 1f);
      this.Reel(playerIndex);
    }
    if (this.states[playerIndex] != StateMachine.State.Attacking)
      return;
    if (this.r >= UnityEngine.Random.Range(50, 100))
    {
      if ((UnityEngine.Object) this.splashObj != (UnityEngine.Object) null)
        UnityEngine.Object.Instantiate<GameObject>(this.splashObj, this.fishingHooks[playerIndex].transform.position - Vector3.back * 0.1f, Quaternion.identity);
      AudioManager.Instance.PlayOneShot("event:/fishing/splash", this.fishingHooks[playerIndex].transform.position);
      this.r = 0;
    }
    else
      ++this.r;
    if ((UnityEngine.Object) this._fishingOverlayControllerUI[playerIndex] != (UnityEngine.Object) null)
    {
      this.ReeledAmount[playerIndex] += (this._fishingOverlayControllerUI[playerIndex].IsNeedleWithinSection() ? this.amountIncreaseOverTime : this.amountDecreaseOverTime) * Time.deltaTime;
      this._fishingOverlayControllerUI[playerIndex].UpdateReelBar(this.ReeledAmount[playerIndex]);
    }
    if ((double) this.ReeledAmount[playerIndex] <= 0.0)
    {
      this.currentFish[playerIndex].Spooked();
      this.StartCoroutine((IEnumerator) this.NoCatch(playerIndex, true));
    }
    else if ((double) this.ReeledAmount[playerIndex] >= 1.0)
      this.FishCaught(playerIndex);
    this.ReelLerped[playerIndex] = Mathf.Lerp(this.ReelLerped[playerIndex], this.ReeledAmount[playerIndex], this.hookReelLerpSpeed * Time.deltaTime);
    Vector3 vector3_1 = Vector3.Lerp(this.HookedFishFleePosition[playerIndex], playerFarming.Spine.transform.position, this.ReelLerped[playerIndex]);
    this.reelingHorizontalOffset += this.hookDirectionSpeed * (float) this.hookDirection * Time.deltaTime;
    this.reelingHorizontalOffset /= 2f;
    if ((double) Time.time > (double) this.hookDirectionChangeTimer)
    {
      this.hookDirection *= -1;
      this.hookDirectionChangeTimer = Time.time + UnityEngine.Random.Range(1.5f, 2.5f);
    }
    this.fishingHooks[playerIndex].transform.position = new Vector3(vector3_1.x + this.reelingHorizontalOffset, vector3_1.y - 1f, this.zPosition);
  }

  public override void GetLabel()
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !DataManager.Instance.WinterModeActive)
    {
      this.Label = LocalizationManager.GetTranslation("Interactions/WaterFrozen");
      this.Interactable = false;
    }
    else if (this.state.CURRENT_STATE == StateMachine.State.Idle)
    {
      if (this.fishables.Count > 0)
      {
        this.Label = ScriptLocalization.Interactions.Fish;
        if (this.Interactable)
          return;
        this.Interactable = true;
        this.HasChanged = true;
      }
      else
      {
        this.Label = ScriptLocalization.Interactions.NoFish;
        this.Interactable = false;
      }
    }
    else
    {
      this.Label = "";
      this.Interactable = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.waiting || this.waitForPlayersCoroutine != null)
      return;
    this._playerFarming = state.GetComponent<PlayerFarming>();
    foreach (UIFishingOverlayController overlayController in this._fishingOverlayControllerUI)
    {
      if ((UnityEngine.Object) overlayController != (UnityEngine.Object) null)
        overlayController.Hide(true);
    }
    this._fishingOverlayControllerUI.Clear();
    this.waitForPlayersCoroutine = this.StartCoroutine((IEnumerator) this.WaitForPlayersToArriveIE((System.Action) (() =>
    {
      for (int index = 0; index < PlayerFarming.players.Count; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        this.gameObject.SetActive(true);
        this.StartCoroutine((IEnumerator) this.BeganFishingIE(player));
      }
      this.waitForPlayersCoroutine = (Coroutine) null;
    })));
  }

  public bool IsGoodFishType(InventoryItem.ITEM_TYPE fishType)
  {
    return fishType == InventoryItem.ITEM_TYPE.FISH_SQUID || fishType == InventoryItem.ITEM_TYPE.FISH_SWORDFISH || fishType == InventoryItem.ITEM_TYPE.FISH_OCTOPUS || fishType == InventoryItem.ITEM_TYPE.FISH_LOBSTER;
  }

  public void Reel(int playerIndex)
  {
    Vector3 vector3 = Vector3.SmoothDamp(this.fishingHooks[playerIndex].transform.position, PlayerFarming.players[playerIndex].FishingLineBone.transform.position + this.playerRightDirection * this.reelingHorizontalOffset, ref this.hookVelocity[playerIndex], this.reelingSmooth, this.maxReelingSpeed);
    this.fishingHooks[playerIndex].transform.position = new Vector3(vector3.x, vector3.y, this.zPosition);
    this.ReelDistance = Vector3.Distance(this.fishingHooks[playerIndex].transform.position, PlayerFarming.players[playerIndex].transform.position);
    if ((double) Vector3.Distance(this.fishingHooks[playerIndex].transform.position, PlayerFarming.players[playerIndex].transform.position) >= 2.0999999046325684)
      return;
    this.StartCoroutine((IEnumerator) this.NoCatch(playerIndex, false));
  }

  public void CastLine(int playerIndex)
  {
    AudioManager.Instance.PlayOneShot("event:/ui/hold_activate", PlayerFarming.players[playerIndex].gameObject.transform.position);
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    this.StartCoroutine((IEnumerator) this.CastLineIE(playerIndex));
  }

  public IEnumerator CastLineIE(int playerIndex)
  {
    Interaction_Fishing interactionFishing = this;
    PlayerFarming playerFarming = PlayerFarming.players[playerIndex];
    interactionFishing.isFishing = true;
    interactionFishing.states[playerIndex] = StateMachine.State.Casting;
    while (true)
    {
      int num = 0;
      foreach (StateMachine.State state in interactionFishing.states)
      {
        if (state == StateMachine.State.Casting)
          ++num;
      }
      if (num < PlayerFarming.playersCount)
        yield return (object) null;
      else
        break;
    }
    interactionFishing.GetLabel();
    playerFarming.FishingLineBone.transform.position = new Vector3(playerFarming.transform.position.x, playerFarming.FishingLineBone.transform.position.y, playerFarming.FishingLineBone.transform.position.z);
    Vector3 fromPosition = playerFarming.FishingLineBone.transform.position;
    interactionFishing.fishingHooks[playerIndex].transform.position = playerFarming.FishingLineBone.transform.position;
    interactionFishing.fishingHooks[playerIndex].gameObject.SetActive(true);
    interactionFishing.fishingLines[playerIndex].gameObject.SetActive(true);
    float a1 = Vector3.Dot(Vector3.left, playerFarming.transform.position - interactionFishing.LockPosition.position);
    float b1 = Vector3.Dot(Vector3.right, playerFarming.transform.position - interactionFishing.LockPosition.position);
    float a2 = Vector3.Dot(Vector3.up, playerFarming.transform.position - interactionFishing.LockPosition.position);
    float b2 = Vector3.Dot(Vector3.down, playerFarming.transform.position - interactionFishing.LockPosition.position);
    float num1 = Mathf.Max(Mathf.Max(a1, b1), Mathf.Max(a2, b2));
    if ((double) num1 == (double) a1)
      interactionFishing.playerDirection = Vector3.right;
    else if ((double) num1 == (double) b1)
      interactionFishing.playerDirection = Vector3.left;
    else if ((double) num1 == (double) a2)
      interactionFishing.playerDirection = Vector3.down;
    else if ((double) num1 == (double) b2)
      interactionFishing.playerDirection = Vector3.up;
    float degree = Utils.GetAngle(Vector3.zero, interactionFishing.playerDirection) + 90f;
    interactionFishing.playerRightDirection = (Vector3) Utils.DegreeToVector2(degree);
    Vector3 hookLandPosition = fromPosition + interactionFishing.playerDirection * Mathf.Lerp(interactionFishing.minMaxCastDistance.x, interactionFishing.minMaxCastDistance.y, interactionFishing.castingStrength[playerIndex]);
    AudioManager.Instance.PlayOneShot("event:/fishing/cast_rod", playerFarming.gameObject.transform.position);
    playerFarming.Spine.AnimationState.SetAnimation(0, "Fishing/fishing-start", false);
    playerFarming.Spine.AnimationState.AddAnimation(0, "Fishing/fishing-loop", true, 0.0f);
    float t = 0.0f;
    while ((double) t < (double) interactionFishing.castDuration)
    {
      float time = t / interactionFishing.castDuration;
      Vector3 vector3 = Vector3.Lerp(fromPosition, new Vector3(hookLandPosition.x, hookLandPosition.y, interactionFishing.zPosition), interactionFishing.castDurationCurve.Evaluate(time)) with
      {
        z = interactionFishing.heightCurve.Evaluate(time) * interactionFishing.height
      };
      interactionFishing.fishingHooks[playerIndex].transform.position = vector3;
      t += Time.deltaTime;
      yield return (object) null;
    }
    if ((UnityEngine.Object) interactionFishing.splashObj != (UnityEngine.Object) null)
      UnityEngine.Object.Instantiate<GameObject>(interactionFishing.splashObj, interactionFishing.fishingHooks[playerIndex].transform.position - Vector3.back * 0.1f, Quaternion.identity);
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", interactionFishing.fishingHooks[playerIndex].transform.position);
    GameManager.GetInstance()?.RemoveFromCamera(interactionFishing.LockPosition.gameObject);
    GameManager.GetInstance().AddToCamera(interactionFishing.fishingHooks[playerIndex].gameObject, 12f);
    interactionFishing.states[playerIndex] = StateMachine.State.Reeling;
    interactionFishing._fishingOverlayControllerUI[playerIndex].SetState(StateMachine.State.Idle);
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_Fishing.FishEvent onCasted = interactionFishing.OnCasted;
    if (onCasted != null)
      onCasted();
  }

  public bool IsClosestFish(int playerIndex, Fishable fish)
  {
    Fishable fishable1 = (Fishable) null;
    foreach (Fishable fishable2 in this.fishables)
    {
      if ((UnityEngine.Object) fishable2 != (UnityEngine.Object) null && ((UnityEngine.Object) fishable1 == (UnityEngine.Object) null || (double) Vector3.Distance(fishable2.transform.position, this.fishingHooks[playerIndex].transform.position) < (double) Vector3.Distance(fishable1.transform.position, this.fishingHooks[playerIndex].transform.position)))
        fishable1 = fishable2;
    }
    return (UnityEngine.Object) fishable1 != (UnityEngine.Object) null && (UnityEngine.Object) fishable1 == (UnityEngine.Object) fish;
  }

  public void FishOn(int playerIndex, Fishable currentFish)
  {
    Debug.Log((object) "Fish on");
    if (!this.startedLoop)
    {
      this.startedLoop = true;
      this.LoopedSound = AudioManager.Instance.CreateLoop("event:/fishing/caught_something_loop", PlayerFarming.players[playerIndex].gameObject, true);
    }
    AudioManager.Instance.PlayOneShot("event:/fishing/caught_something_alert", PlayerFarming.players[playerIndex].transform.position);
    this._fishingOverlayControllerUI[playerIndex].SetState(StateMachine.State.Attacking);
    this._fishingOverlayControllerUI[playerIndex].SetReelingDifficulty(currentFish.FishType.Difficulty - 1);
    this.states[playerIndex] = StateMachine.State.Attacking;
    PlayerFarming.players[playerIndex].Spine.AnimationState.SetAnimation(0, "Fishing/fishing-reel", true);
    this.ReelLerped[playerIndex] = this.ReeledAmount[playerIndex];
    float num = Vector3.Distance(currentFish.transform.position, this.playerPosition.position);
    Vector3 normalized = (currentFish.transform.position - this.playerPosition.position).normalized;
    this.HookedFishFleePosition[playerIndex] = currentFish.transform.position + normalized * num;
    this.fishingHooks[playerIndex].gameObject.SetActive(false);
    this.currentFish[playerIndex] = currentFish;
  }

  public void FishCaught(int playerIndex)
  {
    this.StartCoroutine((IEnumerator) this.FishCaughtIE(playerIndex));
  }

  public IEnumerator FishCaughtIE(int playerIndex)
  {
    Interaction_Fishing interactionFishing = this;
    AudioManager.Instance.StopLoop(interactionFishing.LoopedSound);
    MMVibrate.StopRumble();
    interactionFishing.startedLoop = false;
    interactionFishing.states[playerIndex] = StateMachine.State.FoundItem;
    interactionFishing._fishingOverlayControllerUI[playerIndex].Hide();
    interactionFishing._fishingOverlayControllerUI[playerIndex] = (UIFishingOverlayController) null;
    interactionFishing.isFishing = false;
    while ((double) Mathf.Abs(interactionFishing.ReelLerped[playerIndex] - interactionFishing.ReeledAmount[playerIndex]) > 0.10000000149011612)
    {
      interactionFishing.ReelLerped[playerIndex] = Mathf.Lerp(interactionFishing.ReelLerped[playerIndex], interactionFishing.ReeledAmount[playerIndex], interactionFishing.hookReelLerpSpeed * Time.deltaTime);
      Vector3 vector3 = Vector3.Lerp(interactionFishing.HookedFishFleePosition[playerIndex], PlayerFarming.players[playerIndex].FishingLineBone.transform.position, interactionFishing.ReelLerped[playerIndex]);
      interactionFishing.fishingHooks[playerIndex].transform.position = new Vector3(vector3.x, vector3.y, interactionFishing.zPosition);
      yield return (object) null;
    }
    while (true)
    {
      int num = 0;
      foreach (StateMachine.State state in interactionFishing.states)
      {
        switch (state)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.FoundItem:
            ++num;
            break;
        }
      }
      if (num < PlayerFarming.playersCount)
        yield return (object) null;
      else
        break;
    }
    interactionFishing.fishingHooks[playerIndex].gameObject.SetActive(false);
    interactionFishing.currentFish[playerIndex].gameObject.SetActive(false);
    interactionFishing.fishingLines[playerIndex].gameObject.SetActive(false);
    PlayerFarming.players[playerIndex].Spine.AnimationState.SetAnimation(0, "Fishing/fishing-catch", true);
    AudioManager.Instance.PlayOneShot("event:/fishing/catch_fish", PlayerFarming.players[playerIndex].gameObject.transform.position);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.players[playerIndex].gameObject, 6f);
    interactionFishing.FishingSpot.FishCaught(interactionFishing.currentFish[playerIndex].FishType);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.players[playerIndex].TimedAction(2.4f, (System.Action) null, "reactions/react-happy");
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.players[playerIndex].transform.position);
    if (!DataManager.Instance.GetVariable(DataManager.Variables.ShoreFishFished))
    {
      if ((interactionFishing.currentFish[playerIndex].ItemType == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && DataManager.Instance.FishCaughtTotal > 1 || DataManager.Instance.FishCaughtTotal >= 4) && !DataManager.GetFollowerSkinUnlocked("Fish") && playerIndex == 0)
      {
        FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, 1, PlayerFarming.players[playerIndex].transform.position).GetComponent<FoundItemPickUp>();
        component.FollowerSkinForceSelection = true;
        component.SkinToForce = "Fish";
      }
      else if ((interactionFishing.currentFish[playerIndex].ItemType == InventoryItem.ITEM_TYPE.RELIC || DataManager.Instance.FishCaughtTotal >= 20) && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.FillUpFervour) && DataManager.Instance.OnboardedRelics && playerIndex == 0)
      {
        interactionFishing.currentFish[playerIndex].ItemType = InventoryItem.ITEM_TYPE.RELIC;
        interactionFishing.waiting = true;
        GameObject Speaker = RelicCustomTarget.Create(interactionFishing.currentFish[playerIndex].transform.position, PlayerFarming.players[playerIndex].transform.position, 1f, RelicType.FillUpFervour, new System.Action(interactionFishing.\u003CFishCaughtIE\u003Eb__116_0));
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(Speaker, 6f);
        while (interactionFishing.waiting)
          yield return (object) null;
      }
      else
      {
        InventoryItem.ITEM_TYPE type = interactionFishing.currentFish[playerIndex].ItemType;
        int quantity = interactionFishing.currentFish[playerIndex].Quantity;
        if (type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && (DataManager.GetFollowerSkinUnlocked("Fish") || playerIndex != 0))
        {
          type = InventoryItem.ITEM_TYPE.FISH_BIG;
        }
        else
        {
          switch (type)
          {
            case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION:
              quantity = 1;
              break;
            case InventoryItem.ITEM_TYPE.RELIC:
              type = InventoryItem.ITEM_TYPE.FISH_BIG;
              break;
          }
        }
        InventoryItem.Spawn(type, quantity, PlayerFarming.players[playerIndex].transform.position);
        if (type == InventoryItem.ITEM_TYPE.FISH_SMALL || type == InventoryItem.ITEM_TYPE.FISH_BLOWFISH || type == InventoryItem.ITEM_TYPE.FISH_CRAB || type == InventoryItem.ITEM_TYPE.FISH_SQUID || type == InventoryItem.ITEM_TYPE.FISH_BIG || type == InventoryItem.ITEM_TYPE.FISH_LOBSTER || type == InventoryItem.ITEM_TYPE.FISH_OCTOPUS || type == InventoryItem.ITEM_TYPE.FISH_SWORDFISH || type == InventoryItem.ITEM_TYPE.FISH)
        {
          if (!DataManager.Instance.FishCaught.Contains(interactionFishing.currentFish[playerIndex].ItemType))
            DataManager.Instance.FishCaught.Add(interactionFishing.currentFish[playerIndex].ItemType);
          interactionFishing.checkAchievements();
        }
      }
    }
    else
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_BIG, 2, PlayerFarming.players[playerIndex].transform.position + Vector3.right);
      if (!DataManager.Instance.FishCaught.Contains(InventoryItem.ITEM_TYPE.FISH_BIG))
      {
        DataManager.Instance.FishCaught.Add(InventoryItem.ITEM_TYPE.FISH_BIG);
        interactionFishing.checkAchievements();
      }
    }
    if (interactionFishing.RitualActive)
    {
      for (int index = 0; index < UnityEngine.Random.Range(1, 3); ++index)
      {
        Interaction_Fishing.FishType randomFishType = interactionFishing.GetRandomFishType();
        if (randomFishType.Type != InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && randomFishType.Type != InventoryItem.ITEM_TYPE.RELIC && randomFishType.Type != InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
          InventoryItem.Spawn(randomFishType.Type, 1, PlayerFarming.players[playerIndex].transform.position + Vector3.right);
        if (!DataManager.Instance.FishCaught.Contains(randomFishType.Type))
        {
          DataManager.Instance.FishCaught.Add(randomFishType.Type);
          interactionFishing.checkAchievements();
        }
      }
    }
    interactionFishing.CallBackCaught?.Invoke();
    yield return (object) new WaitForSeconds(1f);
    interactionFishing.ReeledAmount[playerIndex] = 0.5f;
    interactionFishing.fishables.Remove(interactionFishing.currentFish[playerIndex]);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFishing.currentFish[playerIndex]);
    bool flag = false;
    foreach (Fishable fishable in interactionFishing.currentFish)
    {
      if ((UnityEngine.Object) fishable != (UnityEngine.Object) null && fishable.ItemType == InventoryItem.ITEM_TYPE.RELIC)
        flag = true;
    }
    if (!flag || interactionFishing.currentFish[playerIndex].ItemType == InventoryItem.ITEM_TYPE.RELIC)
    {
      interactionFishing.states[playerIndex] = StateMachine.State.Idle;
      GameManager.GetInstance().OnConversationEnd();
    }
    interactionFishing.playerFarming.indicator.Reset();
    interactionFishing.isWithinDistance[playerIndex] = false;
    interactionFishing.HasChanged = true;
    interactionFishing.Activated = false;
    CoopManager.Instance.UnlockAddRemovePlayer();
    System.Action onCatchFish = interactionFishing.OnCatchFish;
    if (onCatchFish != null)
      onCatchFish();
    ++DataManager.Instance.FishCaughtTotal;
  }

  public void checkAchievements()
  {
    Debug.Log((object) ("Achievement check, collected fish types" + DataManager.Instance.FishCaught.Count.ToString()));
    if (DataManager.Instance.FishCaught.Count < 9)
      return;
    Debug.Log((object) "Achievement unlocked, collected all fish types");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FISH_ALL_TYPES"));
  }

  public IEnumerator NoCatch(int playerIndex, bool playAngryAniamtion)
  {
    Interaction_Fishing interactionFishing = this;
    AudioManager.Instance.StopLoop(interactionFishing.LoopedSound);
    interactionFishing.startedLoop = false;
    AudioManager.Instance.PlayOneShot("event:/fishing/fishing_failure", PlayerFarming.players[playerIndex].gameObject.transform.position);
    interactionFishing.CallBackFail?.Invoke();
    System.Action onFishEscaped = interactionFishing.OnFishEscaped;
    if (onFishEscaped != null)
      onFishEscaped();
    interactionFishing.isFishing = false;
    interactionFishing.ReeledAmount[playerIndex] = 0.5f;
    interactionFishing.fishingHooks[playerIndex].gameObject.SetActive(false);
    interactionFishing.fishingLines[playerIndex].gameObject.SetActive(false);
    interactionFishing._fishingOverlayControllerUI[playerIndex].Hide();
    interactionFishing._fishingOverlayControllerUI[playerIndex] = (UIFishingOverlayController) null;
    GameManager.GetInstance()?.RemoveFromCamera(interactionFishing.fishingHooks[playerIndex].gameObject);
    if ((bool) (UnityEngine.Object) PlayerFarming.players[playerIndex] & playAngryAniamtion)
    {
      PlayerFarming.players[playerIndex].Spine.AnimationState.SetAnimation(0, "reactions/react-angry", false);
      PlayerFarming.players[playerIndex].Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      GameManager instance = GameManager.GetInstance();
      if ((instance != null ? (instance.CamFollowTarget.targets.Count <= 0 ? 1 : 0) : 0) != 0)
        GameManager.GetInstance()?.AddPlayersToCamera();
      yield return (object) new WaitForSeconds(2.3f);
    }
    interactionFishing.states[playerIndex] = StateMachine.State.Idle;
    while (true)
    {
      int num = 0;
      foreach (StateMachine.State state in interactionFishing.states)
      {
        switch (state)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.FoundItem:
            ++num;
            break;
        }
      }
      if (num < PlayerFarming.playersCount)
        yield return (object) null;
      else
        break;
    }
    bool flag = true;
    foreach (StateMachine.State state in interactionFishing.states)
    {
      if (state == StateMachine.State.FoundItem)
        flag = false;
    }
    if (flag)
      GameManager.GetInstance()?.OnConversationEnd();
    else
      GameManager.GetInstance()?.AddPlayersToCamera();
    interactionFishing.isWithinDistance[playerIndex] = false;
    interactionFishing.HasChanged = true;
    interactionFishing.Activated = false;
    CoopManager.Instance.UnlockAddRemovePlayer();
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionFishing.FrameDelay(new System.Action(interactionFishing.\u003CNoCatch\u003Eb__118_0)));
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForSeconds(0.1f);
    callback();
  }

  public void SpawnFish(int amount, bool addFishSpawned)
  {
    if (!this.FishingSpot.CanSpawnFish && !this.RitualActive)
      return;
    amount = Mathf.Clamp(amount, 0, 20);
    for (int index = 0; index < amount; ++index)
    {
      Interaction_Fishing.FishType randomFishType = this.GetRandomFishType();
      if (this.fishables.Count >= 20)
        break;
      this.SpawnFish(randomFishType, addFishSpawned);
      if (randomFishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION || randomFishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
        break;
    }
  }

  public Fishable SpawnFish(Interaction_Fishing.FishType fishType, bool addFishSpawned)
  {
    Bounds bounds = this.boundsCollider.bounds;
    Fishable fishable = UnityEngine.Object.Instantiate<Fishable>(this.fishPrefab, new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), this.fishGroup.position.z), Quaternion.identity, this.fishGroup);
    fishable.FadeIn();
    fishable.Configure(fishType, this);
    this.fishables.Add(fishable);
    if (addFishSpawned)
      this.FishingSpot.AddFishSpawned(fishType);
    return fishable;
  }

  public Interaction_Fishing.FishType GetRandomFishType()
  {
    Interaction_Fishing.FishType randomFishType = (Interaction_Fishing.FishType) null;
    ((IList<Interaction_Fishing.FishType>) this.fishTypes).Shuffle<Interaction_Fishing.FishType>();
    while (randomFishType == null)
    {
      foreach (Interaction_Fishing.FishType fishType in this.fishTypes)
      {
        float num = UnityEngine.Random.Range(0.0f, 1f);
        if ((double) fishType.Probability >= (double) num)
        {
          randomFishType = fishType;
          break;
        }
      }
    }
    return randomFishType;
  }

  public IEnumerator WaitForPlayersToArriveIE(System.Action callback)
  {
    Interaction_Fishing interactionFishing = this;
    int count = 0;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      Vector3 vector3 = (Vector3) (index == 0 ? Vector2.zero : Vector2.right * 1.5f);
      PlayerFarming.players[index].GoToAndStop(interactionFishing.transform.position + vector3, interactionFishing.gameObject, GoToCallback: (System.Action) (() => ++count));
    }
    while (count < PlayerFarming.players.Count)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator BeganFishingIE(PlayerFarming playerFarming)
  {
    int playerIndex = PlayerFarming.players.IndexOf(playerFarming);
    CoopManager.Instance.LockAddRemovePlayer();
    this.Activated = true;
    if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null)
      this.curvedLinePoint[playerIndex].lockToGameObject = playerFarming.FishingLineBone;
    this.isFishing = true;
    this.playerPosition.position = playerFarming.transform.position;
    GameManager.GetInstance()?.OnConversationNew();
    this.reelingHorizontalOffset = 0.0f;
    yield return (object) new WaitForEndOfFrame();
    this.castingStrength[playerIndex] = 0.0f;
    playerFarming.TimedAction(float.MaxValue, (System.Action) null, "Fishing/fishing-loop");
    this.states[playerIndex] = StateMachine.State.Aiming;
    this._fishingOverlayControllerUI.Add(MonoSingleton<UIManager>.Instance.FishingOverlayControllerTemplate.Instantiate<UIFishingOverlayController>(GameObject.FindWithTag("Canvas").transform));
    this._fishingOverlayControllerUI[playerIndex].PlayerFarming = playerFarming;
    this._fishingOverlayControllerUI[playerIndex].ConfigurePrompt();
    this._fishingOverlayControllerUI[playerIndex].UpdateCastingStrength(0.0f);
    this._fishingOverlayControllerUI[playerIndex].Show((bool) (UnityEngine.Object) this.playerPosition.gameObject);
    this._fishingOverlayControllerUI[playerIndex].SetState(StateMachine.State.Casting);
    if (playerIndex == 0 && PlayerFarming.playersCount >= 2)
      ((RectTransform) this._fishingOverlayControllerUI[playerIndex].transform).anchoredPosition = new Vector2(0.0f, ((RectTransform) this._fishingOverlayControllerUI[playerIndex].transform).anchoredPosition.y);
    this.startedCastLoop = false;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public new void OnPlayerLeft()
  {
    if (this._fishingOverlayControllerUI == null || (double) Time.timeScale == 0.0)
      return;
    this.StopAllCoroutines();
    for (int index = 0; index < this._fishingOverlayControllerUI.Count; ++index)
    {
      if ((UnityEngine.Object) this._fishingOverlayControllerUI[index] != (UnityEngine.Object) null)
        this._fishingOverlayControllerUI[index].StopAllCoroutines();
      AudioManager.Instance.StopLoop(this.LoopedSound);
      AudioManager.Instance.StopLoop(this.CastLoopedSound);
      foreach (Fishable fishable in this.fishables)
      {
        if ((UnityEngine.Object) fishable != (UnityEngine.Object) null)
          fishable.state = StateMachine.State.Idle;
      }
      this.isFishing = false;
      this.states[index] = StateMachine.State.Idle;
      GameManager.GetInstance()?.OnConversationEnd();
      this.ReeledAmount[index] = 0.5f;
      this.fishingHooks[index].gameObject.SetActive(false);
      this.fishingLines[index].gameObject.SetActive(false);
      this.isWithinDistance[index] = false;
      this.HasChanged = true;
      this.Activated = false;
      CoopManager.Instance.UnlockAddRemovePlayer();
      if ((UnityEngine.Object) this._fishingOverlayControllerUI[index] != (UnityEngine.Object) null)
      {
        UIFishingOverlayController overlayController = this._fishingOverlayControllerUI[index];
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
        {
          overlayController.Hide();
          this.playerFarming.indicator.Reset();
        })));
      }
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    AudioManager.Instance.StopLoop(this.LoopedSound);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__105_0()
  {
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      this.gameObject.SetActive(true);
      this.StartCoroutine((IEnumerator) this.BeganFishingIE(player));
    }
    this.waitForPlayersCoroutine = (Coroutine) null;
  }

  [CompilerGenerated]
  public void \u003CFishCaughtIE\u003Eb__116_0() => this.waiting = false;

  [CompilerGenerated]
  public void \u003CNoCatch\u003Eb__118_0() => this.playerFarming.indicator.Reset();

  [Serializable]
  public class FishType
  {
    public InventoryItem.ITEM_TYPE Type;
    public int Quantity = 2;
    public Vector2 Scale;
    [Range(0.0f, 1f)]
    public float Probability;
    public int Difficulty = 1;
    public bool Summer = true;
    public bool Autumn = true;
    public bool Winter = true;
    public bool Spring = true;
  }

  public delegate void FishEvent();
}
