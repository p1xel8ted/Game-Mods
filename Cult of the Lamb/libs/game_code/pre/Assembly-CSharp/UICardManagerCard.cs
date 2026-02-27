// Decompiled with JetBrains decompiler
// Type: UICardManagerCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICardManagerCard : BaseMonoBehaviour
{
  public GameObject ControlPrompt;
  public GameObject RadialProgressBarGameObject;
  public Image RadialProgressBar;
  public Image RadialProgressBarWhite;
  public SkeletonGraphic Spine;
  public RectTransform Parent;
  public bool Unlocking;
  public Vector2 Shake = Vector2.zero;
  public Vector2 ShakeSpeed = Vector2.zero;
  public Vector2 ShakeDamping = Vector2.zero;
  private float SmoothStepRadial;
  private float SmoothStepStart;
  private float SmoothStepTarget;
  public float UnlockProgressWait;
  public GameObject newCardIcon;
  public TarotCards Card;

  private void Start()
  {
    this.RadialProgressBar.fillAmount = this.RadialProgressBarWhite.fillAmount = 0.0f;
    this.UnlockProgressWait = this.Card.UnlockProgress;
    this.Deselected();
  }

  public void DoShake(float X, float Y) => this.ShakeSpeed += new Vector2(X, Y);

  private void Update()
  {
    this.ShakeSpeed.x += (float) ((0.0 - (double) this.Shake.x) * 0.20000000298023224);
    this.Shake.x += (this.ShakeSpeed.x *= 0.8f);
    this.ShakeSpeed.y += (float) ((0.0 - (double) this.Shake.y) * 0.20000000298023224);
    this.Shake.y += (this.ShakeSpeed.y *= 0.8f);
    this.Parent.localPosition = (Vector3) this.Shake;
    this.RadialProgressBar.fillAmount = Mathf.SmoothStep(this.SmoothStepStart, this.SmoothStepTarget, this.SmoothStepRadial += 2f * Time.deltaTime);
  }

  public void UnlockCard()
  {
    this.Card.Unlocked = true;
    this.RadialProgressBarGameObject.SetActive(!this.Card.Unlocked);
    this.Spine.AnimationState.SetAnimation(0, "menu-reveal", false);
    this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.CompletedAnimation);
    TarotCards.UnlockTrinket(this.Card.Type);
    this.newCardIcon.SetActive(true);
  }

  private void CompletedAnimation(TrackEntry trackEntry)
  {
    this.Unlocking = false;
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompletedAnimation);
    this.Spine.AnimationState.SetAnimation(0, "menu-static", true);
    this.ControlPrompt.SetActive(false);
  }

  private void OnDisable()
  {
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.CompletedAnimation);
  }

  public void Deselected() => this.ControlPrompt.SetActive(false);

  public void Selected() => this.ControlPrompt.SetActive(!this.Card.Unlocked);

  public void SetCard(TarotCards Card)
  {
    this.Card = Card;
    Card.Unlocked = !TarotCards.IsUnlocked(Card.Type);
    this.Spine.AnimationState.SetAnimation(0, Card.Unlocked ? "menu-static" : "menu-static-back", true);
    this.RadialProgressBarGameObject.SetActive(false);
    this.ControlPrompt.SetActive(false);
  }

  public void SetSkin(string Skin) => this.Spine.Skeleton.SetSkin(Skin);
}
