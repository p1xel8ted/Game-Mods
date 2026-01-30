// Decompiled with JetBrains decompiler
// Type: Interaction_Crypt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UI.Menus.CryptMenu;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Crypt : Interaction
{
  public static List<Interaction_Crypt> Crypts = new List<Interaction_Crypt>();
  public Structure Structure;
  public Structures_Crypt _StructureInfo;
  public string _label;
  public bool activated;
  [SerializeField]
  public GameObject doorOpen;
  [SerializeField]
  public GameObject doorClosed;
  [SerializeField]
  public GameObject holePosition;
  [SerializeField]
  public ItemGauge ItemGauge;
  [SerializeField]
  public GameObject flowers;
  public SpriteXPBar XPBar;
  public bool Activating;
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

  public Structures_Crypt structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Crypt;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    this._bodyDirections.Shuffle<Vector3>();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structureBrain == null)
      return;
    Structures_Crypt structureBrain = this.structureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.XPBar.gameObject.SetActive(false);
    Interaction_Crypt.Crypts.Add(this);
    this.UpdateLocalisation();
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdateStructure);
    if (this._StructureInfo != null && this._StructureInfo.FollowersFuneralCount() > 0)
    {
      if (this.structureBrain != null && this.structureBrain.Data != null && (double) this.structureBrain.Data.LastPrayTime == -1.0)
      {
        this.structureBrain.SoulCount = this.structureBrain.SoulMax;
        this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
      }
      this.UpdateBar();
    }
    this.UpdateGauge();
  }

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.structureBrain == null)
      return;
    this.XPBar.gameObject.SetActive(true);
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.structureBrain.SoulCount / (float) this.structureBrain.SoulMax, 0.0f, 1f));
  }

  public void UpdateGauge()
  {
    if (this.structureBrain == null || this.structureBrain.Data == null || this._StructureInfo == null)
      return;
    this.ItemGauge.SetPosition((float) this.structureBrain.Data.MultipleFollowerIDs.Count / (float) this._StructureInfo.DEAD_BODY_SLOTS);
  }

  public void OnBrainAssigned()
  {
    this.UpdateGauge();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    for (int index = this.structureBrain.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (FollowerManager.GetDeadFollowerInfoByID(this.structureBrain.Data.MultipleFollowerIDs[index]) == null)
        this.structureBrain.Data.MultipleFollowerIDs.RemoveAt(index);
    }
    if (this._StructureInfo != null && this._StructureInfo.FollowersFuneralCount() > 0)
    {
      Structures_Crypt structureBrain = this.structureBrain;
      structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
      if ((double) this.structureBrain.Data.LastPrayTime == -1.0)
      {
        this.structureBrain.SoulCount = this.structureBrain.SoulMax;
        this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
      }
      this.UpdateBar();
      this.UpdateStructure();
    }
    this.SetGameObjects();
  }

  public void UpdateStructure()
  {
    if (this.structureBrain == null || (double) this.structureBrain.Data.LastPrayTime == -1.0 || (double) TimeManager.TotalElapsedGameTime <= (double) this.structureBrain.Data.LastPrayTime || this.structureBrain.SoulCount >= this.structureBrain.SoulMax)
      return;
    this.HasChanged = true;
    this.structureBrain.SoulCount = Mathf.Clamp(this.structureBrain.SoulCount + (1 + Mathf.FloorToInt((TimeManager.TotalElapsedGameTime - this.structureBrain.Data.LastPrayTime) / this.structureBrain.TimeBetweenSouls)), 0, this.structureBrain.SoulMax);
    this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void SetGameObjects()
  {
    if (this.StructureInfo == null)
      return;
    bool flag = false;
    foreach (int multipleFollowerId in this.structureBrain.Data.MultipleFollowerIDs)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(multipleFollowerId, true);
      if (infoById != null && infoById.HadFuneral)
        flag = true;
    }
    this.XPBar.gameObject.SetActive(false);
    if (flag)
    {
      this.XPBar.gameObject.SetActive(true);
      if (!this.flowers.activeSelf)
        this.flowers.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
      this.flowers.SetActive(true);
    }
    else
      this.flowers.SetActive(false);
  }

  public override void OnDisableInteraction()
  {
    Interaction_Crypt.Crypts.Remove(this);
    base.OnDisableInteraction();
    TimeManager.OnNewPhaseStarted -= new System.Action(this.UpdateStructure);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this._label = ScriptLocalization.Interactions.Look;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.SecondaryInteractable = false;
    this.HasSecondaryInteraction = false;
    this.SecondaryLabel = "";
    if (this.Activating)
      this.Label = string.Empty;
    else if (this.structureBrain != null && this.structureBrain.Data != null && this.structureBrain.Data.MultipleFollowerIDs.Count <= 0)
    {
      this.Interactable = false;
      this.Label = LocalizationManager.GetTranslation("Interactions/Crypt/BuryFollowers");
    }
    else
    {
      this.Interactable = true;
      string str1 = LocalizeIntegration.ReverseText(this.structureBrain.Data.MultipleFollowerIDs.Count.ToString());
      int num = this.structureBrain.DEAD_BODY_SLOTS;
      string str2 = LocalizeIntegration.ReverseText(num.ToString());
      if (LocalizeIntegration.IsArabic())
        this.Label = $"{this._label} ){str2}/{str1}(";
      else
        this.Label = $"{this._label} ({str1}/{str2})";
      if (this._StructureInfo.FollowersFuneralCount() <= 0)
        return;
      this.XPBar.gameObject.SetActive(true);
      string str3 = (GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      string receiveDevotion = ScriptLocalization.Interactions.ReceiveDevotion;
      if (LocalizeIntegration.IsArabic())
      {
        string[] strArray = new string[8]
        {
          receiveDevotion,
          " ",
          str3,
          " ",
          null,
          null,
          null,
          null
        };
        num = this.structureBrain.SoulMax;
        strArray[4] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[5] = " / ";
        num = this._StructureInfo.SoulCount;
        strArray[6] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[7] = StaticColors.GreyColorHex;
        this.SecondaryLabel = string.Concat(strArray);
      }
      else
      {
        string[] strArray = new string[8]
        {
          receiveDevotion,
          " ",
          str3,
          " ",
          null,
          null,
          null,
          null
        };
        num = this._StructureInfo.SoulCount;
        strArray[4] = num.ToString();
        strArray[5] = StaticColors.GreyColorHex;
        strArray[6] = " / ";
        num = this.structureBrain.SoulMax;
        strArray[7] = num.ToString();
        this.SecondaryLabel = string.Concat(strArray);
      }
      this.SecondaryInteractable = true;
      this.HasSecondaryInteraction = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activated)
      return;
    this.SetDoors(true);
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
    UICryptMenuController cryptMenuController = MonoSingleton<UIManager>.Instance.CryptMenuTemplate.Instantiate<UICryptMenuController>();
    cryptMenuController.Show(followerSelectEntries);
    cryptMenuController.Configure(this);
    cryptMenuController.OnFollowerSelected = cryptMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
      this.StructureInfo.MultipleFollowerIDs.Remove(followerInfo.ID);
      this.SpawnBody(followerInfo);
      this.activated = false;
      this.HasChanged = true;
    });
    cryptMenuController.OnHidden = cryptMenuController.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      this.activated = false;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
      this.SetDoors(false);
      this.HasChanged = true;
    });
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (this.structureBrain.SoulCount > 0)
    {
      if (this.Activating)
        return;
      this.StartCoroutine((IEnumerator) this.GiveReward());
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetDoors(bool open)
  {
    this.doorOpen.SetActive(open);
    this.doorClosed.SetActive(!open);
    this.UpdateGauge();
    this.SetGameObjects();
  }

  public void SpawnBody(FollowerInfo body)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.Position = this.holePosition.transform.position;
    infoByType.BodyWrapped = true;
    infoByType.FollowerID = body.ID;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.holePosition.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      component.WrapBody();
      Collider2D collider2D = Physics2D.OverlapCircle((Vector2) this.transform.position, 2f, LayerMask.GetMask("Island"));
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
    }));
    this.SetGameObjects();
  }

  public IEnumerator GiveReward()
  {
    Interaction_Crypt interactionCrypt = this;
    Debug.Log((object) ("_StructureInfo.SoulCount: " + interactionCrypt._StructureInfo.SoulCount.ToString().Colour(Color.yellow)));
    interactionCrypt.Activating = true;
    for (int i = 0; i < interactionCrypt._StructureInfo.SoulCount; ++i)
    {
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(interactionCrypt.playerFarming.gameObject, interactionCrypt.transform.position, Color.white, new System.Action(interactionCrypt.\u003CGiveReward\u003Eb__36_0));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionCrypt.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      float num = Mathf.Clamp((float) (interactionCrypt._StructureInfo.SoulCount - i) / (float) interactionCrypt.structureBrain.SoulMax, 0.0f, 1f);
      interactionCrypt.XPBar.UpdateBar(num);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionCrypt._StructureInfo.SoulCount = 0;
    interactionCrypt.XPBar.UpdateBar(0.0f);
    interactionCrypt.Activating = false;
    interactionCrypt.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__31_0(FollowerInfo followerInfo)
  {
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
    this.StructureInfo.MultipleFollowerIDs.Remove(followerInfo.ID);
    this.SpawnBody(followerInfo);
    this.activated = false;
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__31_1()
  {
    Time.timeScale = 1f;
    this.activated = false;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
    this.SetDoors(false);
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003CSpawnBody\u003Eb__35_0(GameObject g)
  {
    DeadWorshipper component = g.GetComponent<DeadWorshipper>();
    component.WrapBody();
    Collider2D collider2D = Physics2D.OverlapCircle((Vector2) this.transform.position, 2f, LayerMask.GetMask("Island"));
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

  [CompilerGenerated]
  public void \u003CGiveReward\u003Eb__36_0() => this.playerFarming.GetSoul(1);
}
