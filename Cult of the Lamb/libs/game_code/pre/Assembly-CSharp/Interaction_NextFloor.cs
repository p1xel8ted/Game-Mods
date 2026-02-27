// Decompiled with JetBrains decompiler
// Type: Interaction_NextFloor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_NextFloor : Interaction
{
  public SpriteRenderer spriteRenderer;
  private bool AlreadyUnlocked;
  private int Cost = 1;
  public GameObject darkness;
  public GoopFade GoopFade;
  private string sNextLayer;
  public GameObject BlockingCollider;
  private bool _playedSFX;
  private bool Activating;

  private void Start()
  {
    this.GoopFade.gameObject.SetActive(false);
    this.AutomaticallyInteract = true;
    this.UpdateLocalisation();
    this.AlreadyUnlocked = false;
  }

  private void DestroyEverything()
  {
    Object.Destroy((Object) this.darkness);
    Object.Destroy((Object) this.gameObject);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sNextLayer = ScriptLocalization.Interactions.UnlockDoor;
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sNextLayer;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.BlockingCollider.SetActive(false);
    this.GoopFade.gameObject.SetActive(true);
    this.GoopFade.FadeIn();
    AudioManager.Instance.PlayOneShot("event:/enter_leave_buildings/enter_building", this.transform.position);
    this.DestroyEverything();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.StartCoroutine((IEnumerator) this.PlayOneShot());
  }

  private IEnumerator PlayOneShot()
  {
    yield return (object) new WaitForSeconds(0.33f);
    if (!this._playedSFX)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/end_floor", PlayerFarming.Instance.gameObject);
      this._playedSFX = true;
    }
  }

  private IEnumerator PaySoulsRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(0.5f);
    this.BlockingCollider.SetActive(false);
    this.DestroyEverything();
    GameManager.GetInstance().OnConversationEnd();
  }
}
