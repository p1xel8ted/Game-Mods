// Decompiled with JetBrains decompiler
// Type: PlayerAmmo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
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
  public List<PlayerAmmo.BiomeColour> Colours;
  private Color color;
  private float ShakeDuration = 1f;
  private float ShakeX = 0.1f;

  private void OnEnable()
  {
    FaithAmmo.OnAmmoCountChanged += new System.Action(this.AmmoChanged);
    FaithAmmo.OnCantAfford += new System.Action(this.CantAfford);
  }

  private void Start()
  {
    this.CanvasGroup.DOKill();
    this.CanvasGroup.alpha = 0.0f;
    foreach (PlayerAmmo.BiomeColour colour in this.Colours)
    {
      if (colour.Location == PlayerFarming.Location)
      {
        this.color = colour.Color;
        break;
      }
    }
    foreach (Graphic image in this.Images)
      image.color = this.color;
  }

  private void OnDisable()
  {
    FaithAmmo.OnAmmoCountChanged -= new System.Action(this.AmmoChanged);
    FaithAmmo.OnCantAfford -= new System.Action(this.CantAfford);
  }

  private void CantAfford()
  {
    this.transform.DOShakePosition(this.ShakeDuration, new Vector3(this.ShakeX, 0.0f), randomness: 0.0f);
    this.AmmoChanged();
  }

  private void AmmoChanged()
  {
    this.StopAllCoroutines();
    this.CanvasGroup.DOKill();
    this.CanvasGroup.DOFade(1f, 0.1f);
    int index = -1;
    while (++index < this.Images.Count)
    {
      this.Images[index].gameObject.SetActive(index < Mathf.FloorToInt(FaithAmmo.Total / (float) PlayerSpells.AmmoCost));
      if (index < Mathf.FloorToInt(FaithAmmo.Ammo / (float) PlayerSpells.AmmoCost))
      {
        if ((UnityEngine.Object) this.Images[index].sprite == (UnityEngine.Object) this.EmptySprite)
        {
          this.Images[index].transform.DOKill();
          this.Images[index].transform.DOPunchScale(new Vector3(0.1f, 0.1f), 1f);
        }
        this.Images[index].sprite = this.AmmoSprite;
        this.Images[index].color = this.color;
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

  private IEnumerator FadeOut()
  {
    yield return (object) new WaitForSeconds(3f);
    this.CanvasGroup.DOKill();
    this.CanvasGroup.DOFade(0.0f, 1f);
  }

  private void GetAmmoSprites()
  {
    this.Images = new List<Image>((IEnumerable<Image>) this.GetComponentsInChildren<Image>());
  }

  [Serializable]
  public struct BiomeColour
  {
    public Color Color;
    public FollowerLocation Location;
  }
}
