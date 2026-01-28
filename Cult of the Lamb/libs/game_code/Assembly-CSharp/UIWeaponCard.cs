// Decompiled with JetBrains decompiler
// Type: UIWeaponCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UIWeaponCard : BaseMonoBehaviour
{
  public SkeletonGraphic Spine;
  public Material NormalMaterial;
  public Material RareMaterial;
  public Material SuperRareMaterial;
  public RectTransform SpineRT;
  public RectTransform Effects;
  public TextMeshProUGUI EffectText;
  public TextMeshProUGUI SubtitleText;
  public TextMeshProUGUI NameText;
  [SerializeField]
  public ParticleSystem _particleSystem;
  public CanvasGroup InformationBox;
  [CompilerGenerated]
  public bool \u003CCameFromPauseMenu\u003Ek__BackingField;

  public bool CameFromPauseMenu
  {
    get => this.\u003CCameFromPauseMenu\u003Ek__BackingField;
    set => this.\u003CCameFromPauseMenu\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.Effects.localPosition = new Vector3(-550f, 0.0f);
    this.Spine.AnimationState.SetAnimation(0, "empty", false);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleSpineEvent);
  }

  public void Show(TarotCards.TarotCard Card)
  {
    this._particleSystem.Stop();
    this.Spine.Skeleton.SetSkin(TarotCards.Skin(Card.CardType));
    this.Spine.AnimationState.AddAnimation(0, "static", true, 0.0f);
    this.SubtitleText.text = TarotCards.LocalisedLore(Card.CardType);
    if (this.SubtitleText.text == "")
      this.SubtitleText.gameObject.SetActive(false);
    this.EffectText.text = TarotCards.LocalisedDescription(Card.CardType, 0, PlayerFarming.Instance);
    this.NameText.text = TarotCards.LocalisedName(Card.CardType, 0);
    if (Card.UpgradeIndex == 0)
    {
      this.Spine.material = this.NormalMaterial;
      Debug.Log((object) "Normal Tarot Card");
    }
    else if (Card.UpgradeIndex == 1)
    {
      this.Spine.material = this.RareMaterial;
      this._particleSystem.Play();
      Debug.Log((object) "Rare Tarot Card");
    }
    else if (Card.UpgradeIndex == 2)
    {
      this.Spine.material = this.SuperRareMaterial;
      this._particleSystem.Play();
      Debug.Log((object) "Super Rare Tarot Card");
    }
    else
    {
      this.Spine.material = this.NormalMaterial;
      Debug.Log((object) ("Upgrade Index not set: " + Card.UpgradeIndex.ToString()));
    }
    this.StartCoroutine((IEnumerator) this.RevealEffectDetails());
  }

  public void OnDisable()
  {
    if (!((Object) this.Spine != (Object) null) || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleSpineEvent);
  }

  public void HandleSpineEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "Shake Screen":
        CameraManager.shakeCamera(Random.Range(0.15f, 0.2f), (float) Random.Range(0, 360));
        break;
      case "reveal":
        AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_reveal", this.gameObject);
        break;
    }
  }

  public IEnumerator Play(TarotCards.TarotCard Card, Vector3 Position)
  {
    UIWeaponCard uiWeaponCard = this;
    uiWeaponCard._particleSystem.Stop();
    uiWeaponCard.Spine.enabled = false;
    string skinName = TarotCards.Skin(Card.CardType);
    Debug.Log((object) $"CARD: {Card}  {skinName}");
    if (skinName != "")
      uiWeaponCard.Spine.Skeleton.SetSkin(skinName);
    uiWeaponCard.SubtitleText.text = TarotCards.LocalisedLore(Card.CardType);
    if (uiWeaponCard.SubtitleText.text == "")
      uiWeaponCard.SubtitleText.gameObject.SetActive(false);
    uiWeaponCard.EffectText.text = TarotCards.LocalisedDescription(Card.CardType, Card.UpgradeIndex, PlayerFarming.Instance);
    uiWeaponCard.NameText.text = TarotCards.LocalisedName(Card.CardType, Card.UpgradeIndex);
    if (Card.UpgradeIndex == 0)
    {
      uiWeaponCard.Spine.material = uiWeaponCard.NormalMaterial;
      Debug.Log((object) "Normal Tarot Card");
    }
    else if (Card.UpgradeIndex == 1)
    {
      uiWeaponCard.Spine.material = uiWeaponCard.RareMaterial;
      uiWeaponCard._particleSystem.Play();
      Debug.Log((object) "Rare Tarot Card");
    }
    else if (Card.UpgradeIndex == 2)
    {
      uiWeaponCard.Spine.material = uiWeaponCard.SuperRareMaterial;
      uiWeaponCard._particleSystem.Play();
      Debug.Log((object) "Super Rare Tarot Card");
    }
    else
    {
      uiWeaponCard.Spine.material = uiWeaponCard.NormalMaterial;
      Debug.Log((object) ("Upgrade Index not set: " + Card.UpgradeIndex.ToString()));
    }
    uiWeaponCard.SpineRT.localPosition = Position;
    if (!uiWeaponCard.CameFromPauseMenu || (double) Time.timeScale == 0.0)
    {
      uiWeaponCard.Spine.AnimationState.SetAnimation(0, "reveal", false);
      uiWeaponCard.Spine.AnimationState.AddAnimation(0, "static", true, 0.0f);
    }
    else
      uiWeaponCard.Spine.AnimationState.AddAnimation(0, "static", true, 0.0f);
    uiWeaponCard.Spine.enabled = true;
    yield return (object) new WaitForEndOfFrame();
    uiWeaponCard.Spine.transform.localScale = Vector3.one * 1.75f;
    if (!uiWeaponCard.CameFromPauseMenu || (double) Time.timeScale == 0.0)
      yield return (object) new WaitForSecondsRealtime(0.5f);
    if (!uiWeaponCard.CameFromPauseMenu || (double) Time.timeScale == 0.0)
      uiWeaponCard.StartCoroutine((IEnumerator) uiWeaponCard.RevealEffectDetails());
    else
      uiWeaponCard.Effects.localPosition = Vector3.zero;
  }

  public IEnumerator RevealEffectDetails()
  {
    yield return (object) new WaitForSecondsRealtime(0.2f);
    float Progress = 0.0f;
    Vector3 StartPosition = this.Effects.localPosition;
    while ((double) (Progress += Time.unscaledDeltaTime * 2f) <= 1.0)
    {
      this.Effects.localPosition = Vector3.Lerp(StartPosition, Vector3.zero, Mathf.SmoothStep(0.0f, 1f, Progress));
      yield return (object) null;
    }
    this.Effects.localPosition = Vector3.zero;
  }
}
