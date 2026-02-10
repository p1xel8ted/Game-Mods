// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMScrollbar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMScrollbar : Scrollbar, IMMSelectable
{
  public const float kScrollThreshold = 0.35f;
  public PlayerFarming _playerFarming;

  public Selectable Selectable => (Selectable) this;

  public bool Interactable
  {
    get => this.interactable;
    set => this.interactable = value;
  }

  public PlayerFarming playerFarming
  {
    get => this._playerFarming;
    set => this._playerFarming = value;
  }

  public override void Update()
  {
    base.Update();
    if (!Application.isPlaying || !this.Interactable)
      return;
    float verticalAxis = InputManager.UI.GetVerticalAxis();
    if ((double) verticalAxis > 0.34999999403953552)
    {
      if ((double) this.value >= 1.0)
        return;
    }
    else if ((double) verticalAxis < -0.34999999403953552 && (double) this.value <= 0.0)
      return;
    this.value += verticalAxis * Time.deltaTime;
  }

  public bool TryPerformConfirmAction() => false;

  public IMMSelectable TryNavigateLeft() => (IMMSelectable) this;

  public IMMSelectable TryNavigateRight() => (IMMSelectable) this;

  public IMMSelectable TryNavigateUp() => (IMMSelectable) this;

  public IMMSelectable TryNavigateDown() => (IMMSelectable) this;

  public IMMSelectable FindSelectableFromDirection(Vector3 direction)
  {
    return this.Selectable.FindSelectable(direction) as IMMSelectable;
  }

  public void SetNormalTransitionState()
  {
  }

  public void SetInteractionState(bool state)
  {
  }
}
