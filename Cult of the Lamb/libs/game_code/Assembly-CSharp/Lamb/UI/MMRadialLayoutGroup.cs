// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMRadialLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float _radius;
  [SerializeField]
  public MMRadialLayoutGroup.StartingPosition _startingPosition;
  [SerializeField]
  [Range(0.0f, 360f)]
  public float _offset;
  [SerializeField]
  public bool _rotate;

  public float Radius
  {
    get => this._radius;
    set
    {
      this._radius = value;
      this.UpdateLayout();
    }
  }

  public float Offset
  {
    get => this._offset;
    set
    {
      this._offset = value;
      this.UpdateLayout();
    }
  }

  public void UpdateLayout()
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
      if (this._rotate)
        transformList[index].localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(vector2.y, vector2.x) * 57.2957764f);
    }
  }

  public float AngleForStartingPosiiton(
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

  public void OnTransformChildrenChanged() => this.UpdateLayout();

  [Serializable]
  public enum StartingPosition
  {
    North,
    East,
    South,
    West,
  }
}
