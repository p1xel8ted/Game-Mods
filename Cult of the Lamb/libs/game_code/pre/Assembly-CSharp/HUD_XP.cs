// Decompiled with JetBrains decompiler
// Type: HUD_XP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
public class HUD_XP : BaseMonoBehaviour
{
  public Image _instantBar;
  public Image _lerpBar;
  public Image _flashBar;
  public UI_Transitions _transition;
  public TextMeshProUGUI _text;
  private int _cacheXP;
  private int _xpTmp;
  private float _previousXP;
  private int _tmpXPTarget;
  private Coroutine _getXPCoroutine;
  private Coroutine _lerpBarCoroutine;
  private Coroutine _flashBarCoroutine;

  private int XP => DataManager.Instance.XP;

  private int _targetXP
  {
    get
    {
      return DataManager.TargetXP[Mathf.Min(DataManager.Instance.Level, Mathf.Max(DataManager.TargetXP.Count - 1, 0))];
    }
  }

  private void OnEnable()
  {
    if (!GameManager.HasUnlockAvailable())
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this._xpTmp = this.XP <= this._targetXP ? this.XP : this._targetXP;
      this._tmpXPTarget = this._targetXP;
      this._text.text = $"{(object) this._xpTmp}/{(object) this._tmpXPTarget}";
      PlayerFarming.OnGetXP += new System.Action(this.OnGetXP);
      RectTransform rectTransform1 = this._instantBar.rectTransform;
      RectTransform rectTransform2 = this._lerpBar.rectTransform;
      Vector3 vector3_1 = new Vector3(Mathf.Max(Mathf.Clamp((float) this.XP / (float) this._targetXP, 0.0f, 1f), 0.0f), 1f);
      Vector3 vector3_2 = vector3_1;
      rectTransform2.localScale = vector3_2;
      Vector3 vector3_3 = vector3_1;
      rectTransform1.localScale = vector3_3;
      this._flashBar.enabled = false;
      if (SceneManager.GetActiveScene().name != "Base Biome 1")
        this._transition.hideBar();
      this._cacheXP = this.XP;
      this.StartCoroutine((IEnumerator) this.DungeonRoutine());
    }
  }

  private void Start() => this.StartCoroutine((IEnumerator) this.OnGetXPRoutine(true));

  private IEnumerator DungeonRoutine()
  {
    this._transition.hideBar();
    while (true)
    {
      if (GameManager.IsDungeon(PlayerFarming.Location))
      {
        if (this.XP != this._cacheXP)
        {
          Debug.Log((object) "XP != cache");
          if (this._transition.Hidden)
            this._transition.StartCoroutine((IEnumerator) this._transition.MoveBarIn());
          if (this._transition.Hidden)
            yield return (object) new WaitForSeconds(0.5f);
          this._xpTmp = this.XP <= this._targetXP ? this.XP : this._targetXP;
          this._tmpXPTarget = this._targetXP;
          this._cacheXP = this.XP;
          yield return (object) new WaitForSeconds(3f);
          this._transition.StartCoroutine((IEnumerator) this._transition.MoveBarOut());
          yield return (object) null;
        }
        if (!this._transition.Hidden)
          this._transition.hideBar();
      }
      yield return (object) null;
    }
  }

  private void OnDisable()
  {
    this.StopAllCoroutines();
    PlayerFarming.OnGetXP -= new System.Action(this.OnGetXP);
  }

  private void OnGetXP()
  {
    if (this._getXPCoroutine != null)
      this.StopCoroutine(this._getXPCoroutine);
    this._getXPCoroutine = this.StartCoroutine((IEnumerator) this.OnGetXPRoutine(false));
  }

  private IEnumerator OnGetXPRoutine(bool forced)
  {
    HUD_XP hudXp = this;
    while (!hudXp._transition.Revealed && !forced)
      yield return (object) null;
    hudXp._xpTmp = hudXp.XP <= hudXp._targetXP ? hudXp.XP : hudXp._targetXP;
    hudXp._tmpXPTarget = hudXp._targetXP;
    if (hudXp.XP > hudXp._targetXP)
    {
      hudXp._lerpBar.rectTransform.localScale = hudXp._instantBar.rectTransform.localScale = Vector3.zero;
      if (hudXp._flashBarCoroutine != null)
        hudXp.StopCoroutine(hudXp._flashBarCoroutine);
      hudXp.StartCoroutine((IEnumerator) hudXp.FlashBarRoutine());
    }
    else
    {
      Vector3 vector3 = new Vector3(Mathf.Clamp((float) hudXp.XP / (float) hudXp._targetXP, 0.0f, 1f), 1f);
      if (float.IsNaN(vector3.x))
        vector3.x = 0.0f;
      hudXp._instantBar.rectTransform.localScale = vector3;
      if (hudXp._lerpBarCoroutine != null)
        hudXp.StopCoroutine(hudXp._lerpBarCoroutine);
      if ((double) hudXp._instantBar.rectTransform.localScale.x > (double) hudXp._lerpBar.rectTransform.localScale.x)
        hudXp._lerpBarCoroutine = hudXp.StartCoroutine((IEnumerator) hudXp.LerpBarRoutine());
      else
        hudXp._lerpBar.rectTransform.localScale = hudXp._instantBar.rectTransform.localScale;
    }
    float xp = hudXp._previousXP;
    hudXp._previousXP = (float) hudXp._xpTmp;
    if (forced || (double) xp > (double) hudXp._xpTmp)
    {
      hudXp._text.text = $"{(object) hudXp._xpTmp}/{(object) hudXp._tmpXPTarget}";
    }
    else
    {
      float t = 0.0f;
      while ((double) (t += Time.deltaTime * 2f) < 1.0)
      {
        hudXp._text.text = $"{(object) (int) Mathf.Lerp(xp, (float) hudXp._xpTmp, t)}/{(object) hudXp._tmpXPTarget}";
        yield return (object) null;
      }
      hudXp._text.text = $"{(object) hudXp._xpTmp}/{(object) hudXp._tmpXPTarget}";
    }
  }

  private IEnumerator LerpBarRoutine()
  {
    yield return (object) new WaitForSeconds(0.2f);
    Vector3 startPosition = this._lerpBar.rectTransform.localScale;
    float progress = 0.0f;
    float duration = 0.3f;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      this._lerpBar.rectTransform.localScale = Vector3.Lerp(startPosition, this._instantBar.rectTransform.localScale, Mathf.SmoothStep(0.0f, 1f, progress / duration));
      yield return (object) null;
    }
    this._lerpBar.rectTransform.localScale = this._instantBar.rectTransform.localScale;
  }

  private IEnumerator FlashBarRoutine()
  {
    this._flashBar.enabled = true;
    this._flashBar.color = Color.white;
    Color fadeColor = new Color(1f, 1f, 1f, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    float progress = 0.0f;
    float duration = 1f;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      this._flashBar.color = Color.Lerp(Color.white, fadeColor, Mathf.SmoothStep(0.0f, 1f, progress / duration));
      yield return (object) null;
    }
    this._flashBar.enabled = false;
  }
}
