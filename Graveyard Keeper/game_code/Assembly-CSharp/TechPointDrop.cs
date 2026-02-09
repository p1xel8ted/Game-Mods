// Decompiled with JetBrains decompiler
// Type: TechPointDrop
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TechPointDrop : MonoBehaviour
{
  public Rigidbody2D rigid_body;
  public RoundAndSortComponent round_and_sort;
  public Animator animator;
  public float _collect_delay = 1f;
  public float _cur_time;
  public Transform _tf;
  public Transform _player;
  public float collect_radius = 10f;
  public string _type;
  public bool _inited;
  public float magnet_force = 5f;
  public Collider2D _collider;
  public const bool FLY_TECHPOINTS = true;
  public static List<TechPointDrop> _all = new List<TechPointDrop>();

  public string type => this._type;

  public static List<TechPointDrop> all => TechPointDrop._all;

  public static TechPointDrop Spawn(TechPointDrop prefab, TechPointsSpawner.Type type)
  {
    TechPointDrop techPointDrop = prefab.Copy<TechPointDrop>(MainGame.me.world_root);
    techPointDrop.Init(TechDefinition.TECH_POINTS[(int) type]);
    techPointDrop.animator.SetInteger("color", (int) type);
    TechPointDrop._all.Add(techPointDrop);
    return techPointDrop;
  }

  public void Init(string type)
  {
    this.round_and_sort.enabled = true;
    this._collider = this.GetComponent<Collider2D>();
    this._collect_delay = 0.7f;
    this._tf = this.transform;
    this._player = MainGame.me.player.tf;
    this._type = type;
    this._inited = true;
  }

  public void Update()
  {
    if (!this._inited)
      return;
    this._cur_time += Time.deltaTime;
    if ((double) this._cur_time < (double) this._collect_delay)
      return;
    Vector2 vector2;
    try
    {
      vector2 = (Vector2) (this._player.position - this._tf.position);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      this._inited = false;
      return;
    }
    float sqrMagnitude = vector2.sqrMagnitude;
    if ((double) sqrMagnitude > 40000.0)
    {
      this._collider.isTrigger = false;
    }
    else
    {
      this._collider.isTrigger = true;
      if ((double) sqrMagnitude < (double) this.collect_radius * (double) this.collect_radius)
        this.Collect();
      else
        this.rigid_body.AddForce(vector2 * this.magnet_force * 1.2f);
    }
  }

  public void Collect()
  {
    this._inited = false;
    PlayerComponent p = MainGame.me.player_component;
    Color endValue = Color.black;
    this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    switch (this._type)
    {
      case "r":
        endValue = Color.red;
        break;
      case "g":
        endValue = Color.green;
        break;
      case "b":
        endValue = Color.blue;
        break;
    }
    GUIElements.me.hud.tech_points_bar.Show();
    DOTween.To((DOGetter<Color>) (() => p.player_additional_color), (DOSetter<Color>) (x => p.player_additional_color = x), endValue, 0.03f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => DOTween.To((DOGetter<Color>) (() => p.player_additional_color), (DOSetter<Color>) (x => p.player_additional_color = x), Color.black, 0.05f)));
    DebugDraw.DrawCross(TechPointsDrop.Drop(MainGame.me.player_pos, this._type).transform.position, 1f, Color.yellow, 2f);
    this.DestroyMe();
    Sounds.PlaySound("tech_pickup");
  }

  public void DestroyMe()
  {
    NGUITools.Destroy((UnityEngine.Object) this.gameObject);
    if (!TechPointDrop._all.Contains(this))
      return;
    TechPointDrop._all.Remove(this);
  }

  public static void DestroyAll()
  {
    foreach (Component component in TechPointDrop._all)
      NGUITools.Destroy((UnityEngine.Object) component.gameObject);
    TechPointDrop._all.Clear();
  }
}
