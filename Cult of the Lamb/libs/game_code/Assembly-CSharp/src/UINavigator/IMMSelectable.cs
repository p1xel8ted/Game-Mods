// Decompiled with JetBrains decompiler
// Type: src.UINavigator.IMMSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
