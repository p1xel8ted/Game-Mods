// Decompiled with JetBrains decompiler
// Type: GrowAllSpritesOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
