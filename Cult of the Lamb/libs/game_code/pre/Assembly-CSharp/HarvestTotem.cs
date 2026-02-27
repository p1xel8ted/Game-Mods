// Decompiled with JetBrains decompiler
// Type: HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HarvestTotem : Interaction
{
  public static float EFFECTIVE_DISTANCE = 7f;
  public static Vector3 Centre = new Vector3(0.0f, 0.0f);
  public SpriteRenderer RangeSprite;
  private static List<HarvestTotem> HarvestTotems = new List<HarvestTotem>();
  public GameObject ReceiveSoulPosition;
  private Structure Structure;
  public GameObject DevotionReady;
  [SerializeField]
  private SpriteXPBar XpBar;
  private string sString;
  private LayerMask playerMask;
  private GameObject Player;
  private bool Activating;
  private float Delay;
  public float DistanceToTriggerDeposits = 5f;
  private Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  private float DistanceRadius = 1f;
  private float Distance = 1f;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private bool distanceChanged;
  private Vector3 _updatePos;

  public Structures_HarvestTotem StructureBrain => this.Structure.Brain as Structures_HarvestTotem;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    HarvestTotem.HarvestTotems.Add(this);
    this.Structure = this.GetComponentInChildren<Structure>();
    DataManager.Instance.ShrineLevel = 1;
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    HarvestTotem.HarvestTotems.Remove(this);
  }

  private void Start()
  {
    this.RangeSprite.size = new Vector2(HarvestTotem.EFFECTIVE_DISTANCE, HarvestTotem.EFFECTIVE_DISTANCE);
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    if ((UnityEngine.Object) this.XpBar != (UnityEngine.Object) null)
      this.XpBar.gameObject.SetActive(false);
    this.ActivateDistance = 2f;
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    Utils.DrawCircleXY(this.transform.position + HarvestTotem.Centre, HarvestTotem.EFFECTIVE_DISTANCE, Color.green);
    Utils.DrawCircleXY(this.transform.position + HarvestTotem.Centre, 0.5f, Color.red);
  }

  private void OnStructuresPlaced()
  {
    this.UpdateBar();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  private void OnBrainAssigned()
  {
    if (this.Structure.Type == global::StructureBrain.TYPES.HARVEST_TOTEM)
      return;
    if ((double) this.StructureBrain.Data.LastPrayTime == -1.0)
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdateBar();
  }

  private new void OnDestroy()
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.StructureBrain == null)
      return;
    this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
  }

  public override void GetLabel()
  {
    if (this.Structure.Type == global::StructureBrain.TYPES.HARVEST_TOTEM)
    {
      this.Label = "";
    }
    else
    {
      this.Interactable = this.StructureBrain.SoulCount > 0;
      this.Label = $"{this.sString} {(GameManager.HasUnlockAvailable() ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">")} x{(object) this.StructureBrain.SoulCount}/{(object) this.StructureBrain.SoulMax}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.IndicateHighlighted();
    this.Activating = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void UpdateBar()
  {
    if ((UnityEngine.Object) this.XpBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XpBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
    if (!((UnityEngine.Object) this.DevotionReady != (UnityEngine.Object) null))
      return;
    this.DevotionReady.SetActive(this.StructureBrain.SoulCount > 0);
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating && this.StructureBrain.SoulCount > 0)
    {
      Debug.Log((object) ("souls count: " + (object) this.StructureBrain.SoulCount));
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      --this.StructureBrain.SoulCount;
      this.Delay = 0.2f;
      this.UpdateBar();
    }
    if (this.StructureBrain != null && (double) this.StructureBrain.Data.LastPrayTime != -1.0 && (double) TimeManager.TotalElapsedGameTime > (double) this.StructureBrain.Data.LastPrayTime && this.StructureBrain.SoulCount < this.StructureBrain.SoulMax)
    {
      Debug.Log((object) ("ADD to souls count: " + (object) this.StructureBrain.SoulCount));
      ++this.StructureBrain.SoulCount;
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    }
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = HarvestTotem.EFFECTIVE_DISTANCE;
    }
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.RangeSprite.gameObject.SetActive(true);
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }

  public override void IndicateHighlighted() => this.XpBar.gameObject.SetActive(true);

  public override void EndIndicateHighlighted() => this.XpBar.gameObject.SetActive(false);

  private void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
