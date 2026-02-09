// Decompiled with JetBrains decompiler
// Type: WorldObjectPart
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldObjectPart : MonoBehaviour
{
  public int floor_line;
  [SerializeField]
  public WorldGameObject _parent;
  [SerializeField]
  public MovementCurve _curve_idle;
  [SerializeField]
  public MovementCurve _curve_movement;
  [SerializeField]
  public MovementCurve _curve_attack;
  [SerializeField]
  public MovementCurve _curve_jump;
  [SerializeField]
  public VisibilitySector[] _visibility_sectors;
  public bool _cached;
  public List<GameObject> variations;
  public bool variations_are_radiobutton = true;
  public bool variation_can_be_none = true;
  public List<GOsList> variations_2;
  public bool variations_2_are_radiobutton = true;
  public bool variation_2_can_be_none = true;
  public string skin_id = "";
  public Transform center;
  public SpriteRenderer carrying_item_sprite;

  public WorldGameObject parent
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._parent;
    }
  }

  public void OnEnable() => this.StartCoroutine(this.LateEnableCoroutine());

  public IEnumerator LateEnableCoroutine()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    this.LateEnable();
  }

  public virtual void LateEnable()
  {
    foreach (OptimizedCollider2D componentsInChild in this.gameObject.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
  }

  public void Cache()
  {
    this._parent = this.GetComponentInParent<WorldGameObject>();
    if ((Object) this._parent == (Object) null)
    {
      Transform parent1 = this.transform.parent;
      if ((Object) parent1 != (Object) null)
      {
        this._parent = parent1.GetComponent<WorldGameObject>();
        if ((Object) this._parent == (Object) null)
        {
          Transform parent2 = parent1.parent;
          if ((Object) parent2 != (Object) null)
            this._parent = parent2.GetComponent<WorldGameObject>();
        }
      }
    }
    this._cached = (Object) this._parent != (Object) null;
  }

  public VisibilitySector[] visibility_sectors
  {
    get
    {
      return this._visibility_sectors == null || this._visibility_sectors.Length == 0 ? this.CacheSectors() : this._visibility_sectors;
    }
  }

  public VisibilitySector[] CacheSectors()
  {
    this._visibility_sectors = this.GetComponentsInChildren<VisibilitySector>();
    if ((Object) this.parent != (Object) null)
    {
      foreach (VisibilitySector visibilitySector in this._visibility_sectors)
        visibilitySector.Init(this.parent.components.character);
    }
    return this._visibility_sectors;
  }

  public AnimationCurve GetCurve(CharAnimState anim_state)
  {
    switch (anim_state)
    {
      case CharAnimState.Walking:
        return this.GetCurve("movement");
      case CharAnimState.Idle:
        return this.GetCurve("idle");
      case CharAnimState.Attack:
        return this.GetCurve("attack");
      case CharAnimState.Jump:
        return this.GetCurve("jump");
      default:
        return (AnimationCurve) null;
    }
  }

  public AnimationCurve GetCurve(string curve_name)
  {
    MovementCurve movementCurve = (MovementCurve) null;
    switch (curve_name)
    {
      case "idle":
        movementCurve = this._curve_idle;
        break;
      case "movement":
        movementCurve = this._curve_movement;
        break;
      case "attack":
        movementCurve = this._curve_attack;
        break;
      case "jump":
        movementCurve = this._curve_jump;
        break;
    }
    return !((Object) movementCurve == (Object) null) ? movementCurve.curve : (AnimationCurve) null;
  }
}
