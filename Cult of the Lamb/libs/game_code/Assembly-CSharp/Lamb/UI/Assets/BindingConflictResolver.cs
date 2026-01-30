// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.BindingConflictResolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using RewiredConsts;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "BindingConflictResolver", menuName = "Massive Monster/BindingConflictResolver", order = 1)]
public class BindingConflictResolver : ScriptableObject
{
  [SerializeField]
  public BindingConflictResolver.GameplayBindingEntry[] _gameplayBindings = Array.Empty<BindingConflictResolver.GameplayBindingEntry>();
  [SerializeField]
  public BindingConflictResolver.UIBindingEntry[] _uiBindings = Array.Empty<BindingConflictResolver.UIBindingEntry>();
  [SerializeField]
  public BindingConflictResolver.PhotoModeBindingEntry[] _photoModeBindings = Array.Empty<BindingConflictResolver.PhotoModeBindingEntry>();

  public BindingConflictResolver.BindingEntry GetEntry(KeybindItem keybindItem)
  {
    return this.GetEntry(keybindItem.Category, keybindItem.Action);
  }

  public BindingConflictResolver.BindingEntry GetEntry(int keybindCategory, int keybindAction)
  {
    switch (keybindCategory)
    {
      case 0:
        foreach (BindingConflictResolver.GameplayBindingEntry gameplayBinding in this._gameplayBindings)
        {
          if (gameplayBinding.Binding == keybindAction)
            return (BindingConflictResolver.BindingEntry) gameplayBinding;
        }
        break;
      case 1:
        foreach (BindingConflictResolver.UIBindingEntry uiBinding in this._uiBindings)
        {
          if (uiBinding.Binding == keybindAction)
            return (BindingConflictResolver.BindingEntry) uiBinding;
        }
        break;
      case 2:
        foreach (BindingConflictResolver.PhotoModeBindingEntry photoModeBinding in this._photoModeBindings)
        {
          if (photoModeBinding.Binding == keybindAction)
            return (BindingConflictResolver.BindingEntry) photoModeBinding;
        }
        break;
      default:
        return (BindingConflictResolver.BindingEntry) null;
    }
    return (BindingConflictResolver.BindingEntry) null;
  }

  public void PopulateAll()
  {
    this._gameplayBindings = new BindingConflictResolver.GameplayBindingEntry[RewiredGameplayInputSource.AllBindings.Length];
    for (int index1 = 0; index1 < this._gameplayBindings.Length; ++index1)
    {
      BindingConflictResolver.GameplayBindingEntry[] gameplayBindings = this._gameplayBindings;
      int index2 = index1;
      BindingConflictResolver.GameplayBindingEntry gameplayBindingEntry = new BindingConflictResolver.GameplayBindingEntry();
      gameplayBindingEntry.Binding = RewiredGameplayInputSource.AllBindings[index1];
      gameplayBindings[index2] = gameplayBindingEntry;
    }
    this._uiBindings = new BindingConflictResolver.UIBindingEntry[RewiredUIInputSource.AllBindings.Length];
    for (int index3 = 0; index3 < this._uiBindings.Length; ++index3)
    {
      BindingConflictResolver.UIBindingEntry[] uiBindings = this._uiBindings;
      int index4 = index3;
      BindingConflictResolver.UIBindingEntry uiBindingEntry = new BindingConflictResolver.UIBindingEntry();
      uiBindingEntry.Binding = RewiredUIInputSource.AllBindings[index3];
      uiBindings[index4] = uiBindingEntry;
    }
    this._photoModeBindings = new BindingConflictResolver.PhotoModeBindingEntry[PhotoModeInputSource.AllBindings.Length];
    for (int index5 = 0; index5 < this._photoModeBindings.Length; ++index5)
    {
      BindingConflictResolver.PhotoModeBindingEntry[] photoModeBindings = this._photoModeBindings;
      int index6 = index5;
      BindingConflictResolver.PhotoModeBindingEntry modeBindingEntry = new BindingConflictResolver.PhotoModeBindingEntry();
      modeBindingEntry.Binding = PhotoModeInputSource.AllBindings[index5];
      photoModeBindings[index6] = modeBindingEntry;
    }
  }

  public abstract class BindingEntry
  {
    [ActionIdProperty(typeof (RewiredConsts.Action))]
    public int Binding;
    [Header("Locked?")]
    public bool LockedOnKeyboard;
    public bool LockedOnMouse;
    public bool LockedOnGamepad;
    [ActionIdProperty(typeof (RewiredConsts.Action))]
    public List<int> ConflictingBindings;

    public abstract int[] BindingSource { get; }

    public void ClearAll() => this.ConflictingBindings = new List<int>();

    public void AddGameplay()
    {
      foreach (int allBinding in RewiredGameplayInputSource.AllBindings)
      {
        if (!this.ConflictingBindings.Contains(allBinding))
          this.ConflictingBindings.Add(allBinding);
      }
    }

    public void AddUI()
    {
      foreach (int allBinding in RewiredUIInputSource.AllBindings)
      {
        if (!this.ConflictingBindings.Contains(allBinding))
          this.ConflictingBindings.Add(allBinding);
      }
    }

    public void AddPhotoMode()
    {
      foreach (int allBinding in PhotoModeInputSource.AllBindings)
      {
        if (!this.ConflictingBindings.Contains(allBinding))
          this.ConflictingBindings.Add(allBinding);
      }
    }

    public void PopulateWithAll()
    {
      this.ConflictingBindings = new List<int>();
      for (int index = 0; index < this.BindingSource.Length; ++index)
        this.ConflictingBindings.Add(this.BindingSource[index]);
    }
  }

  [Serializable]
  public class GameplayBindingEntry : BindingConflictResolver.BindingEntry
  {
    public override int[] BindingSource => RewiredGameplayInputSource.AllBindings;
  }

  [Serializable]
  public class UIBindingEntry : BindingConflictResolver.BindingEntry
  {
    public override int[] BindingSource => RewiredUIInputSource.AllBindings;
  }

  [Serializable]
  public class PhotoModeBindingEntry : BindingConflictResolver.BindingEntry
  {
    public override int[] BindingSource => PhotoModeInputSource.AllBindings;
  }
}
