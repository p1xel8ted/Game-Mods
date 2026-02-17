// Decompiled with JetBrains decompiler
// Type: UICurseSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICurseSelection : BaseMonoBehaviour
{
  [SerializeField]
  public TarotCards.Card curseType;
  [SerializeField]
  public Image icon;
  [SerializeField]
  public Image lockIcon;
  [SerializeField]
  public Image selectedIcon;
  public Material NormalMaterial;
  public Material BWMaterial;

  public TarotCards.Card CurseType => this.curseType;

  public void OnEnable() => this.lockIcon.gameObject.SetActive(true);

  public void Selected()
  {
    this.transform.DOScale(1.2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.icon.material = this.NormalMaterial;
  }

  public void Unselected()
  {
    this.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.icon.material = this.BWMaterial;
  }
}
