// Decompiled with JetBrains decompiler
// Type: UICurseSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICurseSelection : BaseMonoBehaviour
{
  [SerializeField]
  private TarotCards.Card curseType;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private Image lockIcon;
  [SerializeField]
  private Image selectedIcon;
  public Material NormalMaterial;
  public Material BWMaterial;

  public TarotCards.Card CurseType => this.curseType;

  private void OnEnable() => this.lockIcon.gameObject.SetActive(true);

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
