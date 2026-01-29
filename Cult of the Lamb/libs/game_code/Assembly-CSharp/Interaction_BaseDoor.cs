// Decompiled with JetBrains decompiler
// Type: Interaction_BaseDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_BaseDoor : Interaction
{
  public GameObject PlayerPosition;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject DoorToMove;
  public int Cost = 2;
  public GameObject ReceiveDevotion;
  public Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4f);
  public static Interaction_BaseDoor Instance;
  public BoxCollider2D CollideForDoor;
  public bool Unlocked;
  public string sOpenDoor;
  public bool Used;

  public override void OnEnableInteraction()
  {
    this.ActivateDistance = 3f;
    base.OnEnableInteraction();
    this.Unlocked = DataManager.Instance.UnlockedBossTempleDoor.Contains(FollowerLocation.Base);
    this.DoorToMove.transform.localPosition = !this.Unlocked ? Vector3.zero : this.OpenDoorPosition;
    this.Used = false;
    this.OpenDoor();
    Interaction_BaseDoor.Instance = this;
  }

  public void Start() => this.UpdateLocalisation();

  public void OpenDoor()
  {
    if (this.Unlocked)
      this.CollideForDoor.enabled = true;
    else
      this.CollideForDoor.enabled = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sOpenDoor = ScriptLocalization.Interactions.UnlockDoor;
  }

  public override void GetLabel() => this.Label = "";

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.Followers.Count < this.Cost)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      this.SimpleSetCamera.Play();
      this.playerFarming.GoToAndStop(this.PlayerPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.EnterTemple())));
    }
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.FrameDelayOpenDoor());

  public IEnumerator FrameDelayOpenDoor()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_BaseDoor interactionBaseDoor = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionBaseDoor.StartCoroutine((IEnumerator) interactionBaseDoor.EnterTemple());
      GameManager.GetInstance().OnConversationNew();
      interactionBaseDoor.SimpleSetCamera.Play();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator EnterTemple()
  {
    Interaction_BaseDoor interactionBaseDoor = this;
    yield return (object) new WaitForSeconds(2.5f);
    if (!DataManager.Instance.UnlockedBossTempleDoor.Contains(FollowerLocation.Base))
      DataManager.Instance.UnlockedBossTempleDoor.Add(FollowerLocation.Base);
    AudioManager.Instance.PlayOneShot("event:/door/door_unlock", interactionBaseDoor.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", interactionBaseDoor.gameObject);
    Vector3 StartingPosition = interactionBaseDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionBaseDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + interactionBaseDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", interactionBaseDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseDoor.SimpleSetCamera.Reset();
    interactionBaseDoor.Unlocked = true;
    interactionBaseDoor.OpenDoor();
    DataManager.Instance.ShrineDoor = true;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadWorldMapAssets(), (System.Action) (() =>
    {
      UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
      mapMenuController.Show();
      mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() => this.Used = false);
    })));
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__15_0()
  {
    this.StartCoroutine((IEnumerator) this.EnterTemple());
  }

  [CompilerGenerated]
  public void \u003COnTriggerEnter2D\u003Eb__20_0()
  {
    UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
    mapMenuController.Show();
    mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() => this.Used = false);
  }

  [CompilerGenerated]
  public void \u003COnTriggerEnter2D\u003Eb__20_1() => this.Used = false;
}
