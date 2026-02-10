// Decompiled with JetBrains decompiler
// Type: src.UINavigator.IMMSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UINavigator;

public interface IMMSelectable
{
  Selectable Selectable { get; }

  bool Interactable { get; set; }

  bool TryPerformConfirmAction();

  IMMSelectable TryNavigateLeft();

  IMMSelectable TryNavigateRight();

  IMMSelectable TryNavigateUp();

  IMMSelectable TryNavigateDown();

  IMMSelectable FindSelectableFromDirection(Vector3 direction);

  void SetNormalTransitionState();

  void SetInteractionState(bool state);

  PlayerFarming playerFarming { get; set; }
}
