// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBottomContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (HorizontalLayoutGroup))]
public class FlockadeBottomContainer : MonoBehaviour
{
  [SerializeField]
  public LayoutElement _leftSpacer;
  [SerializeField]
  public LayoutElement _rightSpacer;
  public HorizontalLayoutGroup _layoutGroup;
  [CompilerGenerated]
  public bool \u003CIsFlipped\u003Ek__BackingField;

  public event Action<bool> ConfigurationChanged;

  public bool IsFlipped
  {
    get => this.\u003CIsFlipped\u003Ek__BackingField;
    set => this.\u003CIsFlipped\u003Ek__BackingField = value;
  }

  public virtual void Awake() => this._layoutGroup = this.GetComponent<HorizontalLayoutGroup>();

  public void Reconfigure(FlockadeGameBoardSide side)
  {
    int num1 = this.IsFlipped ? 1 : 0;
    this.IsFlipped = side == FlockadeGameBoardSide.Right;
    this._layoutGroup.reverseArrangement = this.IsFlipped;
    int num2 = this.IsFlipped ? 1 : 0;
    if (num1 == num2)
      return;
    Action<bool> configurationChanged = this.ConfigurationChanged;
    if (configurationChanged == null)
      return;
    configurationChanged(this.IsFlipped);
  }

  public float GetMargin(FlockadeGameBoardSide side)
  {
    float spacing = this._layoutGroup.spacing;
    float preferredWidth;
    if (side != FlockadeGameBoardSide.Left)
    {
      if (side != FlockadeGameBoardSide.Right)
        throw new ArgumentOutOfRangeException();
      preferredWidth = this._rightSpacer.preferredWidth;
    }
    else
      preferredWidth = this._leftSpacer.preferredWidth;
    return spacing + preferredWidth;
  }
}
