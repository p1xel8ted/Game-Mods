// Decompiled with JetBrains decompiler
// Type: GhostNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Splines;

#nullable disable
public class GhostNPC : BaseMonoBehaviour
{
  public static List<GhostNPC> GhostNPCs = new List<GhostNPC>();
  [SerializeField]
  public DataManager.Variables activeVariable;
  [SerializeField]
  public bool hasShop = true;
  [SerializeField]
  public Transform housePosition;
  [SerializeField]
  public DataManager.Variables brokenShopVariable;
  [SerializeField]
  public Interaction_SimpleConversation brokenShopInteraction;
  [SerializeField]
  public Vector3 endPointScale;
  [SerializeField]
  public DataManager.Variables fixedShopVariable;
  [SerializeField]
  public Interaction_SimpleConversation fixedShopInteraction;
  [SerializeField]
  public DLCRebuildableShop rebuildableShop;
  [SerializeField]
  public Interaction_SimpleInteraction rebuildableShopInteractionForwarder;
  [SerializeField]
  public Interaction_SimpleConversation rescueConversation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_skeletonAnimation")]
  public string enterAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_skeletonAnimation")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_skeletonAnimation")]
  public string grumpyIdleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_skeletonAnimation")]
  public string moveAnimation;
  public float lookAtDistance = 5f;
  [SerializeField]
  public Interaction_SimpleConversation gotLightningShardsInteraction;
  [SerializeField]
  public Interaction_SimpleConversation gotResourcesInteraction;
  [SerializeField]
  public Interaction_SimpleConversation gotResourcesAltInteraction;
  [SerializeField]
  public List<Interaction_SimpleConversation> jobCompleteConvos;
  [SerializeField]
  public Interaction_SimpleConversation jobBoardCompleteConvo;
  public bool _isHome;
  public Vector3 _targetPos;
  public UnitObject _ghostAI;
  public SplineAnimate _splineAnimate;
  public SkeletonAnimation _skeletonAnimation;
  [SerializeField]
  public GameObject casualBark;
  [SerializeField]
  public GameObject repairBark;
  public bool rescued;
  public Queue<Interaction_SimpleConversation> jobCompleteConvoQueue = new Queue<Interaction_SimpleConversation>();
  [CompilerGenerated]
  public bool \u003CIsRevealed\u003Ek__BackingField;
  public bool forwardInteractableSet;

  public Interaction_SimpleConversation BrokenShopInteraction => this.brokenShopInteraction;

  public bool IsHome => this._isHome;

  public bool HasJobCompleteConvos
  {
    get => this.jobCompleteConvos != null && this.jobCompleteConvos.Count > 0;
  }

  public Interaction_SimpleConversation NextJobCompleteConvo
  {
    get
    {
      if (this.jobCompleteConvos == null || this.jobCompleteConvos.Count == 0)
        return (Interaction_SimpleConversation) null;
      if (this.jobCompleteConvoQueue.Count == 0)
      {
        List<Interaction_SimpleConversation> ts;
        using (CollectionPool<List<Interaction_SimpleConversation>, Interaction_SimpleConversation>.Get(out ts))
        {
          ts.AddRange((IEnumerable<Interaction_SimpleConversation>) this.jobCompleteConvos);
          ts.Shuffle<Interaction_SimpleConversation>();
          if (this.jobCompleteConvoQueue == null)
            this.jobCompleteConvoQueue = new Queue<Interaction_SimpleConversation>();
          foreach (Interaction_SimpleConversation simpleConversation in ts)
            this.jobCompleteConvoQueue.Enqueue(simpleConversation);
        }
      }
      return this.jobCompleteConvoQueue.Dequeue();
    }
  }

  public bool IsRancher => this.activeVariable == DataManager.Variables.NPCGhostRancherRescued;

  public bool isBrokenShopInteractionActive
  {
    get
    {
      return (UnityEngine.Object) this.brokenShopInteraction != (UnityEngine.Object) null && DataManager.Instance.GetVariable(this.activeVariable) && !DataManager.Instance.GetVariable(this.brokenShopVariable);
    }
  }

  public bool isFixedShopInteractionActive
  {
    get
    {
      return (UnityEngine.Object) this.brokenShopInteraction != (UnityEngine.Object) null && DataManager.Instance.GetVariable(this.activeVariable) && DataManager.Instance.GetVariable(this.brokenShopVariable) && !DataManager.Instance.GetVariable(this.fixedShopVariable);
    }
  }

  public bool IsActive => DataManager.Instance.GetVariable(this.activeVariable);

  public bool IsRevealed
  {
    get => this.\u003CIsRevealed\u003Ek__BackingField;
    set => this.\u003CIsRevealed\u003Ek__BackingField = value;
  }

  public IReadOnlyList<Interaction_SimpleConversation> JobCompleteConvos
  {
    get => (IReadOnlyList<Interaction_SimpleConversation>) this.jobCompleteConvos;
  }

  public Interaction_SimpleConversation JobBoardCompleteConvo => this.jobBoardCompleteConvo;

  public void Awake()
  {
    this._ghostAI = this.GetComponent<UnitObject>();
    this._splineAnimate = this.GetComponent<SplineAnimate>();
    this._skeletonAnimation = this.GetComponentInChildren<SkeletonAnimation>();
    GhostNPC.GhostNPCs.Add(this);
  }

  public virtual void OnEnable()
  {
    if (this.hasShop)
      this.ConfigureBarks();
    if ((bool) (UnityEngine.Object) this.fixedShopInteraction)
      this.fixedShopInteraction.Callback.AddListener(new UnityAction(this.ConfigureBarks));
    if (this._isHome || !this._splineAnimate.IsPlaying)
      return;
    this.StartCoroutine((IEnumerator) this.WaitToReachHome(new System.Action(this.ReachedHome)));
  }

  public void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this.fixedShopInteraction)
      return;
    this.fixedShopInteraction.Callback.RemoveListener(new UnityAction(this.ConfigureBarks));
  }

  public void Start()
  {
    if (DataManager.Instance.GetVariable(this.activeVariable))
    {
      this.transform.position = (Vector3) this._splineAnimate.Container.EvaluatePosition(1f);
      this._skeletonAnimation.transform.localScale = this.endPointScale;
      this.gameObject.SetActive(true);
      this.rescued = true;
      if (!this.hasShop)
        this.GetComponent<CritterBee>().enabled = true;
      this._isHome = true;
    }
    else
      this.gameObject.SetActive(false);
    if (this.hasShop)
    {
      if (!this.isBrokenShopInteractionActive)
        this.brokenShopInteraction.gameObject.SetActive(false);
      if (!this.isFixedShopInteractionActive)
        this.fixedShopInteraction.gameObject.SetActive(false);
    }
    if (!this.IsRancher)
      return;
    if (DataManager.Instance.GetVariable(this.activeVariable))
    {
      if (!DataManager.Instance.RancherOnboardedLightningShards && ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToRancher) && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_CURSED) >= 2 || DataManager.Instance.RefinedElectrifiedRotstone))
      {
        this._skeletonAnimation.transform.localScale = Vector3.one;
        this._skeletonAnimation.AnimationState.SetAnimation(0, "farmer/farmer-wave", true);
        this.gotResourcesAltInteraction.gameObject.SetActive(true);
        this.gotResourcesAltInteraction.OnInteraction += (Interaction.InteractionEvent) (state =>
        {
          DataManager.Instance.RancherOnboardedLightningShards = true;
          DataManager.Instance.RancherOnboardedHolyYew = true;
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnToRancher);
        });
      }
      else if (!DataManager.Instance.RancherOnboardedLightningShards && ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToRancher))
      {
        this._skeletonAnimation.transform.localScale = Vector3.one;
        this._skeletonAnimation.AnimationState.SetAnimation(0, "farmer/farmer-wave", true);
        this.gotLightningShardsInteraction.gameObject.SetActive(true);
        this.gotLightningShardsInteraction.OnInteraction += (Interaction.InteractionEvent) (state =>
        {
          DataManager.Instance.RancherOnboardedLightningShards = true;
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnToRancher);
          ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Ranching", InventoryItem.ITEM_TYPE.YEW_CURSED, 2, targetLocation: FollowerLocation.Dungeon1_6), true, true);
        });
      }
      else
      {
        if (DataManager.Instance.RancherOnboardedHolyYew || !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToRancher))
          return;
        this._skeletonAnimation.transform.localScale = Vector3.one;
        this._skeletonAnimation.AnimationState.SetAnimation(0, "farmer/farmer-wave", true);
        this.gotResourcesInteraction.gameObject.SetActive(true);
        this.brokenShopInteraction.gameObject.SetActive(true);
        this.gotResourcesInteraction.OnInteraction += (Interaction.InteractionEvent) (state =>
        {
          DataManager.Instance.RancherOnboardedHolyYew = true;
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnToRancher);
        });
      }
    }
    else
      this.brokenShopInteraction.OnInteraction += (Interaction.InteractionEvent) (state => ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Ranching", InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 2, targetLocation: FollowerLocation.Dungeon1_5), true, true));
  }

  public void OnDestroy() => GhostNPC.GhostNPCs.Remove(this);

  public void ConfigureBarks()
  {
    bool variable = DataManager.Instance.GetVariable(this.fixedShopVariable);
    this.casualBark?.gameObject.SetActive(variable);
    this.repairBark?.gameObject.SetActive(!variable);
  }

  public IEnumerator Summon(Vector3 at)
  {
    GhostNPC ghostNpc = this;
    yield return (object) null;
    ghostNpc.gameObject.SetActive(true);
    ghostNpc._targetPos = ghostNpc.transform.position;
    ghostNpc.transform.position = at;
    ghostNpc.transform.DOScale(Vector3.one, 1f).From<Vector3, Vector3, VectorOptions>(Vector3.zero).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    ghostNpc.transform.DOLocalMoveZ(0.0f, 1f).From(1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    ++DataManager.Instance.NPCsRescued;
    ghostNpc._skeletonAnimation.AnimationState.SetAnimation(0, ghostNpc.enterAnimation, false);
    ghostNpc._skeletonAnimation.AnimationState.AddAnimation(0, ghostNpc.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(3.5f);
    if ((bool) (UnityEngine.Object) ghostNpc.rescueConversation)
    {
      ghostNpc.rescueConversation.Play();
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(ghostNpc.\u003CSummon\u003Eb__61_0));
    }
  }

  public void ReturnHome() => this.StartCoroutine((IEnumerator) this.ReturnHomeRoutine());

  public void SetGrumpyEmotion()
  {
    this._skeletonAnimation.AnimationState.SetAnimation(0, this.grumpyIdleAnimation, true);
  }

  public IEnumerator ReturnHomeRoutine()
  {
    GhostNPC ghostNpc = this;
    SimpleBark[] componentsInChildren1 = ghostNpc.GetComponentsInChildren<SimpleBark>(false);
    SimpleBarkRepeating[] componentsInChildren2 = ghostNpc.GetComponentsInChildren<SimpleBarkRepeating>(false);
    foreach (Component component in componentsInChildren1)
      component.gameObject.SetActive(false);
    foreach (Component component in componentsInChildren2)
      component.gameObject.SetActive(false);
    ghostNpc._splineAnimate.Play();
    ghostNpc._skeletonAnimation.AnimationState.SetAnimation(0, ghostNpc.moveAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/ghoststatue/ghost_move");
    yield return (object) ghostNpc.StartCoroutine((IEnumerator) ghostNpc.WaitToReachHome(new System.Action(ghostNpc.ReachedHome)));
  }

  public IEnumerator WaitToReachHome(System.Action onReachedHome)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    GhostNPC ghostNpc = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      onReachedHome();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) new Func<bool>(ghostNpc.\u003CWaitToReachHome\u003Eb__65_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ReachedHome()
  {
    SimpleBark[] componentsInChildren1 = this.GetComponentsInChildren<SimpleBark>(false);
    SimpleBarkRepeating[] componentsInChildren2 = this.GetComponentsInChildren<SimpleBarkRepeating>(false);
    foreach (Component component in componentsInChildren1)
      component.gameObject.SetActive(true);
    foreach (Component component in componentsInChildren2)
      component.gameObject.SetActive(true);
    this._skeletonAnimation.AnimationState.SetAnimation(0, this.idleAnimation, true);
    this._skeletonAnimation.transform.localScale = this.endPointScale;
    this.rescued = true;
    if (this.IsRancher)
    {
      this._skeletonAnimation.AnimationState.SetAnimation(0, "farmer/farmer-no", false);
      this._skeletonAnimation.AnimationState.AddAnimation(0, "farmer/farmer-idle", true, 0.0f);
    }
    else if (this.hasShop)
      this.brokenShopInteraction.gameObject.SetActive(true);
    else
      this.GetComponent<CritterBee>().enabled = true;
    this._isHome = true;
  }

  public void Update()
  {
    if (this._splineAnimate.IsPlaying)
      this.OrientAlongSpline();
    if (this.rescued)
    {
      PlayerFarming closestPlayer = PlayerFarming.GetClosestPlayer(this.transform.position);
      if ((double) Vector3.Distance(closestPlayer.transform.position, this.transform.position) < (double) this.lookAtDistance)
        this._skeletonAnimation.transform.localScale = new Vector3((double) this.transform.position.x > (double) closestPlayer.transform.position.x ? -1f : 1f, 1f, 1f);
    }
    this.UpdateRebuildForward();
  }

  public void UpdateRebuildForward()
  {
    if ((bool) (UnityEngine.Object) this.rebuildableShopInteractionForwarder && (bool) (UnityEngine.Object) this.rebuildableShop && this.rebuildableShop.Interactable && this.rebuildableShop.canRestore)
    {
      this.rebuildableShopInteractionForwarder.sLabelDefault = $"{string.Format(ScriptLocalization.Interactions.Repair, (object) "")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.YEW_HOLY, this.rebuildableShop.Cost)}";
      this.rebuildableShopInteractionForwarder.UpdateLocalisation();
      if (this.rebuildableShopInteractionForwarder.gameObject.activeSelf)
        return;
      this.rebuildableShopInteractionForwarder.gameObject.SetActive(true);
      if ((bool) (UnityEngine.Object) this.rebuildableShop.OutlineTarget)
        this.rebuildableShopInteractionForwarder.OutlineTarget = this.rebuildableShop.OutlineTarget;
      else
        this.rebuildableShopInteractionForwarder.OutlineTarget = this.rebuildableShop.gameObject;
      if (this.forwardInteractableSet)
        return;
      this.rebuildableShopInteractionForwarder.OnInteraction += (Interaction.InteractionEvent) (state =>
      {
        this.forwardInteractableSet = true;
        if (!((UnityEngine.Object) this.rebuildableShop != (UnityEngine.Object) null) || !this.rebuildableShop.Interactable || !this.rebuildableShop.canRestore)
          return;
        this.rebuildableShop.OnInteract(state);
        this.rebuildableShopInteractionForwarder.gameObject.SetActive(false);
      });
    }
    else
    {
      if (!((UnityEngine.Object) this.rebuildableShopInteractionForwarder != (UnityEngine.Object) null))
        return;
      this.rebuildableShopInteractionForwarder.gameObject.SetActive(false);
    }
  }

  public void OrientAlongSpline()
  {
    Vector3 tangent = (Vector3) this._splineAnimate.Container.EvaluateTangent(this._splineAnimate.NormalizedTime);
    float angle = Utils.GetAngle(this.transform.position, this.transform.position + tangent);
    this._ghostAI.state.facingAngle = angle;
    this._ghostAI.state.LookAngle = angle;
    this._skeletonAnimation.gameObject.transform.localScale = new Vector3(Mathf.Sign(tangent.x), 1f, 1f);
  }

  [CompilerGenerated]
  public bool \u003CSummon\u003Eb__61_0() => this.rescueConversation.Finished;

  [CompilerGenerated]
  public bool \u003CWaitToReachHome\u003Eb__65_0() => !this._splineAnimate.IsPlaying;

  [CompilerGenerated]
  public void \u003CUpdateRebuildForward\u003Eb__69_0(StateMachine state)
  {
    this.forwardInteractableSet = true;
    if (!((UnityEngine.Object) this.rebuildableShop != (UnityEngine.Object) null) || !this.rebuildableShop.Interactable || !this.rebuildableShop.canRestore)
      return;
    this.rebuildableShop.OnInteract(state);
    this.rebuildableShopInteractionForwarder.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CUpdateRebuildForward\u003Eb__69_1(StateMachine state)
  {
    if (!((UnityEngine.Object) this.rebuildableShop != (UnityEngine.Object) null) || !this.rebuildableShop.Interactable || !this.rebuildableShop.canRestore)
      return;
    this.rebuildableShop.OnInteract(state);
  }
}
