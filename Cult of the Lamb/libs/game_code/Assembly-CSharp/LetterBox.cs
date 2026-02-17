// Decompiled with JetBrains decompiler
// Type: LetterBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using TMPro;
using UnityEngine;

#nullable disable
public class LetterBox : BaseMonoBehaviour
{
  public static LetterBox Instance;
  public RectTransform Top;
  public RectTransform Bottom;
  public static bool IsPlaying;
  public Canvas canvas;
  public float DisplayOffset;
  [SerializeField]
  public CanvasGroup skipCutsceneGroup;
  [SerializeField]
  public TMP_Text subtitle;
  [SerializeField]
  public CanvasGroup subtitleCanvasGroup;
  public Animator animator;

  public void OnEnable()
  {
    LetterBox.Instance = this;
    this.canvas = this.GetComponentInParent<Canvas>();
    LetterBox.Hide();
  }

  public void OnDestroy()
  {
    if (!((Object) LetterBox.Instance == (Object) this))
      return;
    LetterBox.Instance = (LetterBox) null;
  }

  public void Start()
  {
  }

  public static void Show(bool SnapLetterBox)
  {
    if (!LetterBox.IsPlaying)
      LetterBox.Instance.skipCutsceneGroup.alpha = 0.0f;
    LetterBox.Instance.StopAllCoroutines();
    LetterBox.Instance.subtitleCanvasGroup.DOFade(0.0f, 0.0f);
    LetterBox.IsPlaying = true;
    if (!SnapLetterBox)
      LetterBox.Instance.ShowLetterBox();
    else
      LetterBox.Instance.SnapShowLetterBox();
  }

  public void SnapShowLetterBox()
  {
    if ((Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Hide(true, 0);
    LetterBox.Instance.Top.gameObject.SetActive(true);
    LetterBox.Instance.Bottom.gameObject.SetActive(true);
    LetterBox.Instance.animator.SetInteger("State", 0);
  }

  public void ShowLetterBox()
  {
    if ((Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Hide(false, 0);
    LetterBox.Instance.Top.gameObject.SetActive(true);
    LetterBox.Instance.Bottom.gameObject.SetActive(true);
    LetterBox.Instance.animator.SetInteger("State", 1);
  }

  public static void Hide(bool ShowHUD = true)
  {
    if (ShowHUD && (Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Show(0);
    LetterBox.Instance.animator.SetInteger("State", -1);
    LetterBox.Instance.StopAllCoroutines();
    LetterBox.IsPlaying = false;
  }

  public void ShowSkipPrompt()
  {
    this.skipCutsceneGroup.DOKill();
    this.skipCutsceneGroup.DOFade(1f, 0.5f);
  }

  public void HideSkipPrompt()
  {
    this.skipCutsceneGroup.DOKill();
    this.skipCutsceneGroup.DOFade(0.0f, 0.1f);
  }

  public void ShowSubtitle(string text)
  {
    this.subtitle.text = text;
    this.subtitleCanvasGroup.DOKill();
    this.subtitleCanvasGroup.DOFade(1f, 1f);
  }
}
