// Decompiled with JetBrains decompiler
// Type: BodyPanelSkullBarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BodyPanelSkullBarGUI : MonoBehaviour
{
  public int negative;
  public int positive;
  public int filled;
  [Range(0.0f, 1f)]
  public float durability;
  public UIWidget green_back;
  public UIWidget back;
  public UIProgressBar green_bar;
  public GameObject skull_red;
  public GameObject skull_white;
  public UIGrid grid;
  public UILabel txt_percent;
  public bool scroll_setup;
  public UIScrollView _scroll_view;
  public UIPanel green_panel;
  public UI2DSprite back_spr;
  public UI2DSprite frame;
  public UnityEngine.Sprite active_back;
  public UnityEngine.Sprite inactive_back;
  public UIWidget grid_widget;
  public GamepadNavigationItem gamepad_item;
  public int scroll_skulls_limit = 10;
  public const float INACTIVE_ALPHA = 1f;
  public List<GameObject> _skulls;
  public BodyPanelSkullBarGUI.Align bar_align;

  public event System.Action on_enable_skulls_frame;

  public event System.Action on_disable_skulls_frame;

  public void Awake() => this.InitGamepadItem();

  public void InitGamepadItem()
  {
    this.gamepad_item.SetCallbacks(new GJCommons.VoidDelegate(this.EnableSelectionFrame), new GJCommons.VoidDelegate(this.DisableSelectionFrame), (GJCommons.VoidDelegate) null);
  }

  public void Redraw()
  {
    this.DisableSelectionFrame();
    this.back_spr.SetActive(true);
    this.skull_red.SetActive(false);
    this.skull_white.SetActive(false);
    int num1 = 0;
    while (this.grid.transform.childCount > 2)
    {
      for (int index = 0; index < this.grid.transform.childCount; ++index)
      {
        GameObject gameObject = this.grid.transform.GetChild(index).gameObject;
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) this.skull_red) && !((UnityEngine.Object) gameObject == (UnityEngine.Object) this.skull_white))
        {
          gameObject.transform.parent = (Transform) null;
          NGUITools.Destroy((UnityEngine.Object) gameObject);
        }
      }
      if (++num1 > 100)
        break;
    }
    this._skulls = new List<GameObject>();
    for (int index = 0; index < this.negative; ++index)
      this._skulls.Add(this.skull_red.Copy());
    for (int index = 0; index < this.positive; ++index)
      this._skulls.Add(this.skull_white.Copy());
    int num2 = Mathf.CeilToInt(this.durability * 100f);
    int num3 = num2;
    if (num2 > 90)
      num2 = 100;
    int num4 = this.positive - Mathf.FloorToInt((float) (this.positive * num2) / 100f);
    this.green_back.gameObject.SetActive(num4 > 0 && this.gameObject.activeSelf);
    this.green_bar.gameObject.SetActive(num4 > 0 && this.gameObject.activeSelf);
    this.green_back.width = 2 + 12 * num4;
    this.green_bar.value = (float) ((1.0 + (double) this.positive * 12.0 * (1.0 - (double) num2 / 100.0)) / 182.0);
    this.green_bar.ForceUpdate();
    this.back.width = 3 + (this.positive + this.negative) * 12;
    this.txt_percent.text = num3.ToString() + "%";
    switch (this.bar_align)
    {
      case BodyPanelSkullBarGUI.Align.Left:
        this.transform.localPosition = new Vector3((float) ((this.negative - 1 + this.positive - 3) * 12), 0.0f, 0.0f);
        break;
      case BodyPanelSkullBarGUI.Align.Center:
        this.transform.localPosition = new Vector3((float) ((this.negative - 1 + this.positive - 3 - (this.negative + this.positive) / 2) * 12), 0.0f, 0.0f);
        break;
    }
    this.RedrawFilledSkulls();
    if (num3 < num2)
      this.green_bar.gameObject.SetActive(this.gameObject.activeSelf);
    this.txt_percent.gameObject.SetActive(this.green_bar.gameObject.activeSelf);
    bool flag = this.scroll_setup && this._skulls.Count > this.scroll_skulls_limit && this.gameObject.activeSelf;
    if (this.scroll_setup)
    {
      this._scroll_view.transform.localPosition = Vector3.zero;
      this._scroll_view.panel.UpdateAnchors();
      this._scroll_view.enabled = flag;
      this.gamepad_item.active = flag && this.gamepad_item.gameObject.activeInHierarchy;
      this.back_spr.sprite2D = flag ? this.active_back : this.inactive_back;
      this.back_spr.type = UIBasicSprite.Type.Sliced;
      this.back_spr.border = new Vector4(24f, 12f, 24f, 12f);
      this.grid_widget.width = 12 * this._skulls.Count;
      if (this._skulls.Count <= this.scroll_skulls_limit)
        this.back_spr.width = this.grid_widget.width + 20;
      else
        this.back_spr.width = 12 * this.scroll_skulls_limit + 20;
      int num5 = this._skulls.Count - this.scroll_skulls_limit;
      this.frame.width = this.back_spr.width;
      Vector4 baseClipRegion = this._scroll_view.panel.baseClipRegion with
      {
        z = (float) (this.back_spr.width - 18)
      };
      this._scroll_view.panel.baseClipRegion = baseClipRegion;
      this.green_panel.baseClipRegion = baseClipRegion;
      Vector3 zero = Vector3.zero;
      this._scroll_view.StopScrolling();
      this._scroll_view.transform.DOKill();
      this._scroll_view.RestrictWithinBounds(false);
      if (num5 > 0)
      {
        zero.x = (float) (num5 * -6);
        this._scroll_view.transform.localPosition = zero;
      }
      else
      {
        this._scroll_view.transform.localPosition = zero;
        this._scroll_view.ResetPosition();
      }
      this._scroll_view.panel.UpdateAnchors();
    }
    if (!this.gameObject.activeSelf)
      this.green_bar.SetActive(false);
    this.grid.Reposition();
    this.grid.repositionNow = true;
    if (this.scroll_setup)
      this.green_panel.UpdateAnchors();
    this.Update();
  }

  public void OnDisable() => this.DisableSelectionFrame();

  public void NoBodyRedraw()
  {
    this.back_spr.SetActive(false);
    this.green_bar.SetActive(false);
    this.gamepad_item.active = false;
  }

  public void RedrawFilledSkulls()
  {
    if (this._skulls == null)
    {
      Debug.LogError((object) "skulls is null in body");
    }
    else
    {
      for (int index = 0; index < this._skulls.Count; ++index)
        this._skulls[index].GetComponent<UI2DSprite>().color = new Color(1f, 1f, 1f, index < this.filled ? 1f : 1f);
    }
  }

  public void EnableSelectionFrame()
  {
    if ((UnityEngine.Object) this.frame == (UnityEngine.Object) null || !this._scroll_view.enabled && this.gameObject.activeInHierarchy)
      return;
    this.frame.gameObject.SetActive(true);
    System.Action enableSkullsFrame = this.on_enable_skulls_frame;
    if (enableSkullsFrame != null)
      enableSkullsFrame();
    Sounds.OnGUIHover();
  }

  public void DisableSelectionFrame()
  {
    if ((UnityEngine.Object) this.frame == (UnityEngine.Object) null || !this._scroll_view.enabled)
      return;
    System.Action disableSkullsFrame = this.on_disable_skulls_frame;
    if (disableSkullsFrame != null)
      disableSkullsFrame();
    this.frame.gameObject.SetActive(false);
  }

  public void Update()
  {
    if (!this.scroll_setup)
      return;
    if (this.green_bar.gameObject.activeSelf)
    {
      Transform transform = this.green_bar.transform;
      transform.localPosition = transform.localPosition with
      {
        x = (float) ((this._skulls.Count - 1) * 6 - 39) + this.grid.transform.localPosition.x + this._scroll_view.transform.localPosition.x
      };
    }
    if (!LazyInput.gamepad_active || !this.frame.gameObject.activeSelf)
      return;
    float num = LazyInput.GetDirection2().x * -0.1f;
    if (num.EqualsTo(0.0f))
      return;
    this._scroll_view.Scroll(num);
  }

  [Serializable]
  public enum Align
  {
    Left,
    Center,
  }
}
