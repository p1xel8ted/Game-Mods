// Decompiled with JetBrains decompiler
// Type: HUD_TrinketCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_TrinketCard : BaseMonoBehaviour
{
  public SkeletonGraphic Card;
  public Image FillImage;
  public GrowAndFade RechargeIcon;

  public TarotCards.TarotCard CardType { get; private set; }

  private void Start()
  {
    this.CardType = new TarotCards.TarotCard(TarotCards.Card.Count, 0);
    if (!((Object) this.Card != (Object) null))
      return;
    this.Card.material.SetFloat("_GrayscaleLerpFade", 0.0f);
  }

  private void Update()
  {
    if (!((Object) this.FillImage != (Object) null))
      return;
    this.FillImage.fillAmount = TrinketManager.GetRemainingCooldownPercent(this.CardType.CardType);
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

  private void OnDisable()
  {
    if (!((Object) this.Card != (Object) null))
      return;
    TrinketManager.OnTrinketCooldownStart -= new TrinketManager.TrinketUpdated(this.OnCooldownStart);
    TrinketManager.OnTrinketCooldownEnd -= new TrinketManager.TrinketUpdated(this.OnCooldownEnd);
  }

  private void OnDestroy()
  {
    if (!((Object) this.Card != (Object) null))
      return;
    TrinketManager.OnTrinketCooldownStart -= new TrinketManager.TrinketUpdated(this.OnCooldownStart);
    TrinketManager.OnTrinketCooldownEnd -= new TrinketManager.TrinketUpdated(this.OnCooldownEnd);
  }

  private void OnCooldownStart(TarotCards.Card trinket)
  {
    if (trinket != this.CardType.CardType)
      return;
    this.StartCoroutine((IEnumerator) this.CooldownStartRoutine());
  }

  private IEnumerator CooldownStartRoutine()
  {
    float fade = 0.0f;
    while ((double) fade <= 1.0)
    {
      fade += Time.deltaTime * 2f;
      this.Card.material.SetFloat("_GrayscaleLerpFade", fade);
      yield return (object) null;
    }
  }

  private void OnCooldownEnd(TarotCards.Card trinket)
  {
    this.RechargeIcon.Play();
    this.Card.material.SetFloat("_GrayscaleLerpFade", 0.0f);
  }
}
