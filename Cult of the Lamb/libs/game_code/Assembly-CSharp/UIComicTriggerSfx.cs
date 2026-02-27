// Decompiled with JetBrains decompiler
// Type: UIComicTriggerSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UIComicTriggerSfx : MonoBehaviour
{
  public SFXTriggerType triggerType;
  [SerializeField]
  public string sfxName;
  [SerializeField]
  public float delay;
  [Range(0.0f, 2f)]
  [SerializeField]
  public float volume = 1f;
  [Range(-100f, 100f)]
  [SerializeField]
  public float pan;
  [SerializeField]
  public float stopDelay;
  [SerializeField]
  public UIComicSegment comicSegment;
  [SerializeField]
  public bool playVibration;
  [SerializeField]
  public MMVibrate.HapticTypes vibrationType;
  [SerializeField]
  public bool playLighting;
  [SerializeField]
  public Color lightingColor;
  [SerializeField]
  public UnityEvent OnTrigger;
  public float screenTriggerOnCenterScreen;
  public bool sfxPlayed;
  public bool isVisible;
  public UIComicMenuController comicMenu;
  public EventInstance sfx;

  public void StopSfx() => AudioManager.Instance.StopLoop(this.sfx);

  public void SetupSfx(string _sfx, float _volume, UIComicSegment _comicSegment)
  {
    this.comicSegment = _comicSegment;
    this.sfxName = _sfx;
    this.volume = _volume;
  }

  public void Start() => this.comicMenu = this.GetComponentInParent<UIComicMenuController>();

  public void OnEnable()
  {
    this.sfxPlayed = false;
    if (this.triggerType == SFXTriggerType.OnEnable)
    {
      this.StartCoroutine(this.PlaySFXWithDelay());
      this.sfxPlayed = true;
    }
    if (this.triggerType != SFXTriggerType.OnSegmentShown)
      return;
    this.comicSegment.OnSegmentShown.AddListener((UnityAction) (() =>
    {
      this.StartCoroutine(this.PlaySFXWithDelay());
      this.sfxPlayed = true;
    }));
  }

  public void Update()
  {
    if (this.sfxPlayed || this.triggerType != SFXTriggerType.OnScreen)
      return;
    this.CheckCenterScreen();
  }

  public void CheckCenterScreen()
  {
    if (this.sfxPlayed || !this.IsVisible())
      return;
    this.StartCoroutine(this.PlaySFXWithDelay());
    this.sfxPlayed = true;
  }

  public IEnumerator PlaySFXWithDelay()
  {
    if (!this.sfxPlayed)
    {
      yield return (object) new WaitForSeconds(this.delay);
      this.OnTrigger?.Invoke();
      if (this.playLighting)
        DeviceLightingManager.FlashColor(this.lightingColor);
      if (this.playVibration)
        MMVibrate.Haptic(this.vibrationType);
      this.sfx = AudioManager.Instance.PlayOneShotWithInstanceDontStart(this.sfxName);
      if (this.sfx.isValid())
      {
        int num1 = (int) this.sfx.setParameterByName("parameter:/Pan", this.pan);
        int num2 = (int) this.sfx.setVolume(this.volume);
        int num3 = (int) this.sfx.start();
      }
      if ((double) this.stopDelay > 0.0)
      {
        yield return (object) new WaitForSeconds(this.stopDelay);
        int num = (int) this.sfx.stop(STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.StopOneShotDelay(this.sfx, 0.0f);
      }
    }
  }

  public void OnDisable()
  {
    if ((double) this.stopDelay == -1.0)
      return;
    int num = (int) this.sfx.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopOneShotDelay(this.sfx, 3f);
  }

  public void OnDestroy()
  {
    if ((double) this.stopDelay == -1.0)
      return;
    int num = (int) this.sfx.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopOneShotDelay(this.sfx, 0.0f);
  }

  public bool IsVisible()
  {
    Rect rect = new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height);
    Vector3[] fourCornersArray = new Vector3[4];
    ((RectTransform) this.transform).GetWorldCorners(fourCornersArray);
    int num = 0;
    for (int index = 0; index < fourCornersArray.Length; ++index)
    {
      Vector3 screenPoint = this.comicMenu.Camera.WorldToScreenPoint(fourCornersArray[index]);
      if (rect.Contains(screenPoint))
        ++num;
    }
    return num > 0;
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__19_0()
  {
    this.StartCoroutine(this.PlaySFXWithDelay());
    this.sfxPlayed = true;
  }
}
