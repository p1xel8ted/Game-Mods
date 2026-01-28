// Decompiled with JetBrains decompiler
// Type: Interaction_NextFloor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_NextFloor : Interaction
{
  public SpriteRenderer spriteRenderer;
  public bool AlreadyUnlocked;
  public int Cost = 1;
  public GameObject darkness;
  public GoopFade GoopFade;
  public string sNextLayer;
  public GameObject BlockingCollider;
  public bool _playedSFX;
  public bool Activating;

  public void Start()
  {
    this.GoopFade.gameObject.SetActive(false);
    this.AutomaticallyInteract = true;
    this.UpdateLocalisation();
    this.AlreadyUnlocked = false;
  }

  public void DestroyEverything()
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

  public IEnumerator PlayOneShot()
  {
    Interaction_NextFloor interactionNextFloor = this;
    yield return (object) new WaitForSeconds(0.33f);
    if (!interactionNextFloor._playedSFX)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/end_floor", interactionNextFloor.playerFarming.gameObject);
      interactionNextFloor._playedSFX = true;
    }
  }

  public IEnumerator PaySoulsRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(0.5f);
    this.BlockingCollider.SetActive(false);
    this.DestroyEverything();
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__11_0()
  {
    this.StartCoroutine((IEnumerator) this.PaySoulsRoutine());
  }
}
