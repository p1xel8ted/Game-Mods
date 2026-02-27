// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMRadialLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class MMRadialLayoutGroup : MonoBehaviour, ILayoutGroup, ILayoutController
{
  [SerializeField]
  private float _radius;
  [SerializeField]
  private MMRadialLayoutGroup.StartingPosition _startingPosition;
  [SerializeField]
  [Range(0.0f, 360f)]
  private float _offset;

  public float Radius
  {
    get => this._radius;
    set => this._radius = value;
  }

  public float Offset
  {
    get => this._offset;
    set => this._offset = value;
  }

  private void UpdateLayout()
  {
    List<Transform> transformList = new List<Transform>();
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      LayoutElement component;
      if (this.transform.GetChild(index).gameObject.activeSelf && (!this.transform.GetChild(index).TryGetComponent<LayoutElement>(out component) || !component.ignoreLayout))
        transformList.Add(this.transform.GetChild(index));
    }
    float num1 = this.AngleForStartingPosiiton(this._startingPosition) + (float) Math.PI / 180f * this._offset;
    for (int index = 0; index < transformList.Count; ++index)
    {
      float num2 = 6.28318548f * ((float) index / (float) transformList.Count) + num1;
      Vector2 vector2 = new Vector2(Mathf.Cos(-num2), Mathf.Sin(-num2)) * this._radius;
      transformList[index].localPosition = (Vector3) vector2;
    }
  }

  private float AngleForStartingPosiiton(
    MMRadialLayoutGroup.StartingPosition startingPosition)
  {
    switch (startingPosition)
    {
      case MMRadialLayoutGroup.StartingPosition.North:
        return -1.57079637f;
      case MMRadialLayoutGroup.StartingPosition.East:
        return 0.0f;
      case MMRadialLayoutGroup.StartingPosition.South:
        return 1.57079637f;
      case MMRadialLayoutGroup.StartingPosition.West:
        return 3.14159274f;
      default:
        return 0.0f;
    }
  }

  public void SetLayoutHorizontal() => this.UpdateLayout();

  public void SetLayoutVertical() => this.UpdateLayout();

  private void OnTransformChildrenChanged() => this.UpdateLayout();

  [Serializable]
  private enum StartingPosition
  {
    North,
    East,
    South,
    West,
  }
}
