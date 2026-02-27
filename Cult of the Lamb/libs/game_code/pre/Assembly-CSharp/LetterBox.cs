// Decompiled with JetBrains decompiler
// Type: LetterBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Canvas canvas;
  public float DisplayOffset;
  [SerializeField]
  private CanvasGroup skipCutsceneGroup;
  [SerializeField]
  private TMP_Text subtitle;
  [SerializeField]
  private CanvasGroup subtitleCanvasGroup;
  public Animator animator;
  private static bool CacheInvincible;

  private void OnEnable()
  {
    LetterBox.Instance = this;
    this.canvas = this.GetComponentInParent<Canvas>();
    LetterBox.Hide();
  }

  private void OnDestroy()
  {
    if (!((Object) LetterBox.Instance == (Object) this))
      return;
    LetterBox.Instance = (LetterBox) null;
  }

  private void Start()
  {
  }

  public static void Show(bool SnapLetterBox)
  {
    if ((Object) PlayerFarming.Instance != (Object) null && (Object) PlayerFarming.Instance.health != (Object) null)
      PlayerFarming.Instance.health.invincible = LetterBox.CacheInvincible;
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

  private void SnapShowLetterBox()
  {
    if ((Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Hide(true, 0);
    LetterBox.Instance.Top.gameObject.SetActive(true);
    LetterBox.Instance.Bottom.gameObject.SetActive(true);
    LetterBox.Instance.animator.SetInteger("State", 0);
  }

  private void ShowLetterBox()
  {
    if ((Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Hide(false, 0);
    LetterBox.Instance.Top.gameObject.SetActive(true);
    LetterBox.Instance.Bottom.gameObject.SetActive(true);
    LetterBox.Instance.animator.SetInteger("State", 1);
  }

  public static void Hide(bool ShowHUD = true)
  {
    if ((Object) PlayerFarming.Instance != (Object) null && (Object) PlayerFarming.Instance.health != (Object) null)
    {
      LetterBox.CacheInvincible = PlayerFarming.Instance.health.invincible;
      PlayerFarming.Instance.health.invincible = true;
    }
    Debug.Log((object) ("ShowHUD: " + ShowHUD.ToString()));
    if (ShowHUD && (Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.Show(0);
    LetterBox.Instance.animator.SetInteger("State", -1);
    LetterBox.Instance.StopAllCoroutines();
    LetterBox.IsPlaying = false;
  }

  public void ShowSkipPrompt() => this.skipCutsceneGroup.DOFade(1f, 0.5f);

  public void HideSkipPrompt() => this.skipCutsceneGroup.DOFade(0.0f, 0.1f);

  public void ShowSubtitle(string text)
  {
    this.subtitle.text = text;
    this.subtitleCanvasGroup.DOKill();
    this.subtitleCanvasGroup.DOFade(1f, 1f);
  }
}
