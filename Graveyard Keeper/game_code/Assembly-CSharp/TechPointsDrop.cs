// Decompiled with JetBrains decompiler
// Type: TechPointsDrop
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TechPointsDrop : MonoBehaviour
{
  public static List<TechPointsDrop> _drops = new List<TechPointsDrop>();
  public string _tech_branch_letter = "";
  public Tweener _tweener;
  public bool _flying_to_trash_can;
  public UILabel label;
  public float _delay;
  public System.Action _on_reached_destination;

  public static void Drop(Vector3 pos, int r, int g, int b)
  {
    TechPointsSpawner techPointsSpawner = GUIElements.me.tech_points_spawner.Copy<TechPointsSpawner>(MainGame.me.world_root);
    techPointsSpawner.transform.position = pos;
    techPointsSpawner.Spawn(r, g, b);
  }

  public static void Drop(Vector3 pos, Item item)
  {
    switch (item.id)
    {
      case "r":
        TechPointsDrop.Drop(pos, item.value, 0, 0);
        break;
      case "g":
        TechPointsDrop.Drop(pos, 0, item.value, 0);
        break;
      case "b":
        TechPointsDrop.Drop(pos, 0, 0, item.value);
        break;
      default:
        Debug.LogError((object) ("Wrong techpoint letter = " + item.id));
        break;
    }
  }

  public static TechPointsDrop Drop(
    Vector3 pos,
    string tech_branch_letter,
    System.Action on_reached_destinaion = null)
  {
    TechPointsDrop techPointsDrop = Prefabs.me.tech_points_drop.Copy<TechPointsDrop>();
    techPointsDrop._on_reached_destination = on_reached_destinaion;
    TechPointsDrop._drops.Add(techPointsDrop);
    techPointsDrop.Draw(tech_branch_letter);
    techPointsDrop.gameObject.SetActive(true);
    techPointsDrop._flying_to_trash_can = (double) UnityEngine.Random.Range(0, 100) < (double) PlayerComponent.GetTechPointsLoseChance() * 100.0;
    Vector2 to = GUIElements.me.hud.tech_points_bar.GetTechPointsCounterPosition(tech_branch_letter);
    if (techPointsDrop._flying_to_trash_can)
    {
      GUIElements.me.hud.tech_trash_can.Show();
      to = (Vector2) GUIElements.me.hud.tech_trash_can.pos.transform.position;
    }
    else
      GUIElements.me.hud.tech_points_bar.Show();
    techPointsDrop.transform.SetGUIPosToWorldPos(pos, MainGame.me.world_cam, MainGame.me.gui_cam);
    techPointsDrop.transform.position += new Vector3(0.0f, 100f);
    techPointsDrop.label.text = $"({tech_branch_letter})";
    techPointsDrop._delay = UnityEngine.Random.Range(0.0f, 0.2f);
    techPointsDrop.StartFly(to);
    return techPointsDrop;
  }

  public static void DestroyAllTechspointsBeforeGameExit()
  {
    foreach (Component drop in TechPointsDrop._drops)
      UnityEngine.Object.Destroy((UnityEngine.Object) drop.gameObject);
    TechPointsDrop._drops.Clear();
  }

  public void Draw(string tech_branch_letter) => this._tech_branch_letter = tech_branch_letter;

  public void StartFly(Vector2 to)
  {
    this._tweener = this.transform.DOMove((Vector3) to, 1f + this._delay).OnComplete<Tweener>((TweenCallback) (() =>
    {
      this._tweener = (Tweener) null;
      this.OnReachedDestination();
    })).SetEase<Tweener>(Ease.OutCubic);
  }

  public void OnReachedDestination(bool redraw_hud = true)
  {
    if (this._tweener != null)
      this._tweener.Kill();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    if (!this._flying_to_trash_can)
    {
      MainGame.me.player.AddToParams(this._tech_branch_letter, 1f);
      MainGame.me.save.achievements.CheckKeyQuests("tech_collect_" + this._tech_branch_letter);
    }
    if (redraw_hud && (UnityEngine.Object) GUIElements.me.hud.tech_points_bar != (UnityEngine.Object) null)
      GUIElements.me.hud.tech_points_bar.Redraw();
    TechPointsDrop._drops.Remove(this);
    this._on_reached_destination.TryInvoke();
  }

  [CompilerGenerated]
  public void \u003CStartFly\u003Eb__12_0()
  {
    this._tweener = (Tweener) null;
    this.OnReachedDestination();
  }
}
