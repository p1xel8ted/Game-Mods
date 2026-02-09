// Decompiled with JetBrains decompiler
// Type: OfferingChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class OfferingChest : MonoBehaviour
{
  public GameObject Container;
  public Interaction_Trader TraderInteraction;
  public GameObject chestOpen;
  public GameObject chestClosed;
  public AnimationCurve ChestFallCurve;

  public void OnEnable()
  {
    this.CheckAvailability();
    Interaction_BaseTeleporter.OnPlayerTeleportedIn += new System.Action(this.CheckReveal);
  }

  public void OnDisable()
  {
    Interaction_BaseTeleporter.OnPlayerTeleportedIn -= new System.Action(this.CheckReveal);
  }

  public void CheckReveal()
  {
    if (!DataManager.Instance.OnboardedOfferingChest || DataManager.Instance.RevealOfferingChest)
      return;
    this.StartCoroutine((IEnumerator) this.RevealRoutine());
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public void ChangeChest()
  {
    if (this.chestOpen.activeSelf)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/material/wood_barrel_impact", this.transform.position);
      this.chestOpen.transform.DOComplete();
      this.chestOpen.SetActive(false);
      this.chestClosed.SetActive(true);
      this.chestClosed.transform.DOKill();
      this.chestClosed.transform.DOPunchScale(new Vector3(0.25f, -0.25f), 0.5f);
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/open_menu", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/material/wood_barrel_impact", this.transform.position);
      this.chestOpen.SetActive(true);
      this.chestClosed.transform.DOComplete();
      this.chestClosed.SetActive(false);
      this.chestOpen.transform.DOKill();
      this.chestOpen.transform.DOPunchScale(new Vector3(0.25f, -0.25f), 0.5f);
    }
  }

  public IEnumerator RevealRoutine()
  {
    OfferingChest offeringChest = this;
    while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(offeringChest.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(offeringChest.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", offeringChest.transform.position);
    DataManager.Instance.OnboardedOfferingChest = true;
    DataManager.Instance.RevealOfferingChest = true;
    offeringChest.CheckAvailability();
    offeringChest.Container.transform.localPosition = new Vector3(0.0f, 0.0f, -1f);
    offeringChest.Container.transform.localScale = Vector3.zero;
    TweenerCore<Vector3, Vector3, VectorOptions> t = offeringChest.Container.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.5f);
    offeringChest.Container.transform.DOLocalMove(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(offeringChest.ChestFallCurve);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", offeringChest.transform.position);
    t.Kill();
    offeringChest.Container.transform.localScale = new Vector3(1.5f, 0.5f);
    offeringChest.Container.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(offeringChest.transform.position, new Vector3(2f, 2f, 1f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNext(offeringChest.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void CheckAvailability()
  {
    if (!DataManager.Instance.RevealOfferingChest)
    {
      this.TraderInteraction.enabled = false;
      this.Container.gameObject.SetActive(false);
    }
    else
    {
      this.TraderInteraction.enabled = true;
      this.Container.gameObject.SetActive(true);
    }
  }
}
