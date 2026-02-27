// Decompiled with JetBrains decompiler
// Type: Interaction_Fishing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using EasyCurvedLine;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Fishing : Interaction
{
  [SerializeField]
  private Structure structure;
  private Structures_FishingSpot fishingSpot;
  [Space]
  [SerializeField]
  private Interaction_Fishing.FishType[] fishTypes;
  [SerializeField]
  private Vector2 fishSpawnAmount;
  [SerializeField]
  private Vector2 fishSpawnRandomTime;
  [SerializeField]
  private Transform fishGroup;
  [SerializeField]
  private Fishable fishPrefab;
  [Space]
  [SerializeField]
  private Transform fishingHook;
  [SerializeField]
  private Transform fishingLine;
  [SerializeField]
  private Transform playerPosition;
  [SerializeField]
  private BoxCollider2D boundsCollider;
  [SerializeField]
  private float castingStrengthIncrement;
  [SerializeField]
  private AnimationCurve heightCurve;
  [SerializeField]
  private float height;
  [SerializeField]
  private float castDuration;
  [SerializeField]
  private AnimationCurve castDurationCurve;
  [SerializeField]
  private Vector2 minMaxCastDistance;
  [SerializeField]
  private float reelingSmooth;
  [SerializeField]
  private float reelSlerp;
  [SerializeField]
  private float reelHorizontalMoveSpeed;
  [SerializeField]
  private float reelingMaxSpeed;
  [SerializeField]
  private float reelingShoreOffset;
  [SerializeField]
  private float amountIncreaseOverTime = 0.01f;
  [SerializeField]
  private float amountDecreaseOverTime = 0.01f;
  private List<Fishable> fishables = new List<Fishable>();
  private bool isWithinDistance;
  private bool isFishing;
  private float castingStrength;
  private float maxReelingSpeed;
  private Vector3 hookLandPosition;
  private Vector3 hookVelocity;
  private float reelingHorizontalOffset;
  private float hookReelLerpSpeed = 2f;
  private UIFishingOverlayController _fishingOverlayControllerUI;
  public Fishable currentFish;
  private float zPosition = 0.6f;
  private float fishSpawnTimer;
  private Vector3 playerDirection;
  private Vector3 playerRightDirection;
  public UnityEvent CallBackCaught;
  public UnityEvent CallBackFail;
  public System.Action OnCatchFish;
  public System.Action OnFishEscaped;
  public GameObject FishingParticles;
  private EventInstance CastLoopedSound;
  private EventInstance LoopedSound;
  private const float minTimeBetweenHookedDirectionChange = 1.5f;
  private const float maxTimeBetweenHookedDirectionChange = 2.5f;
  private float hookDirectionChangeTimer;
  private float hookDirectionSpeed = 1f;
  private int hookDirection = 1;
  private const int maxFish = 20;
  private bool startedCastLoop;
  private bool changedState;
  private bool Activated;
  public float ReelDistance;
  private bool startedLoop;
  public CurvedLinePoint curvedLinePoint;

  public Structures_FishingSpot FishingSpot
  {
    get
    {
      if (this.fishingSpot == null)
        this.fishingSpot = this.structure.Brain as Structures_FishingSpot;
      return this.fishingSpot;
    }
  }

  public Transform FishingHook => this.fishingHook;

  public float ReelingMaxSpeed => this.reelingMaxSpeed;

  public bool FishChasing { get; set; }

  public float HookVelocity => this.hookVelocity.magnitude;

  public Vector3 HookedFishFleePosition { get; private set; }

  public Vector3 PlayerPosition
  {
    get => this.playerPosition.position + this.playerDirection * this.reelingShoreOffset;
  }

  public float ReelLerped { get; private set; }

  public event Interaction_Fishing.FishEvent OnCasted;

  public float ReeledAmount { get; private set; } = 0.5f;

  private bool RitualActive => FollowerBrainStats.IsFishing;

  private void Start()
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
    for (int index = 0; index < this.fishTypes.Length; ++index)
    {
      if (this.fishTypes[index].Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && DataManager.GetRandomLockedSkin() == "" || this.fishTypes[index].Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && DataManager.GetRandomLockedDecoration() == StructureBrain.TYPES.NONE)
        this.fishTypes[index].Type = InventoryItem.ITEM_TYPE.FISH_SWORDFISH;
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.fishSpawnTimer = Time.time + UnityEngine.Random.Range(this.fishSpawnRandomTime.x, this.fishSpawnRandomTime.y);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain == null)
      return;
    this.SpawnAlreadySpawnedFish();
  }

  private void OnBrainAssigned()
  {
    this.SpawnAlreadySpawnedFish();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void SpawnAlreadySpawnedFish()
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

  private void IncreaseChanceOfFishSkin()
  {
    if (DataManager.GetFollowerSkinUnlocked("Fish"))
      return;
    foreach (Interaction_Fishing.FishType fishType in this.fishTypes)
    {
      if (fishType.Type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
        fishType.Probability = 0.25f;
    }
  }

  protected override void Update()
  {
    base.Update();
    if (this.Activated && InputManager.Gameplay.GetCancelFishingButtonDown() && (UnityEngine.Object) this._fishingOverlayControllerUI != (UnityEngine.Object) null)
    {
      this.StopAllCoroutines();
      this._fishingOverlayControllerUI.StopAllCoroutines();
      AudioManager.Instance.StopLoop(this.LoopedSound);
      foreach (Fishable fishable in this.fishables)
      {
        if ((UnityEngine.Object) fishable != (UnityEngine.Object) null)
          fishable.state = StateMachine.State.Idle;
      }
      this.isFishing = false;
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      GameManager.GetInstance()?.OnConversationEnd();
      this.ReeledAmount = 0.5f;
      this.fishingHook.gameObject.SetActive(false);
      this.fishingLine.gameObject.SetActive(false);
      this.isWithinDistance = false;
      this.HasChanged = true;
      this.Activated = false;
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
      {
        this._fishingOverlayControllerUI.Hide();
        MonoSingleton<Indicator>.Instance.Reset();
      })));
    }
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance)
    {
      if ((double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) < (double) this.ActivateDistance)
      {
        if (!this.isWithinDistance && this.state.CURRENT_STATE != StateMachine.State.Idle)
        {
          GameManager.GetInstance().AddToCamera(this.LockPosition.gameObject);
          this.isWithinDistance = true;
        }
      }
      else if (this.isWithinDistance)
      {
        GameManager.GetInstance().RemoveFromCamera(this.LockPosition.gameObject);
        this.isWithinDistance = false;
      }
    }
    if ((double) Time.time > (double) this.fishSpawnTimer && (double) this.fishables.Count < (double) this.fishSpawnAmount.y)
    {
      this.SpawnFish(1, true);
      this.fishSpawnTimer = Time.time + UnityEngine.Random.Range(this.fishSpawnRandomTime.x, this.fishSpawnRandomTime.y);
    }
    bool flag1 = SettingsManager.Settings.Accessibility.AutoFish || InputManager.Gameplay.GetInteractButtonDown();
    if (this.state.CURRENT_STATE == StateMachine.State.Aiming & flag1)
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    if (this.state.CURRENT_STATE == StateMachine.State.Charging)
    {
      bool flag2 = false;
      if (!SettingsManager.Settings.Accessibility.AutoFish)
      {
        flag1 = InputManager.Gameplay.GetInteractButtonHeld();
        flag2 = InputManager.Gameplay.GetInteractButtonUp();
      }
      if (flag1)
      {
        if (!this.startedCastLoop)
        {
          this.startedCastLoop = true;
          this.CastLoopedSound = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
        }
        int num = (int) this.CastLoopedSound.setParameterByName("hold_time", this.castingStrength);
        this.castingStrength = Mathf.Clamp(this.castingStrength + this.castingStrengthIncrement * Time.deltaTime, 0.0f, 1f);
        this._fishingOverlayControllerUI.UpdateCastingStrength(this.castingStrength);
        if (!this.changedState)
        {
          this._fishingOverlayControllerUI.CastingButtonDown(true);
          this.changedState = true;
        }
        if (SettingsManager.Settings.Accessibility.AutoFish)
        {
          Vector3 vector3 = PlayerFarming.Instance.FishingLineBone.transform.position + Vector3.down * Mathf.Lerp(this.minMaxCastDistance.x, this.minMaxCastDistance.y, this.castingStrength);
          foreach (Fishable fishable in this.fishables)
          {
            if ((double) vector3.y < (double) fishable.transform.position.y)
              this.CastLine();
          }
        }
        if ((double) this.castingStrength >= 1.0)
          this.CastLine();
      }
      else if (flag2)
      {
        this.CastLine();
        if (this.changedState)
        {
          this._fishingOverlayControllerUI.CastingButtonDown(false);
          this.changedState = false;
        }
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Reeling)
    {
      this.maxReelingSpeed = !InputManager.Gameplay.GetInteractButtonHeld() ? Mathf.Clamp(this.maxReelingSpeed - this.reelSlerp * Time.deltaTime, 0.0f, this.reelingMaxSpeed) : this.reelingMaxSpeed;
      float horizontalAxis = InputManager.Gameplay.GetHorizontalAxis();
      if ((double) Mathf.Abs(horizontalAxis) > 0.10000000149011612)
        this.reelingHorizontalOffset = Mathf.Clamp(this.reelingHorizontalOffset + horizontalAxis * this.reelHorizontalMoveSpeed * Time.deltaTime, -1f, 1f);
      this.Reel();
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking)
      return;
    this.ReeledAmount += (this._fishingOverlayControllerUI.IsNeedleWithinSection() ? this.amountIncreaseOverTime : this.amountDecreaseOverTime) * Time.deltaTime;
    this._fishingOverlayControllerUI.UpdateReelBar(this.ReeledAmount);
    if ((double) this.ReeledAmount <= 0.0)
    {
      this.currentFish.Spooked();
      this.NoCatch();
    }
    else if ((double) this.ReeledAmount >= 1.0)
      this.FishCaught();
    this.ReelLerped = Mathf.Lerp(this.ReelLerped, this.ReeledAmount, this.hookReelLerpSpeed * Time.deltaTime);
    Vector3 vector3_1 = Vector3.Lerp(this.HookedFishFleePosition, PlayerFarming.Instance.Spine.transform.position, this.ReelLerped);
    this.reelingHorizontalOffset += this.hookDirectionSpeed * (float) this.hookDirection * Time.deltaTime;
    if ((double) Time.time > (double) this.hookDirectionChangeTimer)
    {
      this.hookDirection *= -1;
      this.hookDirectionChangeTimer = Time.time + UnityEngine.Random.Range(1.5f, 2.5f);
    }
    this.fishingHook.transform.position = new Vector3(vector3_1.x + this.reelingHorizontalOffset, vector3_1.y, this.zPosition);
  }

  public override void GetLabel()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Idle)
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
    PlayerFarming.Instance.GoToAndStop(this.transform.position, this.gameObject, GoToCallback: (System.Action) (() =>
    {
      this.gameObject.SetActive(true);
      if (this.isFishing)
        return;
      this.StartCoroutine((IEnumerator) this.BeganFishingIE());
    }));
  }

  private bool IsGoodFishType(InventoryItem.ITEM_TYPE fishType)
  {
    return fishType == InventoryItem.ITEM_TYPE.FISH_SQUID || fishType == InventoryItem.ITEM_TYPE.FISH_SWORDFISH || fishType == InventoryItem.ITEM_TYPE.FISH_OCTOPUS || fishType == InventoryItem.ITEM_TYPE.FISH_LOBSTER;
  }

  private void Reel()
  {
    Vector3 vector3 = Vector3.SmoothDamp(this.fishingHook.transform.position, PlayerFarming.Instance.FishingLineBone.transform.position + this.playerRightDirection * this.reelingHorizontalOffset, ref this.hookVelocity, this.reelingSmooth, this.maxReelingSpeed);
    this.fishingHook.transform.position = new Vector3(vector3.x, vector3.y, this.zPosition);
    this.ReelDistance = Vector3.Distance(this.fishingHook.transform.position, PlayerFarming.Instance.transform.position);
    if ((double) Vector3.Distance(this.fishingHook.transform.position, PlayerFarming.Instance.transform.position) >= 2.0999999046325684)
      return;
    this.NoCatch();
  }

  private void CastLine()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/hold_activate", PlayerFarming.Instance.gameObject.transform.position);
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    this.StartCoroutine((IEnumerator) this.CastLineIE());
  }

  private IEnumerator CastLineIE()
  {
    Interaction_Fishing interactionFishing = this;
    interactionFishing.fishingHook.gameObject.SetActive(true);
    interactionFishing.isFishing = true;
    interactionFishing.state.CURRENT_STATE = StateMachine.State.Casting;
    interactionFishing.GetLabel();
    interactionFishing.fishingHook.transform.position = PlayerFarming.Instance.FishingLineBone.transform.position;
    interactionFishing.fishingHook.gameObject.SetActive(true);
    interactionFishing.fishingLine.gameObject.SetActive(true);
    float a1 = Vector3.Dot(Vector3.left, PlayerFarming.Instance.transform.position - interactionFishing.LockPosition.position);
    float b1 = Vector3.Dot(Vector3.right, PlayerFarming.Instance.transform.position - interactionFishing.LockPosition.position);
    float a2 = Vector3.Dot(Vector3.up, PlayerFarming.Instance.transform.position - interactionFishing.LockPosition.position);
    float b2 = Vector3.Dot(Vector3.down, PlayerFarming.Instance.transform.position - interactionFishing.LockPosition.position);
    float num = Mathf.Max(Mathf.Max(a1, b1), Mathf.Max(a2, b2));
    if ((double) num == (double) a1)
      interactionFishing.playerDirection = Vector3.right;
    else if ((double) num == (double) b1)
      interactionFishing.playerDirection = Vector3.left;
    else if ((double) num == (double) a2)
      interactionFishing.playerDirection = Vector3.down;
    else if ((double) num == (double) b2)
      interactionFishing.playerDirection = Vector3.up;
    float degree = Utils.GetAngle(Vector3.zero, interactionFishing.playerDirection) + 90f;
    interactionFishing.playerRightDirection = (Vector3) Utils.DegreeToVector2(degree);
    Vector3 fromPosition = PlayerFarming.Instance.FishingLineBone.transform.position;
    interactionFishing.hookLandPosition = PlayerFarming.Instance.FishingLineBone.transform.position + interactionFishing.playerDirection * Mathf.Lerp(interactionFishing.minMaxCastDistance.x, interactionFishing.minMaxCastDistance.y, interactionFishing.castingStrength);
    AudioManager.Instance.PlayOneShot("event:/fishing/cast_rod", PlayerFarming.Instance.gameObject.transform.position);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Fishing/fishing-start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "Fishing/fishing-loop", true, 0.0f);
    float t = 0.0f;
    while ((double) t < (double) interactionFishing.castDuration)
    {
      float time = t / interactionFishing.castDuration;
      Vector3 vector3 = Vector3.Lerp(fromPosition, new Vector3(interactionFishing.hookLandPosition.x, interactionFishing.hookLandPosition.y, interactionFishing.zPosition), interactionFishing.castDurationCurve.Evaluate(time)) with
      {
        z = interactionFishing.heightCurve.Evaluate(time) * interactionFishing.height
      };
      interactionFishing.fishingHook.transform.position = vector3;
      t += Time.deltaTime;
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", interactionFishing.fishingHook.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionFishing.fishingHook.gameObject, 12f);
    interactionFishing.state.CURRENT_STATE = StateMachine.State.Reeling;
    interactionFishing._fishingOverlayControllerUI.SetState(StateMachine.State.Idle);
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_Fishing.FishEvent onCasted = interactionFishing.OnCasted;
    if (onCasted != null)
      onCasted();
  }

  public bool IsClosestFish(Fishable fish)
  {
    Fishable fishable1 = (Fishable) null;
    foreach (Fishable fishable2 in this.fishables)
    {
      if ((UnityEngine.Object) fishable2 != (UnityEngine.Object) null && ((UnityEngine.Object) fishable1 == (UnityEngine.Object) null || (double) Vector3.Distance(fishable2.transform.position, this.fishingHook.transform.position) < (double) Vector3.Distance(fishable1.transform.position, this.fishingHook.transform.position)))
        fishable1 = fishable2;
    }
    return (UnityEngine.Object) fishable1 != (UnityEngine.Object) null && (UnityEngine.Object) fishable1 == (UnityEngine.Object) fish;
  }

  public void FishOn(Fishable currentFish)
  {
    Debug.Log((object) "Fish on");
    if (!this.startedLoop)
    {
      this.startedLoop = true;
      this.LoopedSound = AudioManager.Instance.CreateLoop("event:/fishing/caught_something_loop", PlayerFarming.Instance.gameObject, true);
    }
    AudioManager.Instance.PlayOneShot("event:/fishing/caught_something_alert", PlayerFarming.Instance.transform.position);
    this._fishingOverlayControllerUI.SetState(StateMachine.State.Attacking);
    this._fishingOverlayControllerUI.SetReelingDifficulty(currentFish.FishType.Difficulty - 1);
    this.state.CURRENT_STATE = StateMachine.State.Attacking;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Fishing/fishing-reel", true);
    this.ReelLerped = this.ReeledAmount;
    float num = Vector3.Distance(currentFish.transform.position, this.playerPosition.position);
    Vector3 normalized = (currentFish.transform.position - this.playerPosition.position).normalized;
    this.HookedFishFleePosition = currentFish.transform.position + normalized * num;
    this.fishingHook.gameObject.SetActive(false);
    this.currentFish = currentFish;
  }

  private void FishCaught() => this.StartCoroutine((IEnumerator) this.FishCaughtIE());

  private IEnumerator FishCaughtIE()
  {
    Interaction_Fishing interactionFishing = this;
    AudioManager.Instance.StopLoop(interactionFishing.LoopedSound);
    MMVibrate.StopRumble();
    interactionFishing.startedLoop = false;
    interactionFishing.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionFishing._fishingOverlayControllerUI.Hide();
    interactionFishing.isFishing = false;
    while ((double) Mathf.Abs(interactionFishing.ReelLerped - interactionFishing.ReeledAmount) > 0.10000000149011612)
    {
      interactionFishing.ReelLerped = Mathf.Lerp(interactionFishing.ReelLerped, interactionFishing.ReeledAmount, interactionFishing.hookReelLerpSpeed * Time.deltaTime);
      Vector3 vector3 = Vector3.Lerp(interactionFishing.HookedFishFleePosition, PlayerFarming.Instance.FishingLineBone.transform.position, interactionFishing.ReelLerped);
      interactionFishing.fishingHook.transform.position = new Vector3(vector3.x, vector3.y, interactionFishing.zPosition);
      yield return (object) null;
    }
    interactionFishing.fishingHook.gameObject.SetActive(false);
    interactionFishing.currentFish.gameObject.SetActive(false);
    interactionFishing.fishingLine.gameObject.SetActive(false);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Fishing/fishing-catch", true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/fishing/catch_fish", PlayerFarming.Instance.gameObject.transform.position);
    interactionFishing.FishingSpot.FishCaught(interactionFishing.currentFish.FishType);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.TimedAction(2.4f, (System.Action) null, "reactions/react-happy");
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
    if (!DataManager.Instance.GetVariable(DataManager.Variables.ShoreFishFished))
    {
      if ((interactionFishing.currentFish.ItemType == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN || DataManager.Instance.FishCaughtTotal == 4) && !DataManager.GetFollowerSkinUnlocked("Fish"))
      {
        FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, 1, PlayerFarming.Instance.transform.position).GetComponent<FoundItemPickUp>();
        component.FollowerSkinForceSelection = true;
        component.SkinToForce = "Fish";
      }
      else
      {
        InventoryItem.ITEM_TYPE type = interactionFishing.currentFish.ItemType;
        int quantity = interactionFishing.currentFish.Quantity;
        if (type == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && DataManager.GetFollowerSkinUnlocked("Fish"))
          type = InventoryItem.ITEM_TYPE.FISH_BIG;
        else if (type == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
          quantity = 1;
        InventoryItem.Spawn(type, quantity, PlayerFarming.Instance.transform.position);
        if (type == InventoryItem.ITEM_TYPE.FISH_SMALL || type == InventoryItem.ITEM_TYPE.FISH_BLOWFISH || type == InventoryItem.ITEM_TYPE.FISH_CRAB || type == InventoryItem.ITEM_TYPE.FISH_SQUID || type == InventoryItem.ITEM_TYPE.FISH_BIG || type == InventoryItem.ITEM_TYPE.FISH_LOBSTER || type == InventoryItem.ITEM_TYPE.FISH_OCTOPUS || type == InventoryItem.ITEM_TYPE.FISH_SWORDFISH)
        {
          if (!DataManager.Instance.FishCaught.Contains(interactionFishing.currentFish.ItemType))
            DataManager.Instance.FishCaught.Add(interactionFishing.currentFish.ItemType);
          interactionFishing.checkAchievements();
        }
      }
    }
    else
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_BIG, 2, PlayerFarming.Instance.transform.position + Vector3.right);
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
        if (randomFishType.Type != InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && randomFishType.Type != InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
          InventoryItem.Spawn(randomFishType.Type, 1, PlayerFarming.Instance.transform.position + Vector3.right);
        if (!DataManager.Instance.FishCaught.Contains(randomFishType.Type))
        {
          DataManager.Instance.FishCaught.Add(randomFishType.Type);
          interactionFishing.checkAchievements();
        }
      }
    }
    interactionFishing.CallBackCaught?.Invoke();
    yield return (object) new WaitForSeconds(1f);
    interactionFishing.ReeledAmount = 0.5f;
    interactionFishing.fishables.Remove(interactionFishing.currentFish);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFishing.currentFish);
    interactionFishing.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    MonoSingleton<Indicator>.Instance.Reset();
    interactionFishing.isWithinDistance = false;
    interactionFishing.HasChanged = true;
    interactionFishing.Activated = false;
    System.Action onCatchFish = interactionFishing.OnCatchFish;
    if (onCatchFish != null)
      onCatchFish();
    ++DataManager.Instance.FishCaughtTotal;
  }

  public void checkAchievements()
  {
    Debug.Log((object) ("Achievement check, collected fish types" + (object) DataManager.Instance.FishCaught.Count));
    if (DataManager.Instance.FishCaught.Count < 8)
      return;
    Debug.Log((object) "Achievement unlocked, collected all fish types");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FISH_ALL_TYPES"));
  }

  private void NoCatch()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.startedLoop = false;
    AudioManager.Instance.PlayOneShot("event:/fishing/fishing_failure", PlayerFarming.Instance.gameObject.transform.position);
    this.CallBackFail?.Invoke();
    System.Action onFishEscaped = this.OnFishEscaped;
    if (onFishEscaped != null)
      onFishEscaped();
    this.isFishing = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance()?.OnConversationEnd();
    this.ReeledAmount = 0.5f;
    this.fishingHook.gameObject.SetActive(false);
    this.fishingLine.gameObject.SetActive(false);
    this._fishingOverlayControllerUI.Hide();
    this.isWithinDistance = false;
    this.HasChanged = true;
    this.Activated = false;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => MonoSingleton<Indicator>.Instance.Reset())));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForSeconds(0.1f);
    callback();
  }

  private void SpawnFish(int amount, bool addFishSpawned)
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

  private Fishable SpawnFish(Interaction_Fishing.FishType fishType, bool addFishSpawned)
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

  private Interaction_Fishing.FishType GetRandomFishType()
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

  private IEnumerator BeganFishingIE()
  {
    Interaction_Fishing interactionFishing = this;
    interactionFishing.Activated = true;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      interactionFishing.curvedLinePoint.lockToGameObject = PlayerFarming.Instance.FishingLineBone;
    interactionFishing.isFishing = true;
    interactionFishing.hookVelocity = Vector3.zero;
    interactionFishing.playerPosition.position = PlayerFarming.Instance.transform.position;
    GameManager.GetInstance()?.OnConversationNew();
    interactionFishing.reelingHorizontalOffset = 0.0f;
    yield return (object) new WaitForEndOfFrame();
    interactionFishing.castingStrength = 0.0f;
    interactionFishing.state.CURRENT_STATE = StateMachine.State.Aiming;
    PlayerFarming.Instance.TimedAction(float.MaxValue, (System.Action) null, "Fishing/fishing-loop");
    if ((UnityEngine.Object) interactionFishing._fishingOverlayControllerUI == (UnityEngine.Object) null)
      interactionFishing._fishingOverlayControllerUI = MonoSingleton<UIManager>.Instance.FishingOverlayControllerTemplate.Instantiate<UIFishingOverlayController>(GameObject.FindWithTag("Canvas").transform);
    interactionFishing._fishingOverlayControllerUI.UpdateCastingStrength(0.0f);
    interactionFishing._fishingOverlayControllerUI.Show((bool) (UnityEngine.Object) interactionFishing.playerPosition.gameObject);
    interactionFishing._fishingOverlayControllerUI.SetState(StateMachine.State.Casting);
    interactionFishing.startedCastLoop = false;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  private new void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.CastLoopedSound);
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public override void IndicateHighlighted()
  {
  }

  public override void EndIndicateHighlighted()
  {
  }

  [Serializable]
  public class FishType
  {
    public InventoryItem.ITEM_TYPE Type;
    public int Quantity = 2;
    public Vector2 Scale;
    [Range(0.0f, 1f)]
    public float Probability;
    public int Difficulty = 1;
  }

  public delegate void FishEvent();
}
