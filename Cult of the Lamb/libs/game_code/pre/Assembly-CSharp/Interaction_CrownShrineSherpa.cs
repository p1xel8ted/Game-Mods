// Decompiled with JetBrains decompiler
// Type: Interaction_CrownShrineSherpa
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CrownShrineSherpa : Interaction
{
  [SerializeField]
  private GameObject ShrineNormal;
  [SerializeField]
  private GameObject ShrineHighlight;
  [SerializeField]
  private GameObject LightingOverride;
  [SerializeField]
  private GameObject FloorMarkings;
  [SerializeField]
  private GameObject TargetPosition;

  public override void GetLabel()
  {
    if (this.Interactable)
      this.Label = ScriptLocalization.Interactions.Pray;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    PlayerFarming.Instance.GoToAndStop(this.TargetPosition.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.InteractIE())));
  }

  private void Start()
  {
    this.ShrineNormal.SetActive(true);
    this.ShrineHighlight.SetActive(false);
    this.FloorMarkings.SetActive(false);
    this.LightingOverride.SetActive(false);
    this.ActivateDistance = 2f;
  }

  private IEnumerator InteractIE()
  {
    Interaction_CrownShrineSherpa crownShrineSherpa = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
    PlayerFarming.Instance.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    crownShrineSherpa.LightingOverride.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    crownShrineSherpa.ShrineNormal.SetActive(false);
    crownShrineSherpa.ShrineHighlight.SetActive(true);
    crownShrineSherpa.FloorMarkings.SetActive(true);
    crownShrineSherpa.gameObject.transform.DOShakePosition(1f, new Vector3(0.25f, 0.0f, 0.0f));
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(crownShrineSherpa.gameObject, "Interactions/CrownShrine/Convo")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", PlayerFarming.Instance.transform.position);
    while (MMConversation.isPlaying)
      yield return (object) null;
    crownShrineSherpa.LightingOverride.SetActive(false);
    DataManager.Instance.PrayedAtCrownShrine = true;
    crownShrineSherpa.Interactable = false;
    GameManager.GetInstance().OnConversationEnd();
  }
}
