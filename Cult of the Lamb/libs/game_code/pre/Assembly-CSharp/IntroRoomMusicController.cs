// Decompiled with JetBrains decompiler
// Type: IntroRoomMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private EventInstance atmosEventInstance;
  [EventRef]
  public string natureEventPath;
  private EventInstance natureEventInstance;
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
  private EventInstance bassEventInstance;
  public GameObject BassFadeObject;
  public float MaxDistance = 20f;
  public AmbientMusicController.Track ExecutionTrack;
  [EventRef]
  public string ExecutionEventPath;
  [EventRef]
  public string biomeMusicEventPath;
  private GameObject Player;

  private void Start()
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

  private void OnDisable()
  {
    this.MMTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.MMTriggered);
    this.DDTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.DDTriggered);
    this.BridgeTrigger.OnTriggered -= new TriggerCanvasGroup.Triggered(this.BridgeTriggered);
    this.StopAll();
  }

  private void MMTriggered() => AudioManager.Instance.PlayOneShot(this.mmEventPath);

  private void DDTriggered() => AudioManager.Instance.PlayOneShot(this.ddEventPath);

  private void BridgeTriggered() => AudioManager.Instance.PlayOneShot(this.bridgeEventPath);

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

  private void OnDestroy() => this.StopAll();

  public void StopAll()
  {
    AudioManager.Instance.StopLoop(this.atmosEventInstance);
    AudioManager.Instance.StopLoop(this.natureEventInstance);
    AudioManager.Instance.StopLoop(this.bassEventInstance);
  }

  private void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null || !this.bassEventInstance.isValid())
      return;
    float num1 = Vector3.Distance(this.Player.transform.position, this.BassFadeObject.transform.position);
    if ((double) this.Player.transform.position.y > (double) this.BassFadeObject.transform.position.y)
      return;
    int num2 = (int) this.bassEventInstance.setParameterByName(SoundParams.Intensity, 1f - Mathf.Clamp01(num1 / this.MaxDistance));
  }

  private void OnDrawGizmos()
  {
    if (!((Object) this.BassFadeObject != (Object) null))
      return;
    Utils.DrawCircleXY(this.BassFadeObject.transform.position, this.MaxDistance, Color.yellow);
  }
}
