// Decompiled with JetBrains decompiler
// Type: IntroRoomMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using UnityEngine;

#nullable disable
public class IntroRoomMusicController : BaseMonoBehaviour
{
  public AmbientMusicController.Track AmbientTrack;
  public AmbientMusicController.Track AmbientNatureTrack;
  [EventRef]
  public string atmosEventPath;
  public EventInstance atmosEventInstance;
  [EventRef]
  public string natureEventPath;
  public EventInstance natureEventInstance;
  public TriggerCanvasGroup MMTrigger;
  public AmbientMusicController.Track MMTrack;
  [EventRef]
  public string mmEventPath;
  public TriggerCanvasGroup DDTrigger;
  public AmbientMusicController.Track DDTrack;
  [EventRef]
  public string ddEventPath;
  public TriggerCanvasGroup BridgeTrigger;
  public AmbientMusicController.Track BridgeTrack;
  [EventRef]
  public string bridgeEventPath;
  public AmbientMusicController.Track BassTrack;
  [EventRef]
  public string bassEventPath;
  public EventInstance bassEventInstance;
  public GameObject BassFadeObject;
  public float MaxDistance = 20f;
  public AmbientMusicController.Track ExecutionTrack;
  [EventRef]
  public string ExecutionEventPath;
  [EventRef]
  public string biomeMusicEventPath;
  public GameObject Player;

  public void Start()
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", this.gameObject);
    this.atmosEventInstance = AudioManager.Instance.CreateLoop(this.atmosEventPath, true);
    this.bassEventInstance = AudioManager.Instance.CreateLoop(this.bassEventPath);
    if (this.bassEventInstance.isValid())
    {
      int num1 = (int) this.bassEventInstance.setParameterByName(SoundParams.Intensity, 0.0f);
      int num2 = (int) this.bassEventInstance.start();
    }
    this.MMTrigger.OnTriggered += new TriggerCanvasGroup.Triggered(this.MMTriggered);
    this.DDTrigger.OnTriggered += new TriggerCanvasGroup.Triggered(this.DDTriggered);
    this.BridgeTrigger.OnTriggered += new TriggerCanvasGroup.Triggered(this.BridgeTriggered);
  }

  public void MMTriggered() => AudioManager.Instance.PlayOneShot(this.mmEventPath);

  public void DDTriggered() => AudioManager.Instance.PlayOneShot(this.ddEventPath);

  public void BridgeTriggered() => AudioManager.Instance.PlayOneShot(this.bridgeEventPath);

  public void PlayExecutionTrack()
  {
    AudioManager.Instance.StopLoop(this.bassEventInstance);
    AudioManager.Instance.PlayOneShot(this.ExecutionEventPath);
  }

  public void PlayAmbientNature()
  {
    if (this.natureEventInstance.isValid())
      return;
    this.natureEventInstance = AudioManager.Instance.CreateLoop(this.natureEventPath, true);
  }

  public void PlayCombatMusic()
  {
    AudioManager.Instance.PlayMusic(this.biomeMusicEventPath);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    AudioManager.Instance.SetMusicCombatState();
  }

  public void OnDestroy()
  {
    this.MMTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.MMTriggered);
    this.DDTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.DDTriggered);
    this.BridgeTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.BridgeTriggered);
    this.StopAll();
  }

  public void StopAll()
  {
    AudioManager.Instance.StopLoop(this.atmosEventInstance);
    AudioManager.Instance.StopLoop(this.natureEventInstance);
    AudioManager.Instance.StopLoop(this.bassEventInstance);
  }

  public void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null || !this.bassEventInstance.isValid())
      return;
    float num1 = Vector3.Distance(this.Player.transform.position, this.BassFadeObject.transform.position);
    if ((double) this.Player.transform.position.y > (double) this.BassFadeObject.transform.position.y)
      return;
    int num2 = (int) this.bassEventInstance.setParameterByName(SoundParams.Intensity, 1f - Mathf.Clamp01(num1 / this.MaxDistance));
  }

  public void OnDrawGizmos()
  {
    if (!((Object) this.BassFadeObject != (Object) null))
      return;
    Utils.DrawCircleXY(this.BassFadeObject.transform.position, this.MaxDistance, Color.yellow);
  }
}
