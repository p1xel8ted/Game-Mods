// Decompiled with JetBrains decompiler
// Type: UITarotDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UITarotDisplay : BaseMonoBehaviour
{
  [SerializeField]
  public TMP_Text title;
  [SerializeField]
  public TMP_Text descriptionText;
  [SerializeField]
  public TMP_Text loreText;
  public CanvasGroup canvasGroup;
  [Space]
  [SerializeField]
  public Vector3 offset;
  public RectTransform rectTransform;
  public GameObject lockPosition;
  [SerializeField]
  public Camera camera;
  public TarotCards.Card tarotCard;
  public PlayerFarming playerFarming;

  public void Play(TarotCards.Card card, GameObject lockPos, PlayerFarming playerFarming)
  {
    this.tarotCard = card;
    this.playerFarming = playerFarming;
    this.LocalizeText();
    this.camera = Camera.main;
    this.lockPosition = lockPos;
    this.rectTransform = this.transform as RectTransform;
    Vector3 offset = this.offset;
    this.offset += Vector3.up * 165f;
    DOTween.To((DOGetter<Vector3>) (() => this.offset), (DOSetter<Vector3>) (x => this.offset = x), offset, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvasGroup.alpha = 0.0f;
    DOTween.To((DOGetter<float>) (() => this.canvasGroup.alpha), (DOSetter<float>) (x => this.canvasGroup.alpha = x), 1f, 0.5f);
  }

  public void LateUpdate()
  {
    if ((Object) this.lockPosition == (Object) null)
      return;
    this.rectTransform.position = (Vector3) (Vector2) (this.camera.WorldToScreenPoint(this.lockPosition.transform.position) + this.offset);
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.LocalizeText);
  }

  public void LocalizeText()
  {
    this.title.text = TarotCards.LocalisedName(this.tarotCard);
    this.descriptionText.text = TarotCards.LocalisedDescription(this.tarotCard, this.playerFarming);
    this.loreText.text = TarotCards.LocalisedLore(this.tarotCard);
  }

  [CompilerGenerated]
  public Vector3 \u003CPlay\u003Eb__10_0() => this.offset;

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__10_1(Vector3 x) => this.offset = x;

  [CompilerGenerated]
  public float \u003CPlay\u003Eb__10_2() => this.canvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__10_3(float x) => this.canvasGroup.alpha = x;
}
