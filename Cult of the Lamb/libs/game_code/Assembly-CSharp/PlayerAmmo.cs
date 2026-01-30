// Decompiled with JetBrains decompiler
// Type: PlayerAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerAmmo : MonoBehaviour
{
  public Sprite AmmoSprite;
  public Sprite EmptySprite;
  public List<Image> Images = new List<Image>();
  public CanvasGroup CanvasGroup;
  public PlayerFarming playerFarming;
  public float ShakeDuration = 1f;
  public float ShakeX = 0.1f;

  public void Initialzie()
  {
    if (!((UnityEngine.Object) this.playerFarming.playerSpells.faithAmmo != (UnityEngine.Object) null))
      return;
    this.playerFarming.playerSpells.faithAmmo.OnAmmoCountChanged -= new System.Action(this.AmmoChanged);
    this.playerFarming.playerSpells.faithAmmo.OnAmmoCountChanged += new System.Action(this.AmmoChanged);
    this.playerFarming.playerSpells.faithAmmo.OnCantAfford -= new System.Action(this.CantAfford);
    this.playerFarming.playerSpells.faithAmmo.OnCantAfford += new System.Action(this.CantAfford);
  }

  public void OnEnable() => this.Initialzie();

  public void Start()
  {
    this.CanvasGroup.DOKill();
    this.CanvasGroup.alpha = 0.0f;
  }

  public void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this.playerFarming || !(bool) (UnityEngine.Object) this.playerFarming.playerSpells || !(bool) (UnityEngine.Object) this.playerFarming.playerSpells.faithAmmo)
      return;
    this.playerFarming.playerSpells.faithAmmo.OnAmmoCountChanged -= new System.Action(this.AmmoChanged);
    this.playerFarming.playerSpells.faithAmmo.OnCantAfford -= new System.Action(this.CantAfford);
  }

  public virtual void CantAfford()
  {
    this.transform.DOShakePosition(this.ShakeDuration, new Vector3(this.ShakeX, 0.0f), randomness: 0.0f);
    this.AmmoChanged();
  }

  public void AmmoChanged()
  {
    this.StopAllCoroutines();
    this.CanvasGroup.DOKill();
    this.CanvasGroup.DOFade(1f, 0.1f);
    int index = -1;
    while (++index < this.Images.Count)
    {
      this.Images[index].gameObject.SetActive(index < Mathf.FloorToInt(this.playerFarming.playerSpells.faithAmmo.Total / (float) this.playerFarming.playerSpells.AmmoCost));
      if (index < Mathf.FloorToInt(this.playerFarming.playerSpells.faithAmmo.Ammo / (float) this.playerFarming.playerSpells.AmmoCost))
      {
        if ((UnityEngine.Object) this.Images[index].sprite == (UnityEngine.Object) this.EmptySprite)
        {
          this.Images[index].transform.DOKill();
          this.Images[index].transform.DOPunchScale(new Vector3(0.1f, 0.1f), 1f);
        }
        this.Images[index].sprite = this.AmmoSprite;
        this.Images[index].color = Color.red;
      }
      else
      {
        if ((UnityEngine.Object) this.Images[index].sprite == (UnityEngine.Object) this.AmmoSprite)
        {
          this.Images[index].transform.DOKill();
          this.Images[index].transform.localScale = Vector3.one * 2f;
          this.Images[index].transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        }
        this.Images[index].sprite = this.EmptySprite;
        this.Images[index].color = Color.white;
      }
    }
    this.StartCoroutine((IEnumerator) this.FadeOut());
  }

  public IEnumerator FadeOut()
  {
    yield return (object) new WaitForSeconds(3f);
    this.CanvasGroup.DOKill();
    this.CanvasGroup.DOFade(0.0f, 1f);
  }

  public void GetAmmoSprites()
  {
    this.Images = new List<Image>((IEnumerable<Image>) this.GetComponentsInChildren<Image>());
  }
}
