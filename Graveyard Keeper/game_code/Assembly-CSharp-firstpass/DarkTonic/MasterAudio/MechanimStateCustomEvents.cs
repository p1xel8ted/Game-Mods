// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.MechanimStateCustomEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class MechanimStateCustomEvents : StateMachineBehaviour
{
  [Header("Retrigger Events Each Time Anim Loops w/o Exiting State")]
  [Tooltip("Select for event to re-fire each time animation loops without exiting state")]
  public bool RetriggerWhenStateLoops;
  [Header("Enter Custom Event")]
  [Tooltip("Fire A Custom Event When State Is Entered")]
  public bool fireEnterEvent;
  [MasterCustomEvent]
  public string enterCustomEvent = "[None]";
  [Tooltip("Fire a Custom Event when state is Exited")]
  [Header("Exit Custom Event")]
  public bool fireExitEvent;
  [MasterCustomEvent]
  public string exitCustomEvent = "[None]";
  [Tooltip("Fire a Custom Event timed to the animation state's normalized time.  Normalized time is simply the length in time of the animation.  Time is represented as a float from 0f - 1f.  0f is the beginning, .5f is the middle, 1f is the end...etc.etc.  Select a Start time from 0 - 1.")]
  [Header("Fire Custom EventTimed to Animation")]
  public bool fireAnimTimeEvent;
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  [Range(0.0f, 1f)]
  public float whenToFireEvent;
  [MasterCustomEvent]
  public string timedCustomEvent = "[None]";
  [Header("Fire Multiple Custom Events Timed to Anim")]
  [Tooltip("Fire a Custom Event with timed to the animation.  This allows you to time your Custom Events to the actions in you animation. Select the number of Custom Events to be fired, up to 4. Then set the time you want each Custom Event to fire with each subsequent time greater than the previous time.")]
  public bool fireMultiAnimTimeEvent;
  [Range(0.0f, 4f)]
  public int numOfMultiEventsToFire;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToFireMultiEvent1;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToFireMultiEvent2;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToFireMultiEvent3;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToFireMultiEvent4;
  [MasterCustomEvent]
  public string MultiTimedEvent = "[None]";
  public bool _playMultiEvent1 = true;
  public bool _playMultiEvent2 = true;
  public bool _playMultiEvent3 = true;
  public bool _playMultiEvent4 = true;
  public bool _fireTimedEvent = true;
  public Transform _actorTrans;
  public int _lastRepetition = -1;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this._lastRepetition = 0;
    this._actorTrans = this.ActorTrans(animator);
    if (!this.fireEnterEvent || this.enterCustomEvent == "[None]" || string.IsNullOrEmpty(this.enterCustomEvent))
      return;
    DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.enterCustomEvent, this._actorTrans);
  }

  public override void OnStateUpdate(
    Animator animator,
    AnimatorStateInfo stateInfo,
    int layerIndex)
  {
    int normalizedTime = (int) stateInfo.normalizedTime;
    float num = stateInfo.normalizedTime - (float) normalizedTime;
    if (this.fireAnimTimeEvent)
    {
      if (!this._fireTimedEvent && this.RetriggerWhenStateLoops && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
        this._fireTimedEvent = true;
      if (this._fireTimedEvent && (double) num > (double) this.whenToFireEvent)
      {
        this._fireTimedEvent = false;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.timedCustomEvent, this._actorTrans);
      }
    }
    if (this.fireMultiAnimTimeEvent)
    {
      if (this.RetriggerWhenStateLoops)
      {
        if (!this._playMultiEvent1 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this._playMultiEvent1 = true;
        if (!this._playMultiEvent2 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this._playMultiEvent2 = true;
        if (!this._playMultiEvent3 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this._playMultiEvent3 = true;
        if (!this._playMultiEvent4 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this._playMultiEvent4 = true;
      }
      if (this._playMultiEvent1 && (double) num >= (double) this.whenToFireMultiEvent1 && this.numOfMultiEventsToFire >= 1)
      {
        this._playMultiEvent1 = false;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.MultiTimedEvent, this._actorTrans);
      }
      if (this._playMultiEvent2 && (double) num >= (double) this.whenToFireMultiEvent2 && this.numOfMultiEventsToFire >= 2)
      {
        this._playMultiEvent2 = false;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.MultiTimedEvent, this._actorTrans);
      }
      if (this._playMultiEvent3 && (double) num >= (double) this.whenToFireMultiEvent3 && this.numOfMultiEventsToFire >= 3)
      {
        this._playMultiEvent3 = false;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.MultiTimedEvent, this._actorTrans);
      }
      if (this._playMultiEvent4 && (double) num >= (double) this.whenToFireMultiEvent4 && this.numOfMultiEventsToFire >= 4)
      {
        this._playMultiEvent4 = false;
        DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.MultiTimedEvent, this._actorTrans);
      }
    }
    this._lastRepetition = normalizedTime;
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (this.fireExitEvent && this.exitCustomEvent != "[None]" && !string.IsNullOrEmpty(this.exitCustomEvent))
      DarkTonic.MasterAudio.MasterAudio.FireCustomEvent(this.exitCustomEvent, this._actorTrans);
    if (this.fireMultiAnimTimeEvent)
    {
      this._playMultiEvent1 = true;
      this._playMultiEvent2 = true;
      this._playMultiEvent3 = true;
      this._playMultiEvent4 = true;
    }
    if (!this.fireAnimTimeEvent)
      return;
    this._fireTimedEvent = true;
  }

  public Transform ActorTrans(Animator anim)
  {
    if ((Object) this._actorTrans != (Object) null)
      return this._actorTrans;
    this._actorTrans = anim.transform;
    return this._actorTrans;
  }
}
