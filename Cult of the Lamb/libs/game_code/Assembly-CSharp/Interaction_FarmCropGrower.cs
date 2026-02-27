// Decompiled with JetBrains decompiler
// Type: Interaction_FarmCropGrower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_FarmCropGrower : Interaction_AddFuel
{
  public static System.Action<Interaction_FarmCropGrower> OnRotate;
  public Structures_FarmCropGrower _StructureInfo;
  public static Vector3 Centre = new Vector3(0.0f, 0.75f);
  public SpriteRenderer RangeSprite;
  public Structure Structure;
  [SerializeField]
  public GameObject directionSprite;
  public int frameIntervalOffset;
  public int updateInterval = 2;
  public float distanceRadius = 1f;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public PlayerStaticAnimationWhenClose playerStatic;
  public static float EFFECTIVE_DISTANCE = 4f;
  public static Vector3[] OFFSETS = new Vector3[4]
  {
    new Vector3(0.0f, -3.5f, 0.0f),
    new Vector3(3.5f, 0.0f, 0.0f),
    new Vector3(0.0f, 3.5f, 0.0f),
    new Vector3(-3.5f, 0.0f, 0.0f)
  };
  public static int[] DIRECTION = new int[4]
  {
    -90,
    0,
    90,
    -180
  };

  public Structures_FarmCropGrower Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FarmCropGrower;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.playerStatic = this.GetComponent<PlayerStaticAnimationWhenClose>();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.Structure = this.GetComponentInChildren<Structure>();
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = LocalizationManager.GetTranslation("Interactions/Rotate");
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.Structure.Brain.Data.Rotation = (int) Mathf.Repeat((float) (this.Structure.Brain.Data.Rotation + 1), 4f);
    System.Action<Interaction_FarmCropGrower> onRotate = Interaction_FarmCropGrower.OnRotate;
    if (onRotate == null)
      return;
    onRotate(this);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.structure != (UnityEngine.Object) null && this.structure.Brain != null)
      this.structure.Brain.Data.Direction = 1;
    if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null && (UnityEngine.Object) this.GetComponentInParent<PlacementObject>() != (UnityEngine.Object) null)
    {
      this.transform.parent.localScale = Vector3.one;
      this.RangeSprite.transform.localPosition = Interaction_FarmCropGrower.OFFSETS[PlacementRegion.Instance.Rotation];
      this.directionSprite.transform.eulerAngles = new Vector3(-60f, 0.0f, (float) Interaction_FarmCropGrower.DIRECTION[PlacementRegion.Instance.Rotation]);
      this.GetComponentInParent<PlacementStructureHighlighter>().PositionOffset = Interaction_FarmCropGrower.OFFSETS[PlacementRegion.Instance.Rotation];
    }
    else if (this.structure.Brain != null)
    {
      this.RangeSprite.transform.localPosition = Interaction_FarmCropGrower.OFFSETS[this.structure.Brain.Data.Rotation];
      this.directionSprite.transform.eulerAngles = new Vector3(-60f, 0.0f, (float) Interaction_FarmCropGrower.DIRECTION[this.structure.Brain.Data.Rotation]);
    }
    if ((Time.frameCount + this.frameIntervalOffset) % this.updateInterval == 0)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
        return;
      if (!GameManager.overridePlayerPosition)
      {
        this._updatePos = PlayerFarming.Instance.transform.position;
        this.distanceRadius = 1f;
      }
      else
        this._updatePos = PlacementRegion.Instance.PlacementPosition;
      if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.distanceRadius)
      {
        this.RangeSprite.gameObject.SetActive(true);
        this.RangeSprite.DOKill();
        this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = true;
      }
      else if (this.distanceChanged)
      {
        this.RangeSprite.DOKill();
        this.RangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = false;
      }
    }
    this.playerStatic.Active = (UnityEngine.Object) this.Structure != (UnityEngine.Object) null && this.Structure.Brain != null && this.structure.Brain.Data.Fuel > 0;
  }
}
