// Decompiled with JetBrains decompiler
// Type: TriggerSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMTools;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TriggerSound : MonoBehaviour
{
  public bool Activated;
  public float ActivatedTimer = 3f;
  public float Progress;
  public string DefaultAnimation;
  public UnitObject player;
  public Collider2D PlayerCollision;
  [EventRef]
  public string VOtoPlay = "event:/enemy/vocals/humanoid/warning";
  public GameObject p;
  public float ActivateDistance = 0.666f;
  public bool UsePlayerPrisoner;
  public UnityEvent Callback;
  public bool PlayOnce;
  public float Distance;
  public bool foundPlayer;

  public void PushPlayer()
  {
    AudioManager.Instance.PlayOneShot(this.VOtoPlay, this.gameObject);
    this.Callback?.Invoke();
  }

  public void Start() => this.FindPlayer();

  public void FindPlayer()
  {
    if (!((Object) this.p == (Object) null))
      return;
    if (this.UsePlayerPrisoner)
    {
      if (!((Object) PlayerPrisonerController.Instance != (Object) null))
        return;
      this.p = PlayerPrisonerController.Instance.gameObject;
      this.player = this.p.GetComponent<UnitObject>();
      this.foundPlayer = true;
    }
    else
    {
      if (!((Object) PlayerFarming.Instance != (Object) null))
        return;
      this.p = PlayerFarming.Instance.gameObject;
      this.player = this.p.GetComponent<UnitObject>();
      this.foundPlayer = true;
    }
  }

  public void Update()
  {
    if ((Object) this.player == (Object) null)
    {
      if (this.foundPlayer)
        return;
      this.FindPlayer();
    }
    else
    {
      if (MMConversation.isPlaying)
        return;
      this.Distance = Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position);
      if ((double) Vector3.Distance(this.gameObject.transform.position, this.player.gameObject.transform.position) < (double) this.ActivateDistance && !this.Activated)
      {
        this.Progress = 0.0f;
        this.Activated = true;
        AudioManager.Instance.PlayOneShot(this.VOtoPlay, this.gameObject);
        this.Callback?.Invoke();
      }
      if (!this.Activated || this.PlayOnce)
        return;
      if ((double) this.Progress < (double) this.ActivatedTimer)
        this.Progress += Time.deltaTime;
      else
        this.Activated = false;
    }
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.ActivateDistance, Color.green);
  }
}
