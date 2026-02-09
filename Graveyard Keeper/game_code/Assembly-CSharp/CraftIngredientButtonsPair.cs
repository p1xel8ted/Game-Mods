// Decompiled with JetBrains decompiler
// Type: CraftIngredientButtonsPair
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CraftIngredientButtonsPair : MonoBehaviour
{
  public UIButton button_previous;
  public UIButton button_next;
  public Action<int> _on_ingredient_changed;

  public void Init(bool available, Action<int> on_ingredient_changed)
  {
    this.gameObject.SetActive(available);
    this._on_ingredient_changed = on_ingredient_changed;
    this.button_next.isEnabled = this.button_previous.isEnabled = true;
  }

  public void SetEnabled(bool previous, bool next)
  {
    if (!this.gameObject.activeSelf)
      return;
    this.button_previous.isEnabled = previous;
    this.button_next.isEnabled = next;
  }

  public void OnPreviousPressed()
  {
    Sounds.OnGUIClick();
    this._on_ingredient_changed(1);
  }

  public void OnNextPressed()
  {
    Sounds.OnGUIClick();
    this._on_ingredient_changed(-1);
  }
}
