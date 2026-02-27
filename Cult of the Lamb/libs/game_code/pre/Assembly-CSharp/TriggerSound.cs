// Decompiled with JetBrains decompiler
// Type: TriggerSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMTools;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TriggerSound : MonoBehaviour
{
  private bool Activated;
  public float ActivatedTimer = 3f;
  private float Progress;
  private string DefaultAnimation;
  private UnitObject player;
  private Collider2D PlayerCollision;
  [EventRef]
  public string VOtoPlay = "event:/enemy/vocals/humanoid/warning";
  private GameObject p;
  public float ActivateDistance = 0.666f;
  public bool UsePlayerPrisoner;
  public UnityEvent Callback;
  public bool PlayOnce;
  public float Distance;
  private bool foundPlayer;

  public void PushPlayer()
  {
    AudioManager.Instance.PlayOneShot(this.VOtoPlay, this.gameObject);
    this.Callback?.Invoke();
  }

  private void Start() => this.FindPlayer();

  private void FindPlayer()
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

  private void Update()
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

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.ActivateDistance, Color.green);
  }
}
