// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMScrollbar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMScrollbar : Scrollbar, IMMSelectable
{
  private const float kScrollThreshold = 0.35f;

  public Selectable Selectable => (Selectable) this;

  public bool Interactable
  {
    get => this.interactable;
    set => this.interactable = value;
  }

  protected override void Update()
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
