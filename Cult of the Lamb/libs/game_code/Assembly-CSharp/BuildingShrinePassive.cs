// Decompiled with JetBrains decompiler
// Type: BuildingShrinePassive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Interaction_AddFuel addFuel;
  [Space]
  [SerializeField]
  public GameObject[] spawnPositions;
  [SerializeField]
  public SpriteRenderer rangeCircle;
  [SerializeField]
  public SpriteXPBar XpBar;
  public string sString;
  [SerializeField]
  public GameObject shrineEyes;
  public Coroutine cShowRangeSprite;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public GameObject Player;
  public bool Activating;
  public float Delay;
  public float Distance;
  public bool InRange;
  public float RangeDistance = 100f;
  public float DistanceToTriggerDeposits = 5f;
  public float DistanceRadius = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;

  public Structures_Shrine_Passive StructureBrain
  {
    get => this.Structure.Brain as Structures_Shrine_Passive;
  }

  public GameObject[] SpawnPositions => this.spawnPositions;

  public void Start()
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
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.addFuel.OnFuelModified += new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
  }

  public void OnUpgradeUnlocked(UpgradeSystem.Type upgradeType)
  {
    this.addFuel.gameObject.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesFlames));
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_Shrine_Passive structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
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

  public void OnStructuresPlaced()
  {
    this.UpdateBar();
    DataManager.Instance.ShrineLevel = 1;
  }

  public void OnFuelModified(float fuel)
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
    {
      Structures_Shrine_Passive structureBrain = this.StructureBrain;
      structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
    }
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.OnUpgradeUnlocked);
    this.addFuel.OnFuelModified -= new Interaction_AddFuel.FuelEvent(this.OnFuelModified);
  }

  public override void GetLabel()
  {
    this.Interactable = this.StructureBrain.SoulCount > 0;
    string str1 = (GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
    string str2 = LocalizeIntegration.ReverseText(this.StructureBrain.SoulCount.ToString());
    string str3 = LocalizeIntegration.ReverseText(this.StructureBrain.SoulMax.ToString());
    if (LocalizeIntegration.IsArabic())
      this.Label = $"{this.sString} {str1} {str3} / {str2}{StaticColors.GreyColorHex}";
    else
      this.Label = $"{this.sString} {str1} {str2}{StaticColors.GreyColorHex} / {str3}";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.IndicateHighlighted(this.playerFarming);
    this.Activating = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
  {
    if (this.StructureBrain == null)
      return;
    float num = Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f);
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
    {
      if (this.StructureBrain.SoulCount == this.StructureBrain.SoulMax)
        this.shrineEyes.SetActive(true);
      else
        this.shrineEyes.SetActive(false);
    }
    if (!((UnityEngine.Object) this.XpBar != (UnityEngine.Object) null))
      return;
    this.XpBar.UpdateBar(num);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
  }

  public override void Update()
  {
    base.Update();
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval == 0)
    {
      if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
        return;
      if (!GameManager.overridePlayerPosition)
      {
        this._updatePos = this.playerFarming.transform.position;
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
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
      SoulCustomTarget.Create(this.state.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.StructureBrain.SoulCount;
    this.UpdateBar();
    this.Delay = 0.1f;
  }

  public void GivePlayerSoul()
  {
    this.UpdateBar();
    this.playerFarming.GetSoul(1);
  }
}
