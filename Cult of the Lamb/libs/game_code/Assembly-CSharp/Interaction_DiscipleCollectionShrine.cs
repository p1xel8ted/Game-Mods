// Decompiled with JetBrains decompiler
// Type: Interaction_DiscipleCollectionShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DiscipleCollectionShrine : Interaction
{
  public static List<Interaction_DiscipleCollectionShrine> Shrines = new List<Interaction_DiscipleCollectionShrine>();
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  public Structures_Shrine_Disciple_Collection StructureBrain;
  [SerializeField]
  public SpriteRenderer rangeCircle;
  [SerializeField]
  public SpriteXPBar XpBar;
  public string sString;
  [SerializeField]
  public GameObject shrineEyes;
  public Coroutine cShowRangeSprite;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public bool _hasUnlockAvailable;
  public int _lastSoulCount = -1;
  public bool activating;
  public float Delay;
  public float Distance;
  public float RangeDistance = 100f;
  public float DistanceToTriggerDeposits = 5f;
  public float DistanceRadius = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public float AccelerateCollection;
  public float AccelerateDelta;

  public void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
  }

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
      this.shrineEyes.SetActive(false);
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.rangeCircle.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    base.OnEnableInteraction();
    Interaction_DiscipleCollectionShrine.Shrines.Add(this);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    this.StructureBrain = this.Structure.Brain as Structures_Shrine_Disciple_Collection;
    Structures_Shrine_Disciple_Collection structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
    this.RangeDistance = Structures_Shrine_Disciple_Collection.Range;
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

  public void OnStructuresPlaced() => this.UpdateBar();

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_DiscipleCollectionShrine.Shrines.Remove(this);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    if (this.StructureBrain == null)
      return;
    Structures_Shrine_Disciple_Collection structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public override void GetLabel()
  {
    this.Interactable = this.StructureBrain.SoulCount > 0;
    if (this.StructureBrain.SoulCount != this._lastSoulCount)
      this._hasUnlockAvailable = GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten;
    string str = this._hasUnlockAvailable ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
    if (LocalizeIntegration.IsArabic())
      this.Label = $"{this.sString} {str} {LocalizeIntegration.ReverseText(this.StructureBrain.SoulMax.ToString())} / {LocalizeIntegration.ReverseText(this.StructureBrain.SoulCount.ToString())}{StaticColors.GreyColorHex}";
    else
      this.Label = $"{this.sString} {str} {this.StructureBrain.SoulCount.ToString()}{StaticColors.GreyColorHex} / {this.StructureBrain.SoulMax.ToString()}";
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

  public bool Activating
  {
    get => this.activating;
    set
    {
      if (this.activating != value)
      {
        this.AccelerateCollection = 0.0f;
        this.AccelerateDelta = 0.0f;
      }
      this.activating = value;
    }
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
        this.DistanceRadius = Structures_Shrine_Disciple_Collection.Range;
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
    this.Delay = 0.1f - this.AccelerateCollection;
    Debug.Log((object) ("AccelerateCollection: " + this.AccelerateCollection.ToString().Colour(Color.red)));
    this.AccelerateCollection = Mathf.Lerp(this.AccelerateCollection, 0.09f, this.AccelerateDelta += Time.deltaTime * 0.5f);
  }

  public void GivePlayerSoul()
  {
    this.UpdateBar();
    this.playerFarming.GetSoul(1);
  }
}
