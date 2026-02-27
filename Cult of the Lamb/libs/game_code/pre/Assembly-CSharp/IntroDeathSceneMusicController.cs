// Decompiled with JetBrains decompiler
// Type: IntroDeathSceneMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using UnityEngine;

#nullable disable
public class IntroDeathSceneMusicController : BaseMonoBehaviour
{
  [EventRef]
  public string ambientEventPath;
  private EventInstance ambientEventInstance;
  private float AmbientFadeIn = 12f;
  [EventRef]
  public string bassEventPath;
  private EventInstance bassEventInstance;
  public GameObject BassFadeObject;
  public float MaxDistance = 20f;
  [EventRef]
  public string crownEventPath;
  private GameObject Player;
  private float Timer;

  private void OnEnable()
  {
    AudioManager.Instance.StopCurrentAtmos();
    this.ambientEventInstance = AudioManager.Instance.CreateLoop(this.ambientEventPath, true);
    this.bassEventInstance = AudioManager.Instance.CreateLoop(this.bassEventPath);
    if (!this.bassEventInstance.isValid())
      return;
    int num1 = (int) this.bassEventInstance.setParameterByName(SoundParams.Intensity, 0.0f);
    int num2 = (int) this.bassEventInstance.start();
  }

  public void PlaySpawnCrown()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      return;
    AudioManager.Instance.PlayOneShot(this.crownEventPath, this.Player.transform.position);
  }

  public void StopAll()
  {
    AudioManager.Instance.StopLoop(this.ambientEventInstance);
    AudioManager.Instance.StopLoop(this.bassEventInstance);
  }

  private void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null)
      return;
    float num1 = Vector3.Distance(this.Player.transform.position, this.BassFadeObject.transform.position);
    if (this.bassEventInstance.isValid() && (double) this.Player.transform.position.y <= (double) this.BassFadeObject.transform.position.y)
    {
      int num2 = (int) this.bassEventInstance.setParameterByName(SoundParams.Intensity, 1f - Mathf.Clamp01(num1 / this.MaxDistance));
    }
    if (!this.ambientEventInstance.isValid() || (double) this.Player.transform.position.y > (double) this.BassFadeObject.transform.position.y)
      return;
    int num3 = (int) this.ambientEventInstance.setParameterByName(SoundParams.Intensity, Mathf.Clamp01(num1 / this.MaxDistance));
  }

  private void OnDisable() => this.StopAll();

  private void OnDrawGizmos()
  {
    if (!((Object) this.BassFadeObject != (Object) null))
      return;
    Utils.DrawCircleXY(this.BassFadeObject.transform.position, this.MaxDistance, Color.yellow);
  }
}
