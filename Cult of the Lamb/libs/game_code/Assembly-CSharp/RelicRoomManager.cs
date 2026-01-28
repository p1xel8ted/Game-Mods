// Decompiled with JetBrains decompiler
// Type: RelicRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMRoomGeneration;
using MMTools;
using Spine.Unity;
using src;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class RelicRoomManager : MonoBehaviour
{
  public static RelicRoomManager Instance;
  [SerializeField]
  public List<SpriteRenderer> stainedGlasses = new List<SpriteRenderer>();
  [SerializeField]
  public List<MeshRenderer> decalStainedMesh = new List<MeshRenderer>();
  [SerializeField]
  public BiomeLightingSettings relicRoomLighting;
  [SerializeField]
  public Interaction_SimpleConversation onboardingConversation;
  [SerializeField]
  public Interaction_SimpleConversation winterConversation;
  [SerializeField]
  [Tooltip("Conversation for Chemach when you encounter him in the D6 dungeon")]
  public Interaction_SimpleConversation rotConversation;
  [SerializeField]
  public Interaction_SimpleConversation equippedRelicConversation;
  [SerializeField]
  public Interaction_SimpleConversation completedRelicConversation;
  [SerializeField]
  public Interaction_SimpleConversation sinOnboardedConversation;
  [SerializeField]
  public AssetReferenceGameObject enemy;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public Vector3 middle;
  [SerializeField]
  public SimpleBark angryBark;
  [SerializeField]
  public SimpleBark defaultBark;
  [SerializeField]
  public GameObject npc;
  [SerializeField]
  public CanvasGroup controlsHUD;
  [SerializeField]
  public Interaction_WeaponSelectionPodium[] podiums;
  [SerializeField]
  public Health[] effigies;
  [SerializeField]
  public Interaction_RelicBook relicBook;
  public List<Material> decalStainedMaterial = new List<Material>();
  public float _randomAlpha0 = 0.5f;
  public float _randomAlpha1 = 0.5f;
  [CompilerGenerated]
  public int \u003CRelicUsedCount\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CRelicTargetCount\u003Ek__BackingField = 1;
  public ObjectivesData objective;
  public bool barksEnabled = true;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public float time;
  public static int Color1 = Shader.PropertyToID("_Color");

  public Interaction_SimpleConversation EquippedRelicConversation => this.equippedRelicConversation;

  public int RelicUsedCount
  {
    get => this.\u003CRelicUsedCount\u003Ek__BackingField;
    set => this.\u003CRelicUsedCount\u003Ek__BackingField = value;
  }

  public int RelicTargetCount => this.\u003CRelicTargetCount\u003Ek__BackingField;

  public void Awake()
  {
    RelicRoomManager.Instance = this;
    this.controlsHUD.alpha = 0.0f;
    foreach (MeshRenderer meshRenderer in this.decalStainedMesh)
    {
      Material material = new Material(meshRenderer.material);
      meshRenderer.sharedMaterial = material;
      this.decalStainedMaterial.Add(meshRenderer.sharedMaterial);
    }
    this._randomAlpha0 = UnityEngine.Random.Range(0.5f, 1f);
    this._randomAlpha1 = UnityEngine.Random.Range(0.5f, 1f);
    if (DataManager.Instance.OnboardedRelics)
    {
      this.defaultBark.gameObject.SetActive(true);
      this.defaultBark.OnPlay += new SimpleBark.NormalEvent(this.DefaultBark_OnPlay);
      this.MakeEffigiesEnemies(true);
      foreach (Interaction podium in this.podiums)
        podium.OnInteraction += new Interaction.InteractionEvent(this.Pod_OnInteraction);
      if (!((UnityEngine.Object) this.sinOnboardedConversation != (UnityEngine.Object) null))
        return;
      this.sinOnboardedConversation.enabled = !DataManager.Instance.ChemachOnboardedSin;
    }
    else
    {
      this.GetComponentInParent<GenerateRoom>().LockingDoors = true;
      this.barksEnabled = false;
    }
  }

  public void Start()
  {
    bool flag1 = (UnityEngine.Object) this.winterConversation != (UnityEngine.Object) null && DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter && this.IsInDungeon5 && !DataManager.Instance.SpokenToChemachWinter;
    bool flag2 = (UnityEngine.Object) this.rotConversation != (UnityEngine.Object) null && this.IsInDungeon6 && !DataManager.Instance.SpokenToChemachRot;
    if (!DataManager.Instance.OnboardedRelics || !(flag1 | flag2))
      return;
    if ((UnityEngine.Object) this.sinOnboardedConversation != (UnityEngine.Object) null)
      this.sinOnboardedConversation.enabled = false;
    this.onboardingConversation.gameObject.SetActive(false);
    this.defaultBark.gameObject.SetActive(false);
    this.winterConversation.gameObject.SetActive(flag1);
    this.rotConversation.gameObject.SetActive(flag2);
  }

  public void OnEnable() => AudioManager.Instance.ToggleFilter(SoundParams.Inside, true);

  public void OnDisable() => AudioManager.Instance.ToggleFilter(SoundParams.Inside, false);

  public bool IsInDungeon5
  {
    get
    {
      bool isInDungeon5 = PlayerFarming.Location == FollowerLocation.Dungeon1_5;
      Debug.Log((object) $"RelicRoomManager: IsInDungeon5? {{PlayerFarming.Location:{PlayerFarming.Location}, Result:{isInDungeon5}}}");
      return isInDungeon5;
    }
  }

  public bool IsInDungeon6
  {
    get
    {
      bool isInDungeon6 = PlayerFarming.Location == FollowerLocation.Dungeon1_6;
      Debug.Log((object) $"RelicRoomManager: IsInDungeon5? {{PlayerFarming.Location:{PlayerFarming.Location}, Result:{isInDungeon6}}}");
      return isInDungeon6;
    }
  }

  public void WinterConversationFinished() => this.defaultBark.gameObject.SetActive(true);

  public void OnDestroy()
  {
    RelicRoomManager.Instance = (RelicRoomManager) null;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerRelic.OnRelicUsed -= new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicUsed);
      player.playerRelic.OnRelicChargeModified -= new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicChargeModified);
    }
    foreach (Interaction podium in this.podiums)
      podium.OnInteraction -= new Interaction.InteractionEvent(this.Pod_OnInteraction);
    this.defaultBark.OnPlay -= new SimpleBark.NormalEvent(this.DefaultBark_OnPlay);
    if (RelicRoomManager.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in RelicRoomManager.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    RelicRoomManager.loadedAddressableAssets.Clear();
  }

  public void DefaultBark_OnPlay() => this.defaultBark.ActivateDistance = 100f;

  public void Pod_OnInteraction(StateMachine state)
  {
    this.angryBark.gameObject.SetActive(false);
    this.defaultBark.gameObject.SetActive(true);
    this.barksEnabled = false;
    GameManager.GetInstance().WaitForSeconds(6f, (System.Action) (() =>
    {
      foreach (Interaction podium in this.podiums)
        podium.OnInteraction -= new Interaction.InteractionEvent(this.Pod_OnInteraction);
      if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
        AudioManager.Instance.PlayOneShot("event:/relics/chemach_leaves_whoosh", this.gameObject);
      if (!((UnityEngine.Object) this.spine != (UnityEngine.Object) null))
        return;
      this.spine.AnimationState.SetAnimation(0, "exit", false);
    }));
  }

  public void Update()
  {
    Color rgb = Color.HSVToRGB(TimeManager.TimeRemainingUntilPhase(DayPhase.Night) / 1200f, 1f, 1f);
    Color color1 = rgb;
    Color color2 = new Color(0.2f, 0.2f, 0.2f);
    color1.a = Mathf.Lerp(0.1f, 0.5f, TimeManager.TimeRemainingUntilPhase(DayPhase.Night) / 1200f);
    foreach (Material material in this.decalStainedMaterial)
    {
      material.SetColor(RelicRoomManager.Color1, color1 + color2);
      material.color = new Color(material.color.r, material.color.g, material.color.g, 0.75f);
    }
    this.relicRoomLighting.AmbientColour = new Color(Mathf.Clamp01(rgb.g - 0.5f), Mathf.Clamp01(rgb.b - 0.5f), Mathf.Clamp01(rgb.r - 0.5f));
    rgb.r = Mathf.Clamp01(rgb.r + 0.8f);
    rgb.g = Mathf.Clamp01(rgb.g + 0.8f);
    rgb.b = Mathf.Clamp01(rgb.b + 0.8f);
    foreach (SpriteRenderer stainedGlass in this.stainedGlasses)
    {
      if ((UnityEngine.Object) stainedGlass != (UnityEngine.Object) null)
        stainedGlass.color = rgb;
    }
  }

  public void AttackedEffigy()
  {
    if (!this.barksEnabled)
      return;
    this.defaultBark.gameObject.SetActive(false);
    if (!this.angryBark.gameObject.activeSelf)
    {
      this.angryBark.gameObject.SetActive(true);
    }
    else
    {
      if (MMConversation.CURRENT_CONVERSATION != null)
        return;
      this.angryBark.Show();
    }
  }

  public void SpawnEnemyAndRechargeRelic()
  {
    DataManager.Instance.SpawnedRelicsThisRun.Add(RelicType.LightningStrike);
    this.podiums[0].transform.DOMoveZ(1f, 0.5f);
    ObjectiveManager.Add(this.objective = (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GiveRelic", Objectives.CustomQuestTypes.ChargeRelic), true);
    PlayerFarming.Instance.health.untouchable = true;
    this.relicBook.Interactable = false;
    this.relicBook.SecondaryInteractable = false;
    this.StartCoroutine((IEnumerator) this.SpawnEnemyIE());
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((bool) (UnityEngine.Object) player.playerRelic && (bool) (UnityEngine.Object) player.playerRelic.CurrentRelic)
        player.playerRelic.CurrentRelic.DamageRequiredToCharge /= 5f;
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.playerRelic.OnRelicUsed += new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicUsed);
      player.playerRelic.OnRelicChargeModified += new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicChargeModified);
    }
  }

  public void PlayerRelic_OnRelicChargeModified(RelicData relic, PlayerFarming target = null)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player1 = PlayerFarming.players[index];
      if ((bool) (UnityEngine.Object) player1.playerRelic && (bool) (UnityEngine.Object) player1.playerRelic.CurrentRelic)
      {
        if ((double) player1.playerRelic.previousChargeAmount != (double) player1.playerRelic.ChargedAmount && player1.playerRelic.IsFullyCharged)
        {
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ChargeRelic);
          ObjectiveManager.Add(this.objective = (ObjectivesData) new Objectives_UseRelic("Objectives/GroupTitles/GiveRelic"));
          this.ShowControlsIE(player1);
          foreach (PlayerFarming player2 in PlayerFarming.players)
            player2.playerRelic.OnRelicChargeModified -= new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicChargeModified);
        }
        player1.playerRelic.previousChargeAmount = player1.playerRelic.ChargedAmount;
      }
    }
  }

  public void PlayerRelic_OnRelicUsed(RelicData relic, PlayerFarming target)
  {
    ++this.RelicUsedCount;
    ObjectiveManager.UpdateObjective(this.objective);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.playerRelic.OnRelicUsed -= new PlayerRelic.RelicEvent(this.PlayerRelic_OnRelicUsed);
    if (this.RelicUsedCount < this.RelicTargetCount)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.CompletedIE(PlayerFarming.Instance));
  }

  public void ShowControlsIE(PlayerFarming player)
  {
    foreach (MMControlPrompt componentsInChild in this.controlsHUD.GetComponentsInChildren<MMControlPrompt>())
    {
      componentsInChild.playerFarming = player;
      componentsInChild.ForceUpdate();
    }
    this.controlsHUD.transform.localScale = Vector3.one;
    this.controlsHUD.alpha = 1f;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) this.controlsHUD.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f));
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.controlsHUD.transform.DOLocalMoveY(-440f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack)).OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
    {
      sequence = DOTween.Sequence();
      sequence.AppendInterval(2f);
      sequence.Append((Tween) this.controlsHUD.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f));
      sequence.SetLoops<DG.Tweening.Sequence>(-1);
    }));
  }

  public IEnumerator SpawnEnemyIE()
  {
    RelicRoomManager relicRoomManager = this;
    int maxEnemies = 3;
    while (relicRoomManager.RelicUsedCount < relicRoomManager.RelicTargetCount)
    {
      while (Health.team2.Count >= maxEnemies)
        yield return (object) null;
      yield return (object) new WaitForSeconds(1f);
      for (int i = 0; i < maxEnemies - Health.team2.Count; ++i)
      {
        Addressables.LoadAssetAsync<GameObject>((object) relicRoomManager.enemy).Completed += new Action<AsyncOperationHandle<GameObject>>(relicRoomManager.\u003CSpawnEnemyIE\u003Eb__55_0);
        yield return (object) new WaitForSeconds(0.5f);
      }
      yield return (object) new WaitForSeconds(1f);
    }
  }

  public IEnumerator CompletedIE(PlayerFarming playerFarming)
  {
    RelicRoomManager relicRoomManager = this;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.playerRelic.CurrentRelic != (UnityEngine.Object) null)
        player.playerRelic.CurrentRelic.DamageRequiredToCharge *= 5f;
    }
    relicRoomManager.controlsHUD.alpha = 0.0f;
    yield return (object) new WaitForSeconds(1.75f);
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null)
        Health.team2[index].DealDamage(Health.team2[index].HP, relicRoomManager.gameObject, relicRoomManager.transform.position);
    }
    yield return (object) new WaitForSeconds(0.25f);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.untouchable = false;
    PlayerReturnToBase.Disabled = false;
    relicRoomManager.relicBook.Interactable = true;
    relicRoomManager.relicBook.SecondaryInteractable = true;
    relicRoomManager.completedRelicConversation.Play();
    relicRoomManager.MakeEffigiesEnemies(true);
  }

  public void MakeEffigiesEnemies(bool enable)
  {
    foreach (Health effigy in this.effigies)
      effigy.team = enable ? Health.Team.Team2 : Health.Team.Neutral;
  }

  [CompilerGenerated]
  public void \u003CPod_OnInteraction\u003Eb__46_0()
  {
    foreach (Interaction podium in this.podiums)
      podium.OnInteraction -= new Interaction.InteractionEvent(this.Pod_OnInteraction);
    if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
      AudioManager.Instance.PlayOneShot("event:/relics/chemach_leaves_whoosh", this.gameObject);
    if (!((UnityEngine.Object) this.spine != (UnityEngine.Object) null))
      return;
    this.spine.AnimationState.SetAnimation(0, "exit", false);
  }

  [CompilerGenerated]
  public void \u003CSpawnEnemyIE\u003Eb__55_0(AsyncOperationHandle<GameObject> obj)
  {
    RelicRoomManager.loadedAddressableAssets.Add(obj);
    EnemySpawner.Create(this.middle + (Vector3) (UnityEngine.Random.insideUnitCircle * 2.5f), this.transform.parent, obj.Result.gameObject);
  }
}
