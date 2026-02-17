// Decompiled with JetBrains decompiler
// Type: MenuDLCAdController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Steamworks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class MenuDLCAdController : MonoBehaviour
{
  [SerializeField]
  public TMP_Text availableText;
  [SerializeField]
  public SteamSaleChecker steamSaleChecker;
  [SerializeField]
  public MMButton buttonObj;
  [SerializeField]
  public int steamAppID;
  [SerializeField]
  public Transform _bannerObj;
  [SerializeField]
  public Image buttonRed;
  [SerializeField]
  public Transform scaleTransform;
  [SerializeField]
  public Image outline;
  [SerializeField]
  public GameObject installedText;
  [SerializeField]
  public GameObject alert;
  [SerializeField]
  public string steamURL;
  [SerializeField]
  public string gogURL;
  public ulong SwitchID = 72108652991111168;
  [SerializeField]
  public string GameCoreID;
  [SerializeField]
  public string Ps4ID;
  [SerializeField]
  public string PS5_SIEE_ID;
  [SerializeField]
  public string PS5_SIEA_ID;
  public Coroutine dlcRefreshRoutine;

  public void Start()
  {
    if (this.steamSaleChecker.isActiveAndEnabled)
      this.StartCoroutine((IEnumerator) this.steamSaleChecker.CheckIfOnSale(this.steamAppID));
    if (GameManager.AuthenticateMajorDLC())
      return;
    this.buttonObj.OnSelected += new System.Action(this.OnButtonSelect);
    this.buttonObj.OnDeselected += new System.Action(this.OnButtonDeSelect);
    this.steamSaleChecker.OnSaleUpdated.AddListener(new UnityAction<PriceOverview>(this.UpdateSaleTextSteam));
  }

  public void OnEnable()
  {
    if (DataManager.Instance.clickedDLCAd)
      this.alert.gameObject.SetActive(false);
    this._bannerObj.gameObject.SetActive(false);
    this.buttonObj.Interactable = false;
    this.buttonObj.enabled = false;
    this._bannerObj.gameObject.SetActive(false);
    if (this.dlcRefreshRoutine == null)
      this.dlcRefreshRoutine = this.StartCoroutine((IEnumerator) this.DLC_refreshed());
    this.availableText.gameObject.SetActive(true);
  }

  public IEnumerator DLC_refreshed()
  {
    Debug.Log((object) "DLC is not installed, showing button");
    this.installedText.gameObject.SetActive(false);
    this._bannerObj.gameObject.SetActive(true);
    this.buttonObj.Interactable = true;
    this.buttonObj.enabled = true;
    while (!GameManager.AuthenticateMajorDLC())
      yield return (object) new WaitForSeconds(1f);
    Debug.Log((object) "DLC is installed, hiding button");
    this.installedText.gameObject.SetActive(true);
    this._bannerObj.gameObject.SetActive(false);
    this.buttonObj.Interactable = false;
    this.buttonObj.enabled = false;
    this.availableText.gameObject.SetActive(false);
  }

  public void UpdateSaleTextSteam(PriceOverview p)
  {
    Debug.Log((object) ("update sales text" + p.discount_percent.ToString()));
    if (p == null)
      return;
    if (p.discount_percent != 0)
      this.availableText.text = ScriptLocalization.UI_Generic.Off.Replace("{0}", p.discount_percent.ToString());
    else
      this.UpdateSaleText();
  }

  public void UpdateSaleText() => this.availableText.text = ScriptLocalization.UI_DLC.AvailableNow;

  public void ButtonPressed()
  {
    if (GameManager.AuthenticateMajorDLC())
      return;
    AudioManager.Instance.PlayOneShot("event:/shop/buy");
    if (!SteamAPI.Init())
      return;
    SteamFriends.ActivateGameOverlayToWebPage(this.steamURL);
  }

  public void OnButtonSelect()
  {
    DataManager.Instance.clickedDLCAd = true;
    this.alert.gameObject.SetActive(false);
    this.outline.transform.DOKill();
    this.outline.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCirc);
    this._bannerObj.transform.DOKill();
    this._bannerObj.transform.localScale = Vector3.one * 1.2f;
    this._bannerObj.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.scaleTransform.transform.DOKill();
    this.scaleTransform.transform.localScale = Vector3.one * 1.2f;
    this.scaleTransform.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.buttonRed.DOKill();
    DOTweenModuleUI.DOFade(this.buttonRed, 1f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCirc);
  }

  public void OnButtonDeSelect()
  {
    this.outline.transform.DOKill();
    this.outline.transform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
    this._bannerObj.transform.DOKill();
    this._bannerObj.transform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
    this.buttonRed.DOKill();
    DOTweenModuleUI.DOFade(this.buttonRed, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InCirc);
  }

  public void CheckDLCAdded(bool returned)
  {
  }

  public void OnDisable()
  {
    if (this.dlcRefreshRoutine == null)
      return;
    this.StopCoroutine(this.dlcRefreshRoutine);
    this.dlcRefreshRoutine = (Coroutine) null;
  }
}
