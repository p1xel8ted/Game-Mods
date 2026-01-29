// Decompiled with JetBrains decompiler
// Type: HUD_BlackSoul
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_BlackSoul : BaseMonoBehaviour
{
  public PlayerFarming playerFarming;
  public Image ProgressBar;
  public Image ProgressBarInstant;
  public ParticleSystem particleSystem;
  public GameObject FollowerTokensObject;
  public TextMeshProUGUI TokenText;
  public NotificationCentreScreen HUD_BlackSoulNotification;
  public static List<int> UpgradeTargets = new List<int>()
  {
    70,
    75,
    80 /*0x50*/,
    85,
    90,
    95,
    100,
    105,
    110,
    120,
    150,
    200
  };
  public List<ConversationEntry> ConversationEntryTutorial;
  public GameObject TutorialObject;
  public RectTransform TutorialRectTransform;
  public RectTransform RingsObject;
  public CanvasGroup canvasGroup;
  public int ParseTokens;

  public void Start()
  {
  }

  public void Init(PlayerFarming playerFarming)
  {
    if (DataManager.Instance.BlackSoulTarget == 0)
      DataManager.Instance.BlackSoulTarget = HUD_BlackSoul.UpgradeTargets[Mathf.Min(DataManager.Instance.Followers.Count + DataManager.Instance.FollowerTokens, HUD_BlackSoul.UpgradeTargets.Count - 1)];
    this.particleSystem.Stop();
    this.ProgressBar.fillAmount = this.ProgressBarInstant.fillAmount = (float) playerFarming.BlackSouls / this.GetUpgradeTarget();
    if (DataManager.Instance.BlackSoulsEnabled)
      return;
    this.RingsObject.gameObject.SetActive(false);
  }

  public void DoTutorial()
  {
    MMConversation.Play(new ConversationObject(ConversationEntry.CloneList(this.ConversationEntryTutorial), (List<MMTools.Response>) null, (System.Action) (() =>
    {
      this.StartCoroutine((IEnumerator) this.StartTutorialRoutine());
      this.StartCoroutine((IEnumerator) this.ScaleProgressBarRoutine());
      DataManager.Instance.BlackSoulsEnabled = true;
    })));
  }

  public IEnumerator StartTutorialRoutine()
  {
    HUD_BlackSoul hudBlackSoul = this;
    if ((UnityEngine.Object) Interaction_DeathNPC.Instance != (UnityEngine.Object) null)
      Interaction_DeathNPC.Instance.enabled = false;
    hudBlackSoul.TutorialRectTransform = UnityEngine.Object.Instantiate<GameObject>(hudBlackSoul.TutorialObject, hudBlackSoul.transform.parent).GetComponent<RectTransform>();
    hudBlackSoul.TutorialRectTransform.SetSiblingIndex(hudBlackSoul.transform.GetSiblingIndex());
    Time.timeScale = 0.0f;
    hudBlackSoul.canvasGroup = hudBlackSoul.TutorialRectTransform.GetComponent<CanvasGroup>();
    float Progress = 0.0f;
    float Duration = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      hudBlackSoul.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
  }

  public IEnumerator ScaleProgressBarRoutine()
  {
    HUD_BlackSoul hudBlackSoul = this;
    yield return (object) new WaitForSecondsRealtime(1f);
    hudBlackSoul.RingsObject.gameObject.SetActive(true);
    CameraManager.shakeCamera(0.3f);
    hudBlackSoul.RingsObject.position = hudBlackSoul.TutorialRectTransform.position;
    hudBlackSoul.ProgressBar.fillAmount = 0.0f;
    hudBlackSoul.ProgressBarInstant.fillAmount = 0.0f;
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = hudBlackSoul.RingsObject.localScale.x;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(2f, StartingScale, Progress / Duration);
      hudBlackSoul.RingsObject.localScale = Vector3.one * num;
      yield return (object) null;
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    CameraManager.shakeCamera(0.3f);
    hudBlackSoul.ProgressBarInstant.fillAmount = 0.5f;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Progress = 0.0f;
    Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      hudBlackSoul.ProgressBar.fillAmount = Mathf.SmoothStep(0.0f, 0.5f, Progress / Duration);
      yield return (object) null;
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    CameraManager.shakeCamera(0.3f);
    hudBlackSoul.ProgressBarInstant.fillAmount = 1f;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Progress = 0.0f;
    Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      hudBlackSoul.ProgressBar.fillAmount = Mathf.SmoothStep(0.5f, 1f, Progress / Duration);
      yield return (object) null;
    }
    yield return (object) new WaitForSecondsRealtime(0.1f);
    hudBlackSoul.ParseTokens = Inventory.FollowerTokens;
    Inventory.FollowerTokens = 1;
    CameraManager.shakeCamera(0.3f);
    hudBlackSoul.StartCoroutine((IEnumerator) hudBlackSoul.ExitTutorialRoutine());
  }

  public IEnumerator ExitTutorialRoutine()
  {
    while (!InputManager.UI.GetAcceptButtonUp() && !InputManager.UI.GetCancelButtonUp())
      yield return (object) null;
    this.EndTutorial();
  }

  public void EndTutorial()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.EndTutorialRoutine());
  }

  public IEnumerator EndTutorialRoutine()
  {
    Time.timeScale = 1f;
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.canvasGroup.alpha = (float) (1.0 - (double) Progress / (double) Duration);
      yield return (object) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.TutorialRectTransform.gameObject);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    Progress = 0.0f;
    Duration = 0.5f;
    Vector3 StartPosition = this.RingsObject.localPosition;
    float Scale = 1f;
    float ScaleSpeed = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      ScaleSpeed += (float) ((1.1000000238418579 - (double) Scale) * 0.40000000596046448);
      Scale += (ScaleSpeed *= 0.6f) * (Time.unscaledDeltaTime * 60f);
      this.RingsObject.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
    Progress = 0.0f;
    Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.RingsObject.localPosition = Vector3.Lerp(StartPosition, Vector3.zero, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    yield return (object) new WaitForSecondsRealtime(0.2f);
    ScaleSpeed = 0.0f;
    Progress = 0.0f;
    Duration = 0.5f;
    CameraManager.shakeCamera(0.4f);
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      ScaleSpeed += (float) ((1.0 - (double) Scale) * 0.20000000298023224);
      Scale += (ScaleSpeed *= 0.6f) * (Time.unscaledDeltaTime * 60f);
      this.RingsObject.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
    this.RingsObject.localScale = Vector3.one;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.ProgressBarInstant.fillAmount = (float) this.playerFarming.BlackSouls / this.GetUpgradeTarget();
    Progress = 0.0f;
    Duration = 0.5f;
    float StartFill = this.ProgressBar.fillAmount;
    float EndFill = (float) this.playerFarming.BlackSouls / this.GetUpgradeTarget();
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.ProgressBar.fillAmount = Mathf.SmoothStep(StartFill, EndFill, Progress / Duration);
      yield return (object) null;
    }
    Inventory.FollowerTokens = this.ParseTokens;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if ((UnityEngine.Object) Interaction_DeathNPC.Instance != (UnityEngine.Object) null)
      Interaction_DeathNPC.Instance.enabled = true;
  }

  public float GetUpgradeTarget() => (float) DataManager.Instance.BlackSoulTarget;

  public void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnGetFollowerToken);
    PlayerFarming.OnGetBlackSoul += new PlayerFarming.GetBlackSoulAction(this.OnGetBlackSoul);
    Inventory.OnGetFollowerToken += new Inventory.GetFollowerToken(this.OnGetFollowerToken);
    Inventory.FollowerTokens = DataManager.Instance.FollowerTokens;
    this.OnGetFollowerToken();
  }

  public void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnGetFollowerToken);
    PlayerFarming.OnGetBlackSoul -= new PlayerFarming.GetBlackSoulAction(this.OnGetBlackSoul);
    Inventory.OnGetFollowerToken -= new Inventory.GetFollowerToken(this.OnGetFollowerToken);
  }

  public void OnGetFollowerToken()
  {
    DataManager.Instance.BlackSoulTarget = HUD_BlackSoul.UpgradeTargets[Mathf.Min(DataManager.Instance.Followers.Count + DataManager.Instance.Followers_Recruit.Count + DataManager.Instance.FollowerTokens, HUD_BlackSoul.UpgradeTargets.Count - 1)];
    this.TokenText.text = DataManager.Instance.FollowerTokens.ToString();
    if (Inventory.FollowerTokens > 0)
      this.FollowerTokensObject.SetActive(true);
    else
      this.FollowerTokensObject.SetActive(false);
  }

  public void OnGetBlackSoul(int DeltaValue, PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) playerFarming)
      return;
    this.particleSystem.Play();
    if ((double) playerFarming.BlackSouls / (double) this.GetUpgradeTarget() >= 1.0)
    {
      ++Inventory.FollowerTokens;
      playerFarming.BlackSouls = 0;
      this.ProgressBar.fillAmount = this.ProgressBarInstant.fillAmount = (float) playerFarming.BlackSouls / this.GetUpgradeTarget();
    }
    else
      this.UpdateProgressBar();
  }

  public void UpdateProgressBar()
  {
    this.ProgressBarInstant.fillAmount = (float) this.playerFarming.BlackSouls / this.GetUpgradeTarget();
    if ((double) this.ProgressBar.fillAmount > (double) this.ProgressBarInstant.fillAmount)
    {
      this.ProgressBar.fillAmount = this.ProgressBarInstant.fillAmount;
    }
    else
    {
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.SmoothStepRoutine());
    }
  }

  public IEnumerator SmoothStepRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    float Progress = 0.0f;
    float Duration = 0.5f;
    float StartPosition = this.ProgressBar.fillAmount;
    float TargetPosition = this.ProgressBarInstant.fillAmount;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.ProgressBar.fillAmount = Mathf.SmoothStep(StartPosition, TargetPosition, Progress / Duration);
      yield return (object) null;
    }
    this.ProgressBar.fillAmount = this.ProgressBarInstant.fillAmount;
  }

  [CompilerGenerated]
  public void \u003CDoTutorial\u003Eb__15_0()
  {
    this.StartCoroutine((IEnumerator) this.StartTutorialRoutine());
    this.StartCoroutine((IEnumerator) this.ScaleProgressBarRoutine());
    DataManager.Instance.BlackSoulsEnabled = true;
  }
}
