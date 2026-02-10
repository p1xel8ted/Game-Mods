// Decompiled with JetBrains decompiler
// Type: Interaction_CrownShrineSherpa
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_CrownShrineSherpa : Interaction
{
  [SerializeField]
  public GameObject ShrineNormal;
  [SerializeField]
  public GameObject ShrineHighlight;
  [SerializeField]
  public GameObject LightingOverride;
  [SerializeField]
  public GameObject FloorMarkings;
  [SerializeField]
  public GameObject TargetPosition;

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
    this.playerFarming.GoToAndStop(this.TargetPosition.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.InteractIE())));
  }

  public void Start()
  {
    this.ShrineNormal.SetActive(true);
    this.ShrineHighlight.SetActive(false);
    this.FloorMarkings.SetActive(false);
    this.LightingOverride.SetActive(false);
    this.ActivateDistance = 2f;
  }

  public IEnumerator InteractIE()
  {
    Interaction_CrownShrineSherpa crownShrineSherpa = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", crownShrineSherpa.playerFarming.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(crownShrineSherpa.playerFarming.gameObject, 12f);
    crownShrineSherpa.playerFarming.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    crownShrineSherpa.LightingOverride.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    crownShrineSherpa.ShrineNormal.SetActive(false);
    crownShrineSherpa.ShrineHighlight.SetActive(true);
    crownShrineSherpa.FloorMarkings.SetActive(true);
    crownShrineSherpa.gameObject.transform.DOShakePosition(1f, new Vector3(0.25f, 0.0f, 0.0f));
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success, crownShrineSherpa.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(crownShrineSherpa.gameObject, "Interactions/CrownShrine/Convo")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", crownShrineSherpa.playerFarming.transform.position);
    while (MMConversation.isPlaying)
      yield return (object) null;
    crownShrineSherpa.LightingOverride.SetActive(false);
    DataManager.Instance.PrayedAtCrownShrine = true;
    crownShrineSherpa.Interactable = false;
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__6_0()
  {
    this.StartCoroutine((IEnumerator) this.InteractIE());
  }
}
