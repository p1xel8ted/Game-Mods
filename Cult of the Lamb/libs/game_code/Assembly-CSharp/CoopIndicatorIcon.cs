// Decompiled with JetBrains decompiler
// Type: CoopIndicatorIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using TMPro;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CoopIndicatorIcon : MonoBehaviour
{
  public GameObject LambIconContainer;
  public GameObject GoatIconContainer;
  public Image LambImage;
  public Image GoatImage;
  public TMP_Text UserName;
  [SerializeField]
  public bool animateIconOnDamaged = true;
  [HideInInspector]
  public bool isLambIcon;
  public static List<CoopIndicatorIcon> allIcons = new List<CoopIndicatorIcon>();

  public static void AnimateDamageOnIcons(PlayerFarming playerFarming)
  {
    foreach (CoopIndicatorIcon allIcon in CoopIndicatorIcon.allIcons)
    {
      if (allIcon.animateIconOnDamaged && allIcon.isLambIcon == playerFarming.isLamb)
      {
        allIcon.transform.DOKill();
        if (allIcon.isLambIcon)
        {
          allIcon.LambImage.DOKill();
          allIcon.LambImage.color = Color.red;
          DOTweenModuleUI.DOColor(allIcon.LambImage, Color.white, 0.5f);
        }
        else
        {
          allIcon.GoatImage.DOKill();
          allIcon.GoatImage.color = Color.red;
          DOTweenModuleUI.DOColor(allIcon.GoatImage, Color.white, 0.5f);
        }
        allIcon.transform.localScale = Vector3.one * 1.5f;
        allIcon.transform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      }
    }
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) CoopManager.Instance == (UnityEngine.Object) null)
      return;
    CoopManager.Instance.OnPlayerJoined -= new System.Action(this.UpdateIcon);
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.UpdateIcon);
  }

  public void OnDisable() => CoopIndicatorIcon.allIcons.Remove(this);

  public void OnEnable()
  {
    if ((UnityEngine.Object) CoopManager.Instance == (UnityEngine.Object) null)
      return;
    if (!CoopIndicatorIcon.allIcons.Contains(this))
      CoopIndicatorIcon.allIcons.Add(this);
    CoopManager.Instance.OnPlayerJoined -= new System.Action(this.UpdateIcon);
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.UpdateIcon);
    CoopManager.Instance.OnPlayerJoined += new System.Action(this.UpdateIcon);
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.UpdateIcon);
    this.UpdateIcon();
  }

  public void UpdateIcon()
  {
    this.gameObject.SetActive(CoopManager.CoopActive && PlayerFarming.playersCount > 1);
  }

  public void SetIcon(CoopIndicatorIcon.CoopIcon coopIcon)
  {
    this.LambIconContainer.SetActive(false);
    this.GoatIconContainer.SetActive(false);
    if (coopIcon == CoopIndicatorIcon.CoopIcon.Lamb)
    {
      this.LambIconContainer.SetActive(true);
      this.isLambIcon = true;
    }
    else
    {
      this.GoatIconContainer.SetActive(true);
      this.isLambIcon = false;
    }
  }

  public void SetUsername(int playernumber)
  {
    User player = UserHelper.GetPlayer(playernumber);
    if (player == null)
      return;
    this.UserName.text = player.nickName;
  }

  public enum CoopIcon
  {
    Lamb,
    Goat,
  }
}
