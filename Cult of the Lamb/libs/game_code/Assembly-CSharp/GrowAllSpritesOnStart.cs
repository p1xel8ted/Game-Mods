// Decompiled with JetBrains decompiler
// Type: GrowAllSpritesOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class GrowAllSpritesOnStart : BaseMonoBehaviour
{
  [SerializeField]
  public float growTime = 0.5f;

  public void Start()
  {
    foreach (Component componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.transform.DOScale(Vector3.zero, this.growTime).From<TweenerCore<Vector3, Vector3, VectorOptions>>();
  }
}
