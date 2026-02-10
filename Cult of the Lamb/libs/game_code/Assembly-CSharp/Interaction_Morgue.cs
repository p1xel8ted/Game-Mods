// Decompiled with JetBrains decompiler
// Type: Interaction_Morgue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Morgue : Interaction
{
  public static List<Interaction_Morgue> Morgues = new List<Interaction_Morgue>();
  public Structure Structure;
  public Structures_Morgue _StructureInfo;
  [SerializeField]
  public GameObject holePosition;
  [SerializeField]
  public ItemGauge itemGauge;
  [SerializeField]
  public GameObject full;
  [SerializeField]
  public GameObject hasBodiesIcon;
  [SerializeField]
  public GameObject[] bodies;
  public bool activated;
  public string _label;
  public int _bodyDirection;
  public List<Vector3> _bodyDirections = new List<Vector3>()
  {
    new Vector3(-1f, 0.0f, 0.0f),
    new Vector3(-1f, -1f, 0.0f),
    new Vector3(0.0f, -1f, 0.0f),
    new Vector3(1f, 0.0f, 0.0f),
    new Vector3(1f, -1f, 0.0f)
  };

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Morgue structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Morgue;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start()
  {
    this._bodyDirections.Shuffle<Vector3>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structureBrain != null)
      this.OnBrainAssigned();
    Interaction_Morgue.Morgues.Add(this);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.UpdateLocalisation();
    if (this.structureBrain == null)
      return;
    this.UpdateGauge();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_Morgue.Morgues.Remove(this);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structureBrain == null)
      return;
    this.structureBrain.OnBodyDeposited -= new Structures_Morgue.MorgueEvent(this.UpdateGauge);
    this.structureBrain.OnBodyWithdrawn -= new Structures_Morgue.MorgueEvent(this.UpdateGauge);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structureBrain.OnBodyDeposited += new Structures_Morgue.MorgueEvent(this.UpdateGauge);
    this.structureBrain.OnBodyWithdrawn += new Structures_Morgue.MorgueEvent(this.UpdateGauge);
    for (int index = this.structureBrain.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.GetDeadFollowerInfoByID(this.structureBrain.Data.MultipleFollowerIDs[index]) == null)
        this.structureBrain.Data.MultipleFollowerIDs.RemoveAt(index);
    }
    this.UpdateGauge();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this._label = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (LocalizeIntegration.IsArabic())
      this.Label = $"{this._label} ){LocalizeIntegration.ReverseText(this.structureBrain.DEAD_BODY_SLOTS.ToString())}/{LocalizeIntegration.ReverseText(this.structureBrain.Data.MultipleFollowerIDs.Count.ToString())}(";
    else
      this.Label = $"{this._label} ({this.structureBrain.Data.MultipleFollowerIDs.Count.ToString()}/{this.structureBrain.DEAD_BODY_SLOTS.ToString()})";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activated)
      return;
    this.activated = true;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    for (int index = this.structureBrain.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead_IDs.Contains(this.structureBrain.Data.MultipleFollowerIDs[index]))
        followerSelectEntries.Add(new FollowerSelectEntry(FollowerInfo.GetInfoByID(this.structureBrain.Data.MultipleFollowerIDs[index], true)));
      else
        this.structureBrain.Data.MultipleFollowerIDs.RemoveAt(index);
    }
    UIDeadFollowerSelectMenu followerSelectMenu = MonoSingleton<UIManager>.Instance.DeadFollowerSelectMenuTemplate.Instantiate<UIDeadFollowerSelectMenu>();
    followerSelectMenu.Show(followerSelectEntries.Count, this._StructureInfo.DEAD_BODY_SLOTS, followerSelectEntries);
    followerSelectMenu.OnFollowerSelected = followerSelectMenu.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo => this.StartCoroutine((IEnumerator) this.SpawnQueue(followerInfo)));
    followerSelectMenu.OnCancel = followerSelectMenu.OnCancel + (System.Action) (() => GameManager.GetInstance().WaitForSecondsRealtime(0.1f, (System.Action) (() =>
    {
      this.OnHidden();
      this.HasChanged = true;
    })));
  }

  public IEnumerator SpawnQueue(FollowerInfo followerInfo)
  {
    Interaction_Morgue interactionMorgue = this;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Time.timeScale = 1f;
    HUD_Manager.Instance.Hide(true);
    interactionMorgue.structureBrain.Data.MultipleFollowerIDs.Remove(followerInfo.ID);
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.Position = interactionMorgue.holePosition.transform.position;
    infoByType.FollowerID = followerInfo.ID;
    infoByType.BeenInMorgueAlready = true;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, infoByType.Position, Vector2Int.one, false, new System.Action<GameObject>(interactionMorgue.\u003CSpawnQueue\u003Eb__24_0));
    if (DataManager.Instance.TotalBodiesHarvested >= 10 && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.SpawnCombatFollowerFromBodies) && DataManager.Instance.OnboardedRelics)
    {
      bool waiting = true;
      RelicCustomTarget.Create(interactionMorgue.transform.position + Vector3.up * 0.5f, interactionMorgue.transform.position + Vector3.up * 0.5f - Vector3.forward, 1.5f, RelicType.SpawnCombatFollowerFromBodies, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
    }
    interactionMorgue.OnHidden();
    interactionMorgue.HasChanged = true;
    interactionMorgue.UpdateGauge();
  }

  public void OnHidden()
  {
    Time.timeScale = 1f;
    this.activated = false;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void UpdateGauge()
  {
    foreach (GameObject body in this.bodies)
      body.gameObject.SetActive(false);
    if (this.structureBrain.Data.MultipleFollowerIDs.Count > 0)
      this.bodies[this.structureBrain.Data.MultipleFollowerIDs.Count - 1].gameObject.SetActive(true);
    this.itemGauge.SetPosition((float) this.structureBrain.Data.MultipleFollowerIDs.Count / (float) this.structureBrain.DEAD_BODY_SLOTS);
    this.full.SetActive(this.structureBrain.Data.MultipleFollowerIDs.Count >= this.structureBrain.DEAD_BODY_SLOTS);
    this.hasBodiesIcon.gameObject.SetActive(this.structureBrain.Data.MultipleFollowerIDs.Count > 0 && !this.structureBrain.IsFull);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_0(FollowerInfo followerInfo)
  {
    this.StartCoroutine((IEnumerator) this.SpawnQueue(followerInfo));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_1()
  {
    GameManager.GetInstance().WaitForSecondsRealtime(0.1f, (System.Action) (() =>
    {
      this.OnHidden();
      this.HasChanged = true;
    }));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_2()
  {
    this.OnHidden();
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003CSpawnQueue\u003Eb__24_0(GameObject g)
  {
    DeadWorshipper component = g.GetComponent<DeadWorshipper>();
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/collect_dead_from_morgue", g.transform.position);
    Collider2D collider2D = Physics2D.OverlapCircle((Vector2) this.transform.position, 1.5f, LayerMask.GetMask("Island"));
    if ((bool) (UnityEngine.Object) collider2D)
      component.BounceOutFromPosition(10f, (collider2D.transform.position - this.transform.position).normalized);
    else
      component.BounceOutFromPosition(10f, this._bodyDirections[this._bodyDirection]);
    if (this._bodyDirection < this._bodyDirections.Count - 1)
      ++this._bodyDirection;
    else
      this._bodyDirection = 0;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
    if (tileAtWorldPosition == null)
      return;
    component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
  }
}
