// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.MechanimStateSounds
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class MechanimStateSounds : StateMachineBehaviour
{
  [Header("Select For Sounds To Follow Object")]
  public bool SoundFollowsObject;
  [Tooltip("Select for sounds to retrigger each time animation loops without exiting state")]
  [Header("Retrigger Sounds Each Time Anim Loops w/o Exiting State")]
  public bool RetriggerWhenStateLoops;
  [Header("Enter Sound Group")]
  [Tooltip("Play a Sound Group when state is Entered")]
  public bool playEnterSound;
  public bool stopEnterSoundOnExit;
  [SoundGroup]
  public string enterSoundGroup = "[None]";
  [Tooltip("Random Variation plays if blank, otherwise name a Variation from the above Sound Group to play.")]
  public string enterVariationName;
  public bool wasEnterSoundPlayed;
  [Tooltip("Play a Sound Group when state is Exited")]
  [Header("Exit Sound Group")]
  public bool playExitSound;
  [SoundGroup]
  public string exitSoundGroup = "[None]";
  [Tooltip("Random Variation plays if blank, otherwise name a Variation from the above Sound Group to play.")]
  public string exitVariationName;
  [Tooltip("Play a Sound Group (Normal or Looped Chain Variation Mode) timed to the animation state's normalized time.  Normalized time is simply the length in time of the animation.  Time is represented as a float from 0f - 1f.  0f is the beginning, .5f is the middle, 1f is the end...etc.etc.  Select a Start time from 0 - 1.  Select a stop time greater than the start time or leave stop time equals to zero and select Stop Anim Time Sound On Exit.  This can be used for Looped Chain Sound Groups since you have to define a stop time.")]
  [Header("Play Sound Group Timed to Animation")]
  public bool playAnimTimeSound;
  public bool stopAnimTimeSoundOnExit;
  [Tooltip("If selected, When To Stop Sound (below) will be used. Otherwise the sound will not be stopped unless you have Stop Anim Time Sound On Exit selected above.")]
  public bool useStopTime;
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  [Range(0.0f, 1f)]
  public float whenToStartSound;
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  [Range(0.0f, 1f)]
  public float whenToStopSound;
  [SoundGroup]
  public string TimedSoundGroup = "[None]";
  [Tooltip("Random Variation plays if blank, otherwise name a Variation from the above Sound Group to play.")]
  public string timedVariationName;
  public bool playSoundStart = true;
  public bool playSoundStop = true;
  [Tooltip("Play a Sound Group with each variation timed to the animation.  This allows you to time your sounds to the actions in you animation.  Example: A sword swing combo where you want swoosh soundsor character grunts timed to the acceleration phase of the sword swing.  Select the number of sounds to be played, up to 4.  Then set the time you want each sound to start with each subsequent time greater than the previous time.")]
  [Header("Play Multiple Sounds Timed to Anim")]
  public bool playMultiAnimTimeSounds;
  public bool StopMultiAnimTimeSoundsOnExit;
  [Range(0.0f, 4f)]
  public int numOfMultiSoundsToPlay;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToStartMultiSound1;
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  [Range(0.0f, 1f)]
  public float whenToStartMultiSound2;
  [Range(0.0f, 1f)]
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  public float whenToStartMultiSound3;
  [Tooltip("This value will be compared to the normalizedTime of the animation you are playing. NormalizedTime is represented as a float so 0 is the beginning, 1 is the end and .5f would be the middle etc.")]
  [Range(0.0f, 1f)]
  public float whenToStartMultiSound4;
  [SoundGroup]
  public string MultiSoundsTimedGroup = "[None]";
  [Tooltip("Random Variation plays if blank, otherwise name a Variation from the above Sound Group to play.")]
  public string multiTimedVariationName;
  public bool playMultiSound1 = true;
  public bool playMultiSound2 = true;
  public bool playMultiSound3 = true;
  public bool playMultiSound4 = true;
  public Transform _actorTrans;
  public int _lastRepetition = -1;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this._lastRepetition = 0;
    this._actorTrans = this.ActorTrans(animator);
    if (!this.playEnterSound)
      return;
    string variationName = MechanimStateSounds.GetVariationName(this.enterVariationName);
    if (this.SoundFollowsObject)
    {
      if (variationName == null)
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.enterSoundGroup, this._actorTrans);
      else
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.enterSoundGroup, this._actorTrans, variationName: variationName);
    }
    else if (variationName == null)
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.enterSoundGroup, this._actorTrans);
    else
      DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.enterSoundGroup, this._actorTrans, variationName: variationName);
    this.wasEnterSoundPlayed = true;
  }

  public override void OnStateUpdate(
    Animator animator,
    AnimatorStateInfo stateInfo,
    int layerIndex)
  {
    int normalizedTime = (int) stateInfo.normalizedTime;
    float num = stateInfo.normalizedTime - (float) normalizedTime;
    if (this.playAnimTimeSound)
    {
      if (!this.playSoundStart && this.RetriggerWhenStateLoops && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
        this.playSoundStart = true;
      if (this.playSoundStart && (double) num > (double) this.whenToStartSound)
      {
        this.playSoundStart = false;
        if (this.useStopTime && (double) this.whenToStopSound < (double) this.whenToStartSound)
        {
          Debug.LogError((object) "Stop time must be greater than start time when Use Stop Time is selected.");
        }
        else
        {
          string variationName = MechanimStateSounds.GetVariationName(this.timedVariationName);
          if (this.SoundFollowsObject)
          {
            if (variationName == null)
              DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.TimedSoundGroup, this._actorTrans);
            else
              DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.TimedSoundGroup, this._actorTrans, variationName: variationName);
          }
          else if (variationName == null)
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.TimedSoundGroup, this._actorTrans);
          else
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.TimedSoundGroup, this._actorTrans, variationName: variationName);
        }
      }
      if (this.useStopTime && this.playSoundStop && (double) num > (double) this.whenToStartSound && !this.stopAnimTimeSoundOnExit && (double) num > (double) this.whenToStopSound)
      {
        this.playSoundStop = false;
        DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this._actorTrans, this.TimedSoundGroup);
      }
    }
    if (this.playMultiAnimTimeSounds)
    {
      if (this.RetriggerWhenStateLoops)
      {
        if (!this.playMultiSound1 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this.playMultiSound1 = true;
        if (!this.playMultiSound2 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this.playMultiSound2 = true;
        if (!this.playMultiSound3 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this.playMultiSound3 = true;
        if (!this.playMultiSound4 && this._lastRepetition >= 0 && normalizedTime > this._lastRepetition)
          this.playMultiSound4 = true;
      }
      string variationName = MechanimStateSounds.GetVariationName(this.multiTimedVariationName);
      if (this.playMultiSound1 && (double) num > (double) this.whenToStartMultiSound1 && this.numOfMultiSoundsToPlay >= 1)
      {
        this.playMultiSound1 = false;
        if (this.SoundFollowsObject)
        {
          if (variationName == null)
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
          else
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
        }
        else if (variationName == null)
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
        else
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
      }
      if (this.playMultiSound2 && (double) num > (double) this.whenToStartMultiSound2 && this.numOfMultiSoundsToPlay >= 2)
      {
        this.playMultiSound2 = false;
        if (this.SoundFollowsObject)
        {
          if (variationName == null)
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
          else
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
        }
        else if (variationName == null)
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
        else
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
      }
      if (this.playMultiSound3 && (double) num > (double) this.whenToStartMultiSound3 && this.numOfMultiSoundsToPlay >= 3)
      {
        this.playMultiSound3 = false;
        if (this.SoundFollowsObject)
        {
          if (variationName == null)
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
          else
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
        }
        else if (variationName == null)
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
        else
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
      }
      if (this.playMultiSound4 && (double) num > (double) this.whenToStartMultiSound4 && this.numOfMultiSoundsToPlay >= 4)
      {
        this.playMultiSound4 = false;
        if (this.SoundFollowsObject)
        {
          if (variationName == null)
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
          else
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
        }
        else if (variationName == null)
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans);
        else
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.MultiSoundsTimedGroup, this._actorTrans, variationName: variationName);
      }
    }
    this._lastRepetition = normalizedTime;
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (this.wasEnterSoundPlayed && this.stopEnterSoundOnExit)
      DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this._actorTrans, this.enterSoundGroup);
    this.wasEnterSoundPlayed = false;
    if (this.playExitSound)
    {
      string variationName = MechanimStateSounds.GetVariationName(this.exitVariationName);
      if (this.SoundFollowsObject)
      {
        if (variationName == null)
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.exitSoundGroup, this._actorTrans);
        else
          DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(this.exitSoundGroup, this._actorTrans, variationName: variationName);
      }
      else if (variationName == null)
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.exitSoundGroup, this._actorTrans);
      else
        DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(this.exitSoundGroup, this._actorTrans, variationName: variationName);
    }
    if (this.playAnimTimeSound)
    {
      if (this.stopAnimTimeSoundOnExit)
        DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this._actorTrans, this.TimedSoundGroup);
      this.playSoundStart = true;
      this.playSoundStop = true;
    }
    if (!this.playMultiAnimTimeSounds)
      return;
    if (this.StopMultiAnimTimeSoundsOnExit)
      DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(this._actorTrans, this.MultiSoundsTimedGroup);
    this.playMultiSound1 = true;
    this.playMultiSound2 = true;
    this.playMultiSound3 = true;
    this.playMultiSound4 = true;
  }

  public Transform ActorTrans(Animator anim)
  {
    if ((Object) this._actorTrans != (Object) null)
      return this._actorTrans;
    this._actorTrans = anim.transform;
    return this._actorTrans;
  }

  public static string GetVariationName(string varName)
  {
    if (string.IsNullOrEmpty(varName))
      return (string) null;
    varName = varName.Trim();
    return string.IsNullOrEmpty(varName) ? (string) null : varName;
  }
}
