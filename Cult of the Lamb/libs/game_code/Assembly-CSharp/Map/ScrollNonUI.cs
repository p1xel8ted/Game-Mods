// Decompiled with JetBrains decompiler
// Type: Map.ScrollNonUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
namespace Map;

public class ScrollNonUI : MonoBehaviour
{
  public float tweenBackDuration = 0.3f;
  public Ease tweenBackEase;
  public bool freezeX;
  public FloatMinMax xConstraints = new FloatMinMax();
  public bool freezeY;
  public FloatMinMax yConstraints = new FloatMinMax();
  public Vector2 offset;
  public Vector3 pointerDisplacement;
  public float zDisplacement;
  public bool dragging;
  public Camera mainCamera;

  public void Awake()
  {
    this.mainCamera = Camera.main;
    this.zDisplacement = -this.mainCamera.transform.position.z + this.transform.position.z;
  }

  public void OnMouseDown()
  {
    this.pointerDisplacement = -this.transform.position + this.MouseInWorldCoords();
    this.transform.DOKill();
    this.dragging = true;
  }

  public void OnMouseUp()
  {
    this.dragging = false;
    this.TweenBack();
  }

  public void Update()
  {
    if (!this.dragging)
      return;
    Vector3 vector3 = this.MouseInWorldCoords();
    this.transform.position = new Vector3(this.freezeX ? this.transform.position.x : vector3.x - this.pointerDisplacement.x, this.freezeY ? this.transform.position.y : vector3.y - this.pointerDisplacement.y, this.transform.position.z);
  }

  public Vector3 MouseInWorldCoords()
  {
    return this.mainCamera.ScreenToWorldPoint(Input.mousePosition with
    {
      z = this.zDisplacement
    });
  }

  public void TweenBack()
  {
    if (this.freezeY)
    {
      if ((double) this.transform.localPosition.x >= (double) this.xConstraints.min && (double) this.transform.localPosition.x <= (double) this.xConstraints.max)
        return;
      this.transform.DOLocalMoveX((double) this.transform.localPosition.x < (double) this.xConstraints.min ? this.xConstraints.min : this.xConstraints.max, this.tweenBackDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.tweenBackEase);
    }
    else
    {
      if (!this.freezeX || (double) this.transform.localPosition.y >= (double) this.yConstraints.min && (double) this.transform.localPosition.y <= (double) this.yConstraints.max)
        return;
      this.transform.DOLocalMoveY((double) this.transform.localPosition.y < (double) this.yConstraints.min ? this.yConstraints.min : this.yConstraints.max, this.tweenBackDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.tweenBackEase);
    }
  }
}
