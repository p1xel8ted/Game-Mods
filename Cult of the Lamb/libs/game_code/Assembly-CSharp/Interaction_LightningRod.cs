// Decompiled with JetBrains decompiler
// Type: Interaction_LightningRod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class Interaction_LightningRod : Interaction
{
  public Structures_LightningRod _StructureInfo;
  public static Vector3 Centre = new Vector3(0.0f, 0.75f);
  public SpriteRenderer RangeSprite;
  public Structure Structure;
  public int frameIntervalOffset;
  public int updateInterval = 2;
  public float distanceRadius = 1f;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public static float EFFECTIVE_DISTANCE_LVL1 = 30f;
  public static float EFFECTIVE_DISTANCE_LVL2 = 45f;

  public Structures_LightningRod Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_LightningRod;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((Object) this.GetComponentInParent<PlacementObject>() == (Object) null)
      this.RangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.Structure = this.GetComponentInChildren<Structure>();
  }

  public override void Update()
  {
    base.Update();
    if ((Time.frameCount + this.frameIntervalOffset) % this.updateInterval != 0 || (Object) PlayerFarming.Instance == (Object) null)
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
    else
    {
      if (!this.distanceChanged)
        return;
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(new Color(1f, 1f, 1f, 0.0f), 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }
}
