// Decompiled with JetBrains decompiler
// Type: HUD_Winter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Winter : MonoBehaviour
{
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject UIContainer;
  [SerializeField]
  public Image wheel;
  [SerializeField]
  public Image blizzardSegment1;
  [SerializeField]
  public Image blizzardSegment2;
  [SerializeField]
  public GameObject[] blizzardIcons;
  [SerializeField]
  public TextMeshProUGUI[] blizzardEndIcons;
  [SerializeField]
  public TextMeshProUGUI CurrentIcon;
  [SerializeField]
  public List<GameObject> WinterSeverityIcons;
  [SerializeField]
  public CanvasGroup severityIconsContainer;
  [SerializeField]
  public Image background;
  [SerializeField]
  public TextMeshProUGUI[] segments;
  [SerializeField]
  public Image[] segmentsDayImages;
  public Image PointerInner;
  [SerializeField]
  public GameObject[] springSegmentIcons;
  [SerializeField]
  public GameObject[] winterSegmentIcons;
  [SerializeField]
  public GameObject padLockIcon;
  public Color fillColorSpring;
  public Color fillColorWinter;
  public Color blizzardColorWinter;
  public Color IconColorSpring;
  public Color IconColorWinter;
  public Color IconColorBlizzard;
  public SeasonsManager.Season lastSeason;
  public bool debugShow;
  public Color IconColor;
  [SerializeField]
  public Color DayIconColor = Color.black;
  public static HUD_Winter Instance;
  public bool Revealing;
  [SerializeField]
  public Image[] graphics;
  [Header("Slices")]
  public Image[] daySlices;
  [Header("Icons")]
  public RectTransform iconParent;
  public GameObject iconPrefab;
  public Sprite longNightIcon;
  public List<GameObject> activeIcons = new List<GameObject>();
  public const int daysShown = 5;
  public float RingOffset = 90f;
  public string springIcon = "\uF185";
  public string winterIcon = "\uF2DC";
  public string blizzardIcon = "\uF742";
  public string defaultIcon = "\uF068";
  [SerializeField]
  public int SegmentFontSize = 14;
  [SerializeField]
  public float IconSize = 12f;
  [SerializeField]
  public Vector2 _IconAngle1 = new Vector2(0.0f, 15f);
  [SerializeField]
  public Vector2 _IconAngle2 = new Vector2(115f, 120f);
  [SerializeField]
  public Vector2 _IconAngle3 = new Vector2(120f, 360f);

  public void Start()
  {
    HUD_Winter.Instance = this;
    if (!DataManager.Instance.HasWeatherVaneUI)
      this.UIContainer.SetActive(false);
    this.UpdateBarFeatures(true);
    this.severityIconsContainer.gameObject.SetActive(SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter);
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    this.UpdateBlizzardIconColours();
  }

  public void OnEnable() => this.BounceCurrentIcon();

  public void UpdateBlizzardIconColours()
  {
    foreach (GameObject blizzardIcon in this.blizzardIcons)
    {
      TextMeshProUGUI component = blizzardIcon.GetComponent<TextMeshProUGUI>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.color = this.IconColorBlizzard;
    }
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    this.UpdateBarFeatures(true);
  }

  public void OnDestroy()
  {
    HUD_Winter.Instance = (HUD_Winter) null;
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealIE());

  public IEnumerator RevealIE()
  {
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    RectTransform t = (RectTransform) HUD_Time.Instance.transform;
    CanvasGroup component = HUD_Time.Instance.GetComponent<CanvasGroup>();
    this.severityIconsContainer.alpha = 0.0f;
    t.DOKill();
    component.DOKill();
    this.Revealing = true;
    this.UIContainer.SetActive(false);
    Vector3 previousPos = t.position;
    Transform previousParent = t.parent;
    UIManager.PlayAudio("event:/dlc/building/weathervane/interact_firsttime");
    Vector3 localPosition = t.localPosition;
    t.parent = HUD_Manager.Instance.transform;
    t.DOLocalMove(Vector3.zero, 0.8f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => t.parent = previousParent));
    yield return (object) new WaitForSeconds(0.6f);
    t.DOScale(t.localScale * 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSeconds(0.9f);
    DataManager.Instance.HasWeatherVaneUI = true;
    this.UIContainer.SetActive(true);
    this.UpdateBarFeatures(true);
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    this.graphics = this.UIContainer.GetComponentsInChildren<Image>();
    foreach (Image graphic in this.graphics)
    {
      Color color = graphic.color with { a = 0.0f };
      graphic.color = color;
    }
    DG.Tweening.Sequence s = DOTween.Sequence();
    this.UIContainer.transform.localScale = new Vector3(-2f, 2f, 2f);
    this.UIContainer.transform.eulerAngles = new Vector3(0.0f, 0.0f, 145f);
    s.Join((Tween) this.UIContainer.transform.DOScale(new Vector3(-1f, 1f, 1f), 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.Join((Tween) this.UIContainer.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 325f), 1.5f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutBack));
    foreach (Image graphic in this.graphics)
      s.Join((Tween) DOTweenModuleUI.DOFade(graphic, 1f, 1.5f));
    yield return (object) new WaitForSeconds(2.5f);
    t.DOMove(previousPos, 0.8f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.6f);
    t.DOScale(t.localScale / 2f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/ui/level_node_beat_level");
    yield return (object) new WaitForSeconds(0.2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    this.Revealing = false;
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.WeatherVane))
    {
      PlayerFarming.SetStateForAllPlayers();
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.WeatherVane);
    }
    else
      PlayerFarming.SetStateForAllPlayers();
    this.severityIconsContainer.DOFade(1f, 1f);
    yield return (object) new WaitForSeconds(1f);
    this.UpdateBarFeatures(true);
  }

  public void UpdateRing()
  {
    int currentDay = TimeManager.CurrentDay;
    float ringOffset = this.RingOffset;
    int num1 = 1200;
    float num2 = TimeManager.TotalElapsedGameTime % (float) num1 / (float) num1;
    if (TimeManager.IsLongNight && TimeManager.CurrentPhase == DayPhase.Night && (double) TimeManager.CurrentPhaseProgress >= 0.5)
    {
      ++currentDay;
      num2 = 0.0f;
    }
    float num3 = Mathf.Clamp01(num2);
    float num4 = 72f;
    float[] spans = new float[5]
    {
      (1f - num3) * num4,
      0.0f,
      0.0f,
      0.0f,
      num3 * num4
    };
    for (int index = 1; index < 4; ++index)
      spans[index] = num4;
    float num5 = 0.0f;
    for (int index = 0; index < 5; ++index)
      num5 += spans[index];
    float num6 = 360f / num5;
    for (int index = 0; index < 5; ++index)
      spans[index] *= num6;
    float z = ringOffset - 90f;
    for (int index = 0; index < 5; ++index)
    {
      int day = currentDay + index;
      SeasonsManager.Season seasonForDay = SeasonsManager.GetSeasonForDay(day);
      this.daySlices[index].color = this.GetSeasonColor(seasonForDay, day);
      this.daySlices[index].fillAmount = spans[index] / 360f;
      this.daySlices[index].rectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, z);
      z += spans[index];
    }
    this.UpdateDayMarkers(spans, ringOffset - 90f, currentDay);
  }

  public void UpdateDayMarkers(float[] spans, float angleStart, int currentDay)
  {
    float num1 = angleStart;
    for (int index = 0; index < 5 && index < this.segments.Length; ++index)
    {
      float num2 = 2f;
      if (index == 0)
      {
        this.segments[index].transform.localScale = Vector3.one * num2;
      }
      else
      {
        float f = (float) (Math.PI / 180.0 * ((double) num1 + 90.0));
        Vector3 vector3 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * 75f;
        if (this.segments[index].text == this.defaultIcon)
          vector3 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * 76f;
        this.segments[index].transform.localRotation = Quaternion.Euler(0.0f, 0.0f, num1 + 90f);
        this.segments[index].transform.localPosition = vector3;
        int day = currentDay + index;
        SeasonsManager.Season seasonForDay = SeasonsManager.GetSeasonForDay(day);
        if (day == SeasonsManager.SeasonTimestamp + 1)
        {
          this.segments[index].text = seasonForDay == SeasonsManager.Season.Winter ? this.winterIcon : this.springIcon;
          this.segments[index].color = seasonForDay == SeasonsManager.Season.Winter ? this.IconColorWinter : this.IconColorSpring;
          this.segments[index].fontSize = this.IconSize;
          this.segmentsDayImages[index].gameObject.SetActive(false);
        }
        else
        {
          this.segments[index].text = "";
          this.segments[index].color = this.DayIconColor;
          this.segments[index].fontSize = (float) this.SegmentFontSize;
          this.segmentsDayImages[index].gameObject.SetActive(true);
        }
        float num3 = (float) ((((double) (Mathf.Atan2(vector3.y, vector3.x) * 57.29578f) - 90.0) % 360.0 + 360.0) % 360.0);
        float num4 = 1f;
        if ((double) num3 >= (double) this._IconAngle1.x && (double) num3 <= (double) this._IconAngle1.y)
          num4 = Mathf.InverseLerp(this._IconAngle1.x, this._IconAngle1.y, num3);
        else if ((double) num3 >= (double) this._IconAngle2.x && (double) num3 <= (double) this._IconAngle2.y)
          num4 = Mathf.InverseLerp(this._IconAngle2.y, this._IconAngle2.x, num3);
        else if ((double) num3 >= (double) this._IconAngle3.x && (double) num3 <= (double) this._IconAngle3.y)
          num4 = 0.0f;
        if ((double) num4 > 0.05000000074505806)
          num4 = Mathf.Clamp(num4, 0.5f, float.MaxValue);
        if (!string.IsNullOrEmpty(this.segments[index].text))
          this.segments[index].transform.localScale = Vector3.one * num4 * num2;
        else
          this.segments[index].transform.localScale = Vector3.one * num2;
      }
      num1 += spans[index];
    }
  }

  public Color GetSeasonColor(SeasonsManager.Season season, int day)
  {
    if (season == SeasonsManager.Season.Spring)
      return this.fillColorSpring;
    return season == SeasonsManager.Season.Winter ? this.fillColorWinter : Color.white;
  }

  public void UpdateBlizzardSegments()
  {
    this.blizzardSegment1.gameObject.SetActive(false);
    this.blizzardSegment2.gameObject.SetActive(false);
    if (this.blizzardIcons.Length != 0)
      this.blizzardIcons[0].SetActive(false);
    if (this.blizzardIcons.Length > 1)
      this.blizzardIcons[1].SetActive(false);
    if (this.blizzardEndIcons != null)
    {
      for (int index = 0; index < this.blizzardEndIcons.Length; ++index)
        this.blizzardEndIcons[index].gameObject.SetActive(false);
    }
    SeasonsManager.SetBlizzardTimes();
    this.DrawBlizzard(this.blizzardSegment1, DataManager.Instance.blizzardTimeInCurrentSeason, DataManager.Instance.blizzardEndTimeInCurrentSeason, 0);
    this.DrawBlizzard(this.blizzardSegment2, DataManager.Instance.blizzardTimeInCurrentSeason2, DataManager.Instance.blizzardEndTimeInCurrentSeason2, 1);
  }

  public void DrawBlizzard(Image segment, float startNorm, float endNorm, int index)
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Spring)
    {
      ++startNorm;
      ++endNorm;
    }
    if ((double) startNorm < 0.0 || (double) endNorm < 0.0 || (double) endNorm <= (double) startNorm)
    {
      segment.gameObject.SetActive(false);
      if (index < this.blizzardIcons.Length)
        this.blizzardIcons[index].SetActive(false);
      if (this.blizzardEndIcons == null || index >= this.blizzardEndIcons.Length)
        return;
      this.blizzardEndIcons[index].gameObject.SetActive(false);
    }
    else
    {
      SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason);
      int num1 = SeasonsManager.SeasonTimestamp - SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason) + 1;
      int num2 = 1200;
      int num3 = num2;
      float num4 = (float) (num1 * num3);
      float num5 = (float) (SeasonsManager.GetSeasonDuration(SeasonsManager.CurrentSeason) * num2);
      if (TimeManager.IsLongNight && TimeManager.CurrentPhase == DayPhase.Night && (double) TimeManager.CurrentPhaseProgress >= 0.5)
      {
        float num6 = (float) (((double) TimeManager.CurrentPhaseProgress - 0.5) / 0.5);
        num4 += (float) (240.0 * (double) num6 / 2.0);
        num5 += (float) (240.0 * (double) num6 / 2.0);
      }
      float b1 = num4 + startNorm * num5;
      float b2 = num4 + endNorm * num5;
      float totalElapsedGameTime = TimeManager.TotalElapsedGameTime;
      float num7 = (float) (5 * num2);
      float num8 = Mathf.Max(totalElapsedGameTime, b1);
      float num9 = Mathf.Min(totalElapsedGameTime + num7, b2);
      if ((double) num9 <= (double) num8)
      {
        segment.gameObject.SetActive(false);
      }
      else
      {
        float num10 = (num8 - totalElapsedGameTime) / num7;
        double num11 = ((double) num9 - (double) totalElapsedGameTime) / (double) num7;
        float z = (float) ((double) this.RingOffset - 90.0 + (double) num10 * 360.0);
        double num12 = (double) num10;
        float num13 = (float) ((num11 - num12) * 360.0);
        float num14 = z + num13;
        segment.gameObject.SetActive(true);
        segment.fillClockwise = false;
        segment.rectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, z);
        segment.fillAmount = num13 / 360f;
        segment.color = this.blizzardColorWinter;
        if (index < this.blizzardIcons.Length)
        {
          this.blizzardIcons[index].SetActive(true);
          float f = (float) (Math.PI / 180.0 * ((double) z + (double) this.RingOffset));
          Vector3 vector3 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * 75f;
          this.blizzardIcons[index].transform.localPosition = vector3;
          this.blizzardIcons[index].transform.localRotation = this.blizzardIcons[index].transform.parent.rotation;
          float num15 = 2f;
          float num16 = (float) ((((double) (Mathf.Atan2(vector3.y, vector3.x) * 57.29578f) - 90.0) % 360.0 + 360.0) % 360.0);
          float num17 = 1f;
          if ((double) num16 >= (double) this._IconAngle1.x && (double) num16 <= (double) this._IconAngle1.y)
            num17 = Mathf.InverseLerp(this._IconAngle1.x, this._IconAngle1.y, num16);
          else if ((double) num16 >= (double) this._IconAngle2.x && (double) num16 <= (double) this._IconAngle2.y)
            num17 = Mathf.InverseLerp(this._IconAngle2.y, this._IconAngle2.x, num16);
          else if ((double) num16 >= (double) this._IconAngle3.x && (double) num16 <= (double) this._IconAngle3.y)
            num17 = 0.0f;
          if ((double) num17 > 0.05000000074505806)
            num17 = Mathf.Clamp(num17, 0.3f, float.MaxValue);
          this.blizzardIcons[index].transform.localScale = Vector3.one * num17 * num15;
        }
        if (this.blizzardEndIcons == null || index >= this.blizzardEndIcons.Length)
          return;
        this.blizzardIcons[index].gameObject.SetActive(true);
        this.blizzardEndIcons[index].gameObject.SetActive(true);
        int num18 = 1200;
        bool flag = SeasonsManager.GetSeasonForDay(Mathf.FloorToInt((b2 + 0.01f) / (float) num18)) == SeasonsManager.Season.Spring;
        this.blizzardEndIcons[index].text = flag ? this.springIcon : this.winterIcon;
        this.blizzardEndIcons[index].color = flag ? this.IconColorSpring : this.IconColorWinter;
        float f1 = (float) (Math.PI / 180.0 * ((double) num14 + (double) this.RingOffset));
        Vector3 vector3_1 = new Vector3(Mathf.Cos(f1), Mathf.Sin(f1), 0.0f) * 75f;
        this.blizzardEndIcons[index].transform.localPosition = vector3_1;
        this.blizzardEndIcons[index].transform.localRotation = this.blizzardEndIcons[index].transform.parent.rotation;
        float num19 = 2f;
        float num20 = (float) ((((double) Mathf.Atan2(vector3_1.y, vector3_1.x) * 57.295780181884766 - 90.0) % 360.0 + 360.0) % 360.0);
        float num21 = 1f;
        if ((double) num20 >= (double) this._IconAngle1.x && (double) num20 <= (double) this._IconAngle1.y)
          num21 = Mathf.InverseLerp(this._IconAngle1.x, this._IconAngle1.y, num20);
        else if ((double) num20 >= (double) this._IconAngle2.x && (double) num20 <= (double) this._IconAngle2.y)
          num21 = Mathf.InverseLerp(this._IconAngle2.y, this._IconAngle2.x, num20);
        else if ((double) num20 >= (double) this._IconAngle3.x && (double) num20 <= (double) this._IconAngle3.y)
          num21 = 0.0f;
        if ((double) num21 > 0.05000000074505806)
          num21 = Mathf.Clamp(num21, 0.3f, float.MaxValue);
        this.blizzardEndIcons[index].transform.localScale = Vector3.one * num21 * num19;
      }
    }
  }

  public void Update()
  {
    if (DataManager.Instance.HasWeatherVaneUI)
    {
      this.UpdateRing();
      this.UpdateBlizzardSegments();
      if ((UnityEngine.Object) this.PointerInner != (UnityEngine.Object) null)
        this.PointerInner.color = this.IconColor;
      if ((UnityEngine.Object) this.CurrentIcon != (UnityEngine.Object) null)
      {
        if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
        {
          if (this.CurrentIcon.text != this.blizzardIcon)
            this.BounceCurrentIcon();
          this.CurrentIcon.text = this.blizzardIcon;
          this.CurrentIcon.color = this.blizzardColorWinter;
        }
        else if (SeasonsManager.GetSeasonForDay(TimeManager.CurrentDay) == SeasonsManager.Season.Winter)
        {
          if (this.CurrentIcon.text != this.winterIcon)
            this.BounceCurrentIcon();
          this.CurrentIcon.text = this.winterIcon;
        }
        else
        {
          if (this.CurrentIcon.text != this.springIcon)
            this.BounceCurrentIcon();
          this.CurrentIcon.text = this.springIcon;
        }
        this.CurrentIcon.color = this.IconColor;
      }
      if (this.Revealing)
        return;
      this.UpdateBarFeatures();
      this.UIContainer.SetActive(DataManager.Instance.WinterLoopEnabled);
    }
    else
    {
      if (!this.UIContainer.activeSelf)
        return;
      this.UIContainer.SetActive(false);
    }
  }

  public void BounceCurrentIcon()
  {
    this.CurrentIcon.transform.DOKill();
    this.CurrentIcon.transform.localScale = Vector3.one * 4f;
    this.CurrentIcon.transform.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.CurrentIcon.color = SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter ? this.IconColorWinter : this.IconColorSpring;
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
      return;
    this.CurrentIcon.color = this.IconColorBlizzard;
  }

  public void UpdateBarFeatures(bool force = false)
  {
    if (!(SeasonsManager.CurrentSeason != this.lastSeason | force))
      return;
    this.severityIconsContainer.gameObject.SetActive(SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.WinterSeverity > 0);
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && this.WinterSeverityIcons.Count > 0)
    {
      int num = Mathf.Clamp(SeasonsManager.WinterSeverity - 1, 0, this.WinterSeverityIcons.Count - 1);
      for (int index = 0; index < this.WinterSeverityIcons.Count; ++index)
        this.WinterSeverityIcons[index].gameObject.SetActive(num >= index);
    }
    this.lastSeason = SeasonsManager.CurrentSeason;
  }
}
