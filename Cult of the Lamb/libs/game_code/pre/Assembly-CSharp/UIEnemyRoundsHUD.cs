// Decompiled with JetBrains decompiler
// Type: UIEnemyRoundsHUD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class UIEnemyRoundsHUD : BaseMonoBehaviour
{
  public static UIEnemyRoundsHUD Instance;
  [SerializeField]
  private TMP_Text roundsText;
  [SerializeField]
  private CanvasGroup cG;

  public static void Play(int currentRound, int totalRounds)
  {
    if ((Object) UIEnemyRoundsHUD.Instance == (Object) null)
      UIEnemyRoundsHUD.Instance = Object.Instantiate<UIEnemyRoundsHUD>(UnityEngine.Resources.Load<UIEnemyRoundsHUD>("Prefabs/UI/UI Enemy Rounds HUD"), GameObject.FindWithTag("Canvas").transform);
    UIEnemyRoundsHUD.Instance.OnRoundStart(currentRound, totalRounds);
    EnemyRoundsBase.OnRoundStart += new EnemyRoundsBase.RoundEvent(UIEnemyRoundsHUD.Instance.OnRoundStart);
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(false);
  }

  public static void Hide()
  {
    if ((Object) UIEnemyRoundsHUD.Instance != (Object) null)
    {
      Object.Destroy((Object) UIEnemyRoundsHUD.Instance.gameObject);
      UIEnemyRoundsHUD.Instance = (UIEnemyRoundsHUD) null;
    }
    HUD_Manager.Instance?.XPBarTransitions?.gameObject.SetActive(true);
  }

  private void OnRoundStart(int round, int maxRounds)
  {
    this.cG.DOFade(1f, 1f);
    if (!((Object) this.roundsText != (Object) null))
      return;
    this.roundsText.text = string.Format(ScriptLocalization.Interactions.Round, (object) (round + 1), (object) maxRounds);
  }
}
