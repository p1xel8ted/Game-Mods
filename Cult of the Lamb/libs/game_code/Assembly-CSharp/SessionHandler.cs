// Decompiled with JetBrains decompiler
// Type: SessionHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

#nullable disable
public class SessionHandler : MonoBehaviour
{
  [CompilerGenerated]
  public static bool \u003CHasSessionStarted\u003Ek__BackingField;
  [CompilerGenerated]
  public AsyncOperationHandle<SceneInstance> \u003CBufferAsyncLoad\u003Ek__BackingField;
  public static bool DLC_Checked;
  public UnityEvent onEnd;

  public static bool HasSessionStarted
  {
    get => SessionHandler.\u003CHasSessionStarted\u003Ek__BackingField;
    set => SessionHandler.\u003CHasSessionStarted\u003Ek__BackingField = value;
  }

  public AsyncOperationHandle<SceneInstance> BufferAsyncLoad
  {
    get => this.\u003CBufferAsyncLoad\u003Ek__BackingField;
    set => this.\u003CBufferAsyncLoad\u003Ek__BackingField = value;
  }

  public void Start()
  {
    SessionManager.OnSessionStart += new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd += new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void OnDestroy()
  {
    SessionManager.OnSessionStart -= new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd -= new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void OnSessionStart(Guid sessionGuid, User sessionUser)
  {
    if (SessionHandler.HasSessionStarted)
      return;
    SessionHandler.HasSessionStarted = true;
    Engagement.GlobalAllowEngagement = false;
    InputManager.General.ResetBindings();
    AchievementsWrapper.LoadAchievementData();
    this.gameObject.AddComponent<AchievementsWrapper>();
  }

  public void OnSessionEnd(Guid sessionGuid, User sessionUser)
  {
    Time.timeScale = 1f;
    MMVideoPlayer.ForceStopVideo();
    GameManager.InMenu = false;
    PhotoModeManager.PhotoModeActive = false;
    PhotoModeManager.TakeScreenShotActive = false;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIMenuBase.ActiveMenus.Clear();
    UIDynamicNotificationCenter.Reset();
    SimulationManager.Pause();
    FollowerManager.Reset();
    StructureManager.Reset();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<AchievementsWrapper>());
    DeviceLightingManager.Reset();
    TwitchManager.Abort();
    this.StartCoroutine("SessionEnded");
    this.onEnd?.Invoke();
  }

  public IEnumerator SessionEnded()
  {
    yield return (object) new WaitForSecondsRealtime(0.2f);
    SessionHandler.HasSessionStarted = false;
    Engagement.GlobalAllowEngagement = true;
  }

  public void ResetToTitle()
  {
    SessionHandler.HasSessionStarted = false;
    Engagement.GlobalAllowEngagement = true;
    MMTransition.StopCurrentTransition();
    MMTransition.ForceShowIcon = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 3f, "", (System.Action) (() => { }));
  }

  public void UnlockTestAchievement()
  {
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("test"));
  }

  public void Update()
  {
    if (!PhotoModeManager.PhotoModeActive)
      return;
    Time.timeScale = 0.0f;
  }
}
