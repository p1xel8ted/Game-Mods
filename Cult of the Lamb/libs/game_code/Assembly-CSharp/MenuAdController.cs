// Decompiled with JetBrains decompiler
// Type: MenuAdController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class MenuAdController : MonoBehaviour
{
  [SerializeField]
  public MMButton buttonObj;
  [SerializeField]
  public Transform _bannerObj;
  [SerializeField]
  public Image buttonRed;
  [SerializeField]
  public Transform scaleTransform;
  [SerializeField]
  public Image outline;
  [SerializeField]
  public MenuAdController.AdViewer ad1;
  [SerializeField]
  public MenuAdController.AdViewer ad2;
  [SerializeField]
  public TMP_Text buttonText;
  [SerializeField]
  public List<MenuAdController.Ad> nonRemoteAds;
  public string link;
  public List<MenuAdController.Ad> ads = new List<MenuAdController.Ad>();
  public int index;
  public const float timeBetweenAds = 10f;
  public float timestamp;
  public const string jsonURL = "https://drive.google.com/uc?export=download&id=1Z0tXv_E6bn-Ukw1B6j1_Sl54Ajc7EXoE";

  public void Start()
  {
    if (this.nonRemoteAds.Count > 0)
      this.ads = this.nonRemoteAds;
    DOTweenModuleUI.DOFade(this.buttonRed, 0.0f, 0.1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.buttonObj.OnSelected += new System.Action(this.OnButtonSelect);
    this.buttonObj.OnDeselected += new System.Action(this.OnButtonDeSelect);
    this.ads.Shuffle<MenuAdController.Ad>();
    this.StartCoroutine((IEnumerator) this.LoadAdsFromWeb());
  }

  public void OnButtonSelect()
  {
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

  public void Configure(MenuAdController.Ad ad) => this.ads.Add(ad);

  public void Update()
  {
    if ((double) Time.time <= (double) this.timestamp)
      return;
    this.IncrementAd(1);
  }

  public void IncrementAd(int count)
  {
    this.index = (int) Utils.Repeat((float) (this.index + count), (float) this.ads.Count);
    this.ShowAd(this.ads[this.index]);
    this.timestamp = Time.time + 10f;
  }

  public void ShowAd(MenuAdController.Ad ad)
  {
    MenuAdController.AdViewer adViewer1 = this.ad2;
    MenuAdController.AdViewer adViewer2 = this.ad1;
    if ((double) this.ad1.CanvasGroup.alpha >= 1.0)
    {
      adViewer2 = this.ad2;
      adViewer1 = this.ad1;
    }
    adViewer2.RawContent.gameObject.SetActive((UnityEngine.Object) ad.contentTexture != (UnityEngine.Object) null);
    adViewer2.Content.gameObject.SetActive((UnityEngine.Object) ad.contentTexture == (UnityEngine.Object) null);
    if ((UnityEngine.Object) ad.contentTexture != (UnityEngine.Object) null)
      adViewer2.RawContent.texture = ad.contentTexture;
    else
      adViewer2.Content.sprite = ad.contentSprite;
    adViewer2.Message.text = ad.messageTerm;
    adViewer2.Message.GetComponent<Localize>().Term = ad.messageTerm;
    this.buttonText.text = ad.buttonTerm;
    this.buttonText.GetComponent<Localize>().Term = ad.buttonTerm;
    this.link = ad.link;
    adViewer1.CanvasGroup.DOFade(0.0f, 0.5f);
    adViewer2.CanvasGroup.DOFade(1f, 0.5f);
  }

  public void ButtonPressed()
  {
    AudioManager.Instance.PlayOneShot("event:/shop/buy");
    Application.OpenURL(this.link);
  }

  public IEnumerator LoadAdsFromWeb()
  {
    UnityWebRequest request = UnityWebRequest.Get("https://drive.google.com/uc?export=download&id=1Z0tXv_E6bn-Ukw1B6j1_Sl54Ajc7EXoE");
    yield return (object) request.SendWebRequest();
    if (request.result == UnityWebRequest.Result.Success)
    {
      try
      {
        MenuAdController.AdNetworked adNetworked = JsonUtility.FromJson<MenuAdController.AdNetworked>(request.downloadHandler.text);
        if (adNetworked.Data != null)
        {
          if (adNetworked.Data.Length != 0)
          {
            ((IList<MenuAdController.AdData>) adNetworked.Data).Shuffle<MenuAdController.AdData>();
            for (int index = 0; index < adNetworked.Data.Length; ++index)
              this.LoadAd(adNetworked.Data[index]);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }
    request.Dispose();
  }

  public void LoadAd(MenuAdController.AdData ad)
  {
    this.StartCoroutine((IEnumerator) this.GetImageFromWeb(ad.ImageURL, (Action<Texture>) (texture => this.ads.Insert(this.index + 1, new MenuAdController.Ad()
    {
      messageTerm = ad.Message,
      link = ad.Link,
      buttonTerm = ad.Button,
      contentTexture = texture
    }))));
  }

  public IEnumerator GetImageFromWeb(string url, Action<Texture> callback)
  {
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
    yield return (object) request.SendWebRequest();
    if (request.result == UnityWebRequest.Result.Success)
    {
      Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
      Action<Texture> action = callback;
      if (action != null)
        action((Texture) texture);
    }
    request.Dispose();
  }

  [Serializable]
  public struct Ad
  {
    public Sprite contentSprite;
    public Texture contentTexture;
    public string messageTerm;
    public string buttonTerm;
    public string link;
  }

  [Serializable]
  public struct AdNetworked
  {
    public MenuAdController.AdData[] Data;
  }

  [Serializable]
  public struct AdData
  {
    public string Message;
    public string Button;
    public string Link;
    public string ImageURL;
  }

  [Serializable]
  public struct AdViewer
  {
    public Image Content;
    public RawImage RawContent;
    public TMP_Text Message;
    public CanvasGroup CanvasGroup;
  }
}
