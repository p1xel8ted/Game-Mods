// Decompiled with JetBrains decompiler
// Type: HUD_Timer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Timer : BaseMonoBehaviour
{
  public LayoutElement layoutElementComponent;
  public static bool _TimerPaused;
  public static bool _TimerRunning;
  public HUD_TimerToken[] icons;
  public static float StartingTotalTime;

  public static event HUD_Timer.PauseTimer OnPauseTimer;

  public static event HUD_Timer.UnPauseTimer OnUnPauseTimer;

  public static bool TimerPaused
  {
    get => HUD_Timer._TimerPaused;
    set
    {
      if (HUD_Timer._TimerPaused != value)
      {
        if (value)
        {
          if (HUD_Timer.OnPauseTimer != null)
            HUD_Timer.OnPauseTimer();
        }
        else if (HUD_Timer.OnUnPauseTimer != null)
          HUD_Timer.OnUnPauseTimer();
      }
      HUD_Timer._TimerPaused = value;
    }
  }

  public static bool TimerRunning
  {
    get => HUD_Timer._TimerRunning;
    set
    {
      HUD_Timer._TimerRunning = value;
      if (!HUD_Timer._TimerRunning)
        return;
      HUD_Timer.StartingTotalTime = HUD_Timer.Timer;
    }
  }

  public static float Timer
  {
    get => DataManager.Instance.DUNGEON_TIME;
    set => DataManager.Instance.DUNGEON_TIME = value;
  }

  public static float GetTimer() => HUD_Timer.Timer;

  public static bool GetTimeRunning() => HUD_Timer.TimerRunning;

  public static bool IsTimeUp => (double) HUD_Timer.Timer <= 0.0;

  public static float Progress => Mathf.Min(HUD_Timer.Timer / HUD_Timer.StartingTotalTime, 1f);

  public void Start()
  {
    this.layoutElementComponent.ignoreLayout = true;
    this.UpdateAndSetTime();
  }

  public void Update()
  {
    this.UpdateAndSetTime();
    if (!HUD_Timer.TimerRunning || !this.layoutElementComponent.ignoreLayout)
      return;
    this.layoutElementComponent.ignoreLayout = false;
  }

  public void UpdateAndSetTime()
  {
    if (!HUD_Timer.IsTimeUp && (double) HUD_Timer.Timer - (double) Time.deltaTime <= 0.0)
      AudioManager.Instance.SetMusicCombatState();
    if (HUD_Timer.TimerRunning && !HUD_Timer.TimerPaused)
      HUD_Timer.Timer -= Time.deltaTime;
    int index = -1;
    int num1 = Mathf.FloorToInt(HUD_Timer.Timer / 60f);
    float num2 = 0.0f;
    while (++index < this.icons.Length)
    {
      if (index > num1 || (double) HUD_Timer.Timer <= 0.0)
      {
        if (this.icons[index].gameObject.activeSelf)
          this.icons[index].gameObject.SetActive(false);
      }
      else if (index == num1)
      {
        if (!this.icons[index].gameObject.activeSelf)
          this.StartCoroutine((IEnumerator) this.EnableIcon(this.icons[index], num2 += 0.1f));
        this.icons[index].fillAmount = (float) (((double) HUD_Timer.Timer - (double) (60 * num1)) / 60.0);
      }
      else
      {
        this.icons[index].fillAmount = 1f;
        if (!this.icons[index].gameObject.activeSelf)
          this.StartCoroutine((IEnumerator) this.EnableIcon(this.icons[index], num2 += 0.1f));
      }
    }
  }

  public IEnumerator EnableIcon(HUD_TimerToken icon, float Delay)
  {
    yield return (object) new WaitForSeconds(Delay);
    icon.gameObject.SetActive(true);
  }

  public delegate void PauseTimer();

  public delegate void UnPauseTimer();
}
