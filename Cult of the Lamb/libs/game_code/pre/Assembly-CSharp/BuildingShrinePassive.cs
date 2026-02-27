// Decompiled with JetBrains decompiler
// Type: BuildingShrinePassive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuildingShrinePassive : Interaction
{
  public static List<BuildingShrinePassive> Shrines = new List<BuildingShrinePassive>();
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  [SerializeField]
  private Interaction_AddFuel addFuel;
  [Space]
  [SerializeField]
  private GameObject[] spawnPositions;
  [SerializeField]
  private SpriteRenderer rangeCircle;
  [SerializeField]
  private SpriteXPBar XpBar;
  private string sString;
  [SerializeField]
  private GameObject shrineEyes;
  private Coroutine cShowRangeSprite;
  private Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  private GameObject Player;
  private bool Activating;
  private float Delay;
  private float Distance;
  private bool InRange;
  private float RangeDistance = 100f;
  public float DistanceToTriggerDeposits = 5f;
  private float DistanceRadius = 1f;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private bool distanceChanged;
  private Vector3 _updatePos;

  public Structures_Shrine_Passive StructureBrain
  {
    get => this.Structure.Brain as Structures_Shrine_Passive;
  }

  public GameObject[] SpawnPositions => this.spawnPositions;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    foreach (GameObject spawnPosition in this.spawnPositions)
      spawnPosition.SetActive(false);
  }

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
      this.shrineEyes.SetActive(false);
    DataManager.Instance.ShrineLevel = 1;
    this.rangeCircle.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    base.OnEnableInteraction();
    BuildingShrinePassive.Shrines.Add(this);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    this.addFuel.OnFuelModified += new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
  }

  private void OnUpgradeUnlocked(UpgradeSystem.Type upgradeType)
  {
    this.addFuel.gameObject.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesFlames));
  }

  private void OnBrainAssigned()
  {
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
    this.OnUpgradeUnlocked(UpgradeSystem.Type.Ability_Eat);
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    this.RangeDistance = Structures_Shrine_Passive.Range(this.StructureBrain.Data.Type);
    this.rangeCircle.size = new Vector2(this.RangeDistance, this.RangeDistance);
  }

  public IEnumerator ShowRangeSprite(Color TargetColor, bool Disable)
  {
    this.rangeCircle.gameObject.SetActive(true);
    this.rangeCircle.enabled = true;
    float Progress = 0.0f;
    float Duration = 0.3f;
    Color StartColor = this.rangeCircle.color;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.rangeCircle.color = Color.Lerp(StartColor, TargetColor, Progress / Duration);
      yield return (object) null;
    }
    this.rangeCircle.color = TargetColor;
    if (Disable)
      this.rangeCircle.enabled = false;
  }

  private void OnStructuresPlaced()
  {
    this.UpdateBar();
    DataManager.Instance.ShrineLevel = 1;
  }

  private void OnFuelModified(float fuel)
  {
    if (this.Structure.Structure_Info.FullyFueled)
      this.addFuel.Interactable = false;
    else
      this.addFuel.Interactable = true;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    BuildingShrinePassive.Shrines.Remove(this);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    if (this.StructureBrain != null)
      this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    this.addFuel.OnFuelModified -= new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
  }

  public override void GetLabel()
  {
    this.Interactable = this.StructureBrain.SoulCount > 0;
    this.Label = $"{this.sString} {(GameManager.HasUnlockAvailable() ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">")} {(object) this.StructureBrain.SoulCount}{StaticColors.GreyColorHex} / {(object) this.StructureBrain.SoulMax}";
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
    float num = Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f);
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
    {
      if (this.StructureBrain.SoulCount == this.StructureBrain.SoulMax)
        this.shrineEyes.SetActive(true);
      else
        this.shrineEyes.SetActive(false);
    }
    this.XpBar.UpdateBar(num);
  }

  public override void OnBecomeCurrent() => base.OnBecomeCurrent();

  public override void OnBecomeNotCurrent() => base.OnBecomeNotCurrent();

  private new void Update()
  {
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval == 0)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
        return;
      if (!GameManager.overridePlayerPosition)
      {
        this._updatePos = PlayerFarming.Instance.transform.position;
        this.DistanceRadius = 1f;
      }
      else
      {
        this._updatePos = PlacementRegion.Instance.PlacementPosition;
        this.DistanceRadius = Structures_Shrine_Passive.Range(this.StructureBrain.Data.Type);
      }
      if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
      {
        this.rangeCircle.gameObject.SetActive(true);
        this.rangeCircle.DOKill();
        this.rangeCircle.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = true;
      }
      else if (this.distanceChanged)
      {
        this.rangeCircle.DOKill();
        this.rangeCircle.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = false;
      }
    }
    this.GetLabel();
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if (GameManager.HasUnlockAvailable())
      SoulCustomTarget.Create(this.state.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.StructureBrain.SoulCount;
    this.UpdateBar();
    this.Delay = 0.1f;
  }

  private void GivePlayerSoul()
  {
    this.UpdateBar();
    PlayerFarming.Instance.GetSoul(1);
  }
}
