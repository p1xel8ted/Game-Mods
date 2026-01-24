// Decompiled with JetBrains decompiler
// Type: HUD_TrinketCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_TrinketCard : BaseMonoBehaviour
{
  public SkeletonGraphic Card;
  public Image FillImage;
  public GrowAndFade RechargeIcon;
  public TextMeshProUGUI CardQuantity;
  public GameObject CardContainer;
  [CompilerGenerated]
  public TarotCards.TarotCard \u003CCardType\u003Ek__BackingField = new TarotCards.TarotCard(TarotCards.Card.Count, 0);

  public TarotCards.TarotCard CardType
  {
    get => this.\u003CCardType\u003Ek__BackingField;
    set => this.\u003CCardType\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if (!((Object) this.Card != (Object) null))
      return;
    this.Card.material.SetFloat("_GrayscaleLerpFade", 0.0f);
  }

  public void Update()
  {
    if (!((Object) this.FillImage != (Object) null))
      return;
    this.FillImage.fillAmount = TrinketManager.GetRemainingCooldownPercent(this.CardType.CardType, PlayerFarming.Instance);
  }

  public void SetCard(TarotCards.TarotCard card)
  {
    this.CardType = card;
    if (!((Object) this.Card != (Object) null))
      return;
    if (this.Card.Skeleton != null)
      this.Card.Skeleton.SetSkin(TarotCards.Skin(card.CardType));
    TrinketManager.OnTrinketCooldownStart += new TrinketManager.TrinketUpdated(this.OnCooldownStart);
    TrinketManager.OnTrinketCooldownEnd += new TrinketManager.TrinketUpdated(this.OnCooldownEnd);
  }

  public void OnDisable()
  {
    if (!((Object) this.Card != (Object) null))
      return;
    TrinketManager.OnTrinketCooldownStart -= new TrinketManager.TrinketUpdated(this.OnCooldownStart);
    TrinketManager.OnTrinketCooldownEnd -= new TrinketManager.TrinketUpdated(this.OnCooldownEnd);
  }

  public void OnDestroy()
  {
    if (!((Object) this.Card != (Object) null))
      return;
    TrinketManager.OnTrinketCooldownStart -= new TrinketManager.TrinketUpdated(this.OnCooldownStart);
    TrinketManager.OnTrinketCooldownEnd -= new TrinketManager.TrinketUpdated(this.OnCooldownEnd);
  }

  public void OnCooldownStart(TarotCards.Card trinket, PlayerFarming playerFarming = null)
  {
    if (trinket != this.CardType.CardType)
      return;
    this.StartCoroutine((IEnumerator) this.CooldownStartRoutine());
  }

  public IEnumerator CooldownStartRoutine()
  {
    float fade = 0.0f;
    while ((double) fade <= 1.0)
    {
      fade += Time.deltaTime * 2f;
      this.Card.material.SetFloat("_GrayscaleLerpFade", fade);
      yield return (object) null;
    }
  }

  public void OnCooldownEnd(TarotCards.Card trinket, PlayerFarming playerFarming = null)
  {
    this.RechargeIcon.Play();
    this.Card.material.SetFloat("_GrayscaleLerpFade", 0.0f);
  }
}
