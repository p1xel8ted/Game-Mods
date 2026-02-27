// Decompiled with JetBrains decompiler
// Type: src.UINavigator.IMMSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
}
