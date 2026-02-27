// Decompiled with JetBrains decompiler
// Type: FarmStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FarmStation : Interaction
{
  public static Vector3 Centre = new Vector3(0.0f, 0.0f);
  public SpriteRenderer RangeSprite;
  public static List<FarmStation> FarmStations = new List<FarmStation>();
  public Structure Structure;
  private Structures_FarmerStation _StructureInfo;
  public GameObject WorshipperPosition;
  private LayerMask playerMask;
  private string sRequireLevel2;
  private string sMoreActions;
  private Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  private float DistanceRadius = 1f;
  private float Distance = 1f;
  private int FrameIntervalOffset;
  private int UpdateInterval = 2;
  private bool distanceChanged;
  private Vector3 _updatePos;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_FarmerStation StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FarmerStation;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void Start()
  {
    this.UpdateLocalisation();
    this.Interactable = false;
    this.HasSecondaryInteraction = true;
    this.RangeSprite.size = new Vector2(6f, 6f);
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
    this.FrameIntervalOffset = Random.Range(0, this.UpdateInterval);
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void OnEnableInteraction() => base.OnEnableInteraction();

  protected override void OnEnable()
  {
    base.OnEnable();
    FarmStation.FarmStations.Add(this);
    if (!((Object) this.GetComponentInParent<PlacementObject>() == (Object) null))
      return;
    this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    FarmStation.FarmStations.Remove(this);
  }

  public override void GetLabel()
  {
    this.Label = "";
    this.Interactable = false;
  }

  public override void GetSecondaryLabel()
  {
    this.SecondaryLabel = "";
    this.SecondaryInteractable = false;
  }

  protected override void Update()
  {
    base.Update();
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (Object) PlayerFarming.Instance == (Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = PlayerFarming.Instance.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = 6f;
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

  public override void OnSecondaryInteract(StateMachine state) => base.OnSecondaryInteract(state);
}
