// Decompiled with JetBrains decompiler
// Type: UIRitualPerformed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIRitualPerformed : BaseMonoBehaviour
{
  public RectTransform Container;
  public Image WhiteFlash;
  public Image BackFlash;
  public TextMeshProUGUI TitleText;
  public TextMeshProUGUI DescriptionText;

  public void Play(UpgradeSystem.Type RitualType)
  {
    this.TitleText.text = UpgradeSystem.GetLocalizedName(RitualType);
    this.DescriptionText.text = UpgradeSystem.GetLocalizedActivated(RitualType);
    this.StartCoroutine((IEnumerator) this.PlayRoutine());
  }

  public IEnumerator PlayRoutine()
  {
    UIRitualPerformed uiRitualPerformed = this;
    uiRitualPerformed.BackFlash.enabled = false;
    uiRitualPerformed.Container.localScale = Vector3.one * 2f;
    uiRitualPerformed.Container.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    uiRitualPerformed.WhiteFlash.color = new Color(1f, 1f, 1f, 0.6f);
    DOTweenModuleUI.DOColor(uiRitualPerformed.WhiteFlash, new Color(0.0f, 0.0f, 0.0f, 1f), 0.3f);
    yield return (object) new WaitForSeconds(0.4f);
    CameraManager.instance.ShakeCameraForDuration(1.2f, 1.5f, 0.3f);
    GameManager.GetInstance().HitStop();
    uiRitualPerformed.BackFlash.enabled = true;
    uiRitualPerformed.BackFlash.color = Color.white;
    DOTweenModuleUI.DOColor(uiRitualPerformed.BackFlash, new Color(1f, 1f, 1f, 0.0f), 0.3f);
    uiRitualPerformed.BackFlash.rectTransform.localScale = Vector3.one;
    uiRitualPerformed.BackFlash.rectTransform.DOScale(new Vector3(1.5f, 1.5f), 0.3f);
    uiRitualPerformed.WhiteFlash.color = new Color(1f, 1f, 1f, 0.6f);
    DOTweenModuleUI.DOColor(uiRitualPerformed.WhiteFlash, new Color(0.0f, 0.0f, 0.0f, 1f), 0.3f);
    uiRitualPerformed.Container.DOShakePosition(0.5f, 10f);
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    uiRitualPerformed.Container.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(uiRitualPerformed.\u003CPlayRoutine\u003Eb__6_0));
  }

  [CompilerGenerated]
  public void \u003CPlayRoutine\u003Eb__6_0() => Object.Destroy((Object) this.gameObject);
}
