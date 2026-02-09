// Decompiled with JetBrains decompiler
// Type: IntroDeathSceneMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using UnityEngine;

#nullable disable
public class IntroDeathSceneMusicController : BaseMonoBehaviour
{
  [EventRef]
  public string ambientEventPath;
  public EventInstance ambientEventInstance;
  public float AmbientFadeIn = 12f;
  [EventRef]
  public string bassEventPath;
  public EventInstance bassEventInstance;
  public GameObject BassFadeObject;
  public float MaxDistance = 20f;
  [EventRef]
  public string crownEventPath;
  public GameObject Player;
  public float Timer;

  public void OnEnable()
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

  public void Update()
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

  public void OnDisable() => this.StopAll();

  public void OnDrawGizmos()
  {
    if (!((Object) this.BassFadeObject != (Object) null))
      return;
    Utils.DrawCircleXY(this.BassFadeObject.transform.position, this.MaxDistance, Color.yellow);
  }
}
