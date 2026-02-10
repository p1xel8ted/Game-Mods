// Decompiled with JetBrains decompiler
// Type: Scarecrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Scarecrow : Interaction
{
  public Structures_Scarecrow _StructureInfo;
  public static Vector3 Centre = new Vector3(0.0f, 0.75f);
  public SpriteRenderer RangeSprite;
  public static List<Scarecrow> Scarecrows = new List<Scarecrow>();
  public Structure Structure;
  public LayerMask playerMask;
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public float DistanceRadius = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 _updatePos;
  public string sOpenTrap;
  public GameObject TrapOpen;
  public GameObject TrapShut;
  public bool InBirdRoutine;
  public GameObject Bird;

  public Structures_Scarecrow Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Scarecrow;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public static float EFFECTIVE_DISTANCE(StructureBrain.TYPES Type)
  {
    return Type == StructureBrain.TYPES.SCARECROW || Type != StructureBrain.TYPES.SCARECROW_2 ? 9f : 11f;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    Scarecrow.Scarecrows.Add(this);
    this.Structure = this.GetComponentInChildren<Structure>();
    this.RangeSprite.size = new Vector2(Scarecrow.EFFECTIVE_DISTANCE(this.Structure.Type), Scarecrow.EFFECTIVE_DISTANCE(this.Structure.Type));
    this.HasSecondaryInteraction = false;
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.HoldToInteract = true;
    if (!((UnityEngine.Object) this.Bird != (UnityEngine.Object) null))
      return;
    this.Bird.SetActive(false);
  }

  public void OnBrainAssigned()
  {
    if (this.Brain.HasBird)
      this.ShutTrap();
    this.Brain.OnCatchBird += new System.Action(this.CatchBird);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Scarecrow.Scarecrows.Remove(this);
    if ((bool) (UnityEngine.Object) this.Structure)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Brain == null)
      return;
    this.Brain.OnCatchBird -= new System.Action(this.CatchBird);
  }

  public override void GetLabel()
  {
    if (this.Brain == null)
      return;
    if (this.Brain.HasBird && !this.InBirdRoutine)
    {
      this.Interactable = true;
      this.Label = this.sOpenTrap;
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (!this.Brain.HasBird)
      return;
    base.OnInteract(state);
    this.OpenTrap();
    this.Brain.EmptyTrap();
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, UnityEngine.Random.Range(2, 5), this.transform.position);
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
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
        this.DistanceRadius = Scarecrow.EFFECTIVE_DISTANCE(this.Structure.Type);
      }
      if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
      {
        this.RangeSprite.gameObject.SetActive(true);
        this.RangeSprite.DOKill();
        this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = true;
      }
      else if (this.distanceChanged)
      {
        this.RangeSprite.DOKill();
        this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
        this.distanceChanged = false;
      }
    }
    if (this.Brain == null || !((UnityEngine.Object) this.TrapShut != (UnityEngine.Object) null) || this.Brain.HasBird || !this.TrapShut.gameObject.activeSelf)
      return;
    this.OpenTrap();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sOpenTrap = ScriptLocalization.Interactions.OpenTrap;
  }

  public void ShutTrap()
  {
    Debug.Log((object) "SET TRAP!");
    this.TrapOpen.SetActive(false);
    this.TrapShut.SetActive(true);
    this.TrapShut.transform.DOPunchScale(new Vector3(0.1f, 0.3f), 0.5f);
  }

  public void OpenTrap()
  {
    this.TrapOpen.SetActive(true);
    this.TrapShut.SetActive(false);
    this.TrapOpen.transform.DOPunchScale(new Vector3(0.3f, 0.1f), 0.5f);
  }

  public void CatchBird() => this.StartCoroutine((IEnumerator) this.BirdRoutine());

  public IEnumerator BirdRoutine()
  {
    this.InBirdRoutine = true;
    this.Bird.SetActive(true);
    this.Bird.GetComponentInChildren<Animator>().SetTrigger("FLY");
    float num1 = (float) UnityEngine.Random.Range(0, 360);
    this.Bird.transform.localScale = new Vector3((double) num1 >= 90.0 || (double) num1 <= -90.0 ? -1f : 1f, 1f, 1f);
    float num2 = (float) UnityEngine.Random.Range(8, 10);
    this.Bird.transform.localPosition = new Vector3(num2 * Mathf.Cos(num1 * ((float) Math.PI / 180f)), num2 * Mathf.Cos(num1 * ((float) Math.PI / 180f)), -10f);
    this.Bird.transform.DOLocalMove(new Vector3(0.0f, 0.25f), 2f);
    yield return (object) new WaitForSeconds(2f);
    this.ShutTrap();
    this.Bird.SetActive(false);
    this.InBirdRoutine = false;
  }
}
