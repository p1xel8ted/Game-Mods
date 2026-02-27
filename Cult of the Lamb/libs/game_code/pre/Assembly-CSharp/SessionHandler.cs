// Decompiled with JetBrains decompiler
// Type: SessionHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using Rewired;
using System;
using Unify;
using Unify.Input;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class SessionHandler : MonoBehaviour
{
  public static bool DLC_Checked;

  public static bool HasSessionStarted { get; private set; }

  public void Start()
  {
    SessionManager.OnSessionStart += new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd += new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void Awake()
  {
  }

  public void OnDestroy()
  {
    SessionManager.OnSessionStart -= new SessionManager.SessionEventDelegate(this.OnSessionStart);
    SessionManager.OnSessionEnd -= new SessionManager.SessionEventDelegate(this.OnSessionEnd);
  }

  public void OnSessionStart(Guid sessionGuid, User sessionUser)
  {
    SessionHandler.HasSessionStarted = true;
    InputManager.General.ResetBindings();
    this.gameObject.AddComponent<AchievementsWrapper>();
  }

  public void OnSessionEnd(Guid sessionGuid, User sessionUser)
  {
    SessionHandler.HasSessionStarted = false;
    Time.timeScale = 1f;
    GameManager.InMenu = false;
    MMVideoPlayer.ForceStopVideo();
    MMTransition.ResumePlay();
    SimulationManager.Pause();
    FollowerManager.Reset();
    StructureManager.Reset();
    KeyboardLightingManager.Reset();
    UIDynamicNotificationCenter.Reset();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<AchievementsWrapper>());
    TwitchManager.Abort();
  }

  public void ResetToTitle()
  {
    Addressables.LoadSceneAsync((object) "Assets/Scenes/Main Menu.unity");
  }

  public void UnlockTestAchievement()
  {
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("test"));
  }

  public void Update()
  {
    Player mainPlayer = RewiredInputManager.MainPlayer;
    if (mainPlayer == null || !mainPlayer.GetButtonDown("Debug"))
      return;
    Debug.Log((object) "DEBUG KEY PRESSED!");
    CheatConsole objectOfType = UnityEngine.Object.FindObjectOfType<CheatConsole>();
    System.Action action;
    if (!((UnityEngine.Object) objectOfType != (UnityEngine.Object) null) || !objectOfType.Cheats.TryGetValue("K", out action))
      return;
    Debug.Log((object) "running Cheat: K");
    action();
  }
}
