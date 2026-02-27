// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIPlayerControllerElementGlyph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Player Controller Element Glyph")]
public class UnityUIPlayerControllerElementGlyph : UnityUIPlayerControllerElementGlyphBase
{
  [Tooltip("The Player id.")]
  [SerializeField]
  public int _playerId;
  [Tooltip("The Action name.")]
  [SerializeField]
  public string _actionName;
  [NonSerialized]
  public int _actionId = -1;
  [NonSerialized]
  public bool _actionIdCached;

  public override int playerId
  {
    get => this._playerId;
    set => this._playerId = value;
  }

  public override int actionId
  {
    get
    {
      if (!this._actionIdCached)
        this.CacheActionId();
      return this._actionId;
    }
    set
    {
      if (!ReInput.isReady)
        return;
      InputAction action = ReInput.mapping.GetAction(value);
      if (action == null)
      {
        Debug.LogError((object) ("Invalid Action id: " + value.ToString()));
      }
      else
      {
        this._actionName = action.name;
        this.CacheActionId();
      }
    }
  }

  public string actionName
  {
    get => this._actionName;
    set
    {
      this._actionName = value;
      this.CacheActionId();
    }
  }

  public void CacheActionId()
  {
    if (!ReInput.isReady)
      return;
    InputAction action = ReInput.mapping.GetAction(this._actionName);
    this._actionId = action != null ? action.id : -1;
    this._actionIdCached = true;
  }
}
