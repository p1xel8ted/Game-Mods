// Decompiled with JetBrains decompiler
// Type: SoulPanelSkullBarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoulPanelSkullBarGUI : MonoBehaviour
{
  public int negative_capacity;
  public int positive_capacity;
  public int red_skulls_sin;
  public int green_skulls_sin;
  public int red_skulls_organ;
  public int green_skulls_organ;
  public UIWidget green_back;
  public UIWidget back;
  public UIProgressBar green_bar;
  public UIProgressBar red_bar;
  [Space]
  public UIProgressBar green_bar_potential;
  [Space]
  public UIProgressBar red_bar_potential;
  public GameObject skull_red;
  public GameObject skull_white;
  public UIGrid grid;
  public const float INACTIVE_ALPHA = 1f;
  public List<GameObject> _skulls_red;
  public List<GameObject> _skulls_white;
  public SoulPanelSkullBarGUI.Align bar_align;

  public void SetSkullValues(
    int red_skulls_sin,
    int white_skulls_sin,
    int red_skulls_organ = 0,
    int white_skulls_organ = 0)
  {
    this.red_skulls_sin = red_skulls_sin;
    this.green_skulls_sin = white_skulls_sin;
    this.red_skulls_organ = red_skulls_organ;
    this.green_skulls_organ = white_skulls_organ;
    this.Redraw();
  }

  public float GetSkullsFillRate()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (this.red_skulls_sin > 0)
      num1 = (float) this.red_skulls_organ / (float) this.red_skulls_sin;
    if (this.green_skulls_sin > 0)
      num2 = (float) this.green_skulls_organ / (float) this.green_skulls_sin;
    return (float) (((double) num1 + (double) num2) / 2.0);
  }

  public void Redraw()
  {
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
    this._skulls_red = new List<GameObject>();
    this._skulls_white = new List<GameObject>();
    for (int index = 0; index < this.negative_capacity; ++index)
      this._skulls_red.Add(this.skull_red.Copy());
    for (int index = 0; index < this.positive_capacity; ++index)
      this._skulls_white.Add(this.skull_white.Copy());
    this.grid.Reposition();
    this.grid.repositionNow = true;
    int redSkullsSin = this.red_skulls_sin;
    for (int index = this._skulls_red.Count - 1; index >= 0 && redSkullsSin > 0; --index)
    {
      --redSkullsSin;
      SkullIconContainerGUI component;
      if (this._skulls_red[index].TryGetComponent<SkullIconContainerGUI>(out component))
        component.SetSkullActive(redSkullsSin >= 0);
    }
    int greenSkullsSin = this.green_skulls_sin;
    for (int index = 0; index < this._skulls_white.Count && greenSkullsSin > 0; ++index)
    {
      --greenSkullsSin;
      SkullIconContainerGUI component;
      if (this._skulls_white[index].TryGetComponent<SkullIconContainerGUI>(out component))
        component.SetSkullActive(greenSkullsSin >= 0);
    }
    if (this.green_skulls_organ > this.positive_capacity)
      this.green_skulls_organ = this.positive_capacity;
    int greenSkullsOrgan = this.green_skulls_organ;
    this.green_bar_potential.gameObject.SetActive(greenSkullsOrgan > 0);
    this.green_bar_potential.value = (float) ((1.0 + (double) greenSkullsOrgan * 12.0) / 182.0);
    this.green_bar_potential.ForceUpdate();
    int num2 = Math.Min(this.green_skulls_organ, this.green_skulls_sin);
    this.green_bar.gameObject.SetActive(num2 > 0);
    this.green_bar.value = (float) ((1.0 + (double) num2 * 12.0) / 182.0);
    this.green_bar.ForceUpdate();
    if (this.red_skulls_organ > this.negative_capacity)
      this.red_skulls_organ = this.negative_capacity;
    int redSkullsOrgan = this.red_skulls_organ;
    this.red_bar_potential.gameObject.SetActive(redSkullsOrgan > 0);
    this.red_bar_potential.value = (float) ((1.0 + (double) redSkullsOrgan * 12.0) / 182.0);
    this.red_bar_potential.ForceUpdate();
    int num3 = Math.Min(this.red_skulls_organ, this.red_skulls_sin);
    this.red_bar.SetActive(num3 > 0);
    this.red_bar.value = (float) ((1.0 + (double) num3 * 12.0) / 182.0);
    this.red_bar.ForceUpdate();
    this.back.width = 3 + (this.positive_capacity + this.negative_capacity) * 12;
    switch (this.bar_align)
    {
      case SoulPanelSkullBarGUI.Align.Left:
        this.transform.localPosition = new Vector3((float) ((this.negative_capacity - 1 + this.positive_capacity - 3) * 12), 0.0f, 0.0f);
        break;
      case SoulPanelSkullBarGUI.Align.Center:
        this.transform.localPosition = new Vector3((float) ((this.negative_capacity - 1 + this.positive_capacity - 3 - (this.negative_capacity + this.positive_capacity) / 2) * 12), 0.0f, 0.0f);
        break;
    }
  }

  [Serializable]
  public enum Align
  {
    Left,
    Center,
  }
}
