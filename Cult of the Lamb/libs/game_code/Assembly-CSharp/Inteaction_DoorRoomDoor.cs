// Decompiled with JetBrains decompiler
// Type: Inteaction_DoorRoomDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Inteaction_DoorRoomDoor : Interaction
{
  public int FollowerCount = 1;
  public FollowerLocation Location;
  public string SceneName;
  public BoxCollider2D CollideForDoor;
  public GameObject PlayerPosition;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject DoorToMove;
  public Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4f);
  public bool Used;
  public bool Unlocked;
  public SimpleBark Bark;
  public string sOpenDoor;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Used = false;
    this.Unlocked = DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location);
    this.DoorToMove.transform.localPosition = !this.Unlocked ? Vector3.zero : this.OpenDoorPosition;
    this.OpenDoor();
  }

  public void OpenDoor()
  {
    if (this.Unlocked)
      this.CollideForDoor.enabled = true;
    else
      this.CollideForDoor.enabled = false;
  }

  public void Start()
  {
    this.Bark = this.GetComponentInChildren<SimpleBark>();
    this.Bark.Translate = false;
    int num1 = 0;
    int num2 = -1;
    while (++num2 <= 4)
    {
      Debug.Log((object) num2);
      if (DataManager.HasKeyPieceFromLocation(this.Location, num2))
        ++num1;
    }
    this.Bark.Entries[0].TermToSpeak = $"{LocalizationManager.Sources[0].GetTranslation(this.Bark.Entries[0].TermToSpeak)} - {num1.ToString()}/4x <sprite name=\"icon_key\">";
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.Unlocked ? "" : this.sOpenDoor;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    this.SimpleSetCamera.Play();
    this.playerFarming.GoToAndStop(this.PlayerPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.EnterTemple())));
  }

  public IEnumerator EnterTemple()
  {
    Inteaction_DoorRoomDoor inteactionDoorRoomDoor = this;
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(inteactionDoorRoomDoor.Location))
    {
      Debug.Log((object) ("ADD ME! " + inteactionDoorRoomDoor.Location.ToString()));
      DataManager.Instance.UnlockedDungeonDoor.Add(inteactionDoorRoomDoor.Location);
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_unlock", inteactionDoorRoomDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", inteactionDoorRoomDoor.gameObject);
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    Vector3 StartingPosition = inteactionDoorRoomDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      inteactionDoorRoomDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + inteactionDoorRoomDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_done", inteactionDoorRoomDoor.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    inteactionDoorRoomDoor.SimpleSetCamera.Reset();
    inteactionDoorRoomDoor.Unlocked = true;
    inteactionDoorRoomDoor.OpenDoor();
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    Inteaction_DoorRoomDoor.GetFloor(this.Location, false);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.SceneName, 1f, "", new System.Action(this.FadeSave));
  }

  public static void GetFloor(FollowerLocation Location, bool InDungeon)
  {
    DataManager.LocationAndLayer locationAndLayer = DataManager.LocationAndLayer.ContainsLocation(Location, DataManager.Instance.CachePreviousRun);
    int num1 = 4;
    int num2 = DataManager.Instance.GetDungeonLayer(Location);
    bool flag = num2 >= num1 || DataManager.Instance.DungeonCompleted(Location);
    if (GameManager.Layer2)
      num2 = DataManager.GetGodTearNotches(Location) + 1;
    DataManager.Instance.DungeonBossFight = num2 >= num1 && !DataManager.Instance.DungeonCompleted(Location, GameManager.Layer2);
    if (flag)
    {
      num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      if (locationAndLayer != null)
      {
        while (num2 == locationAndLayer.Layer)
          num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      }
    }
    GameManager.DungeonUseAllLayers = flag;
    GameManager.NextDungeonLayer(num2);
    GameManager.NewRun("", InDungeon, Location);
    if (locationAndLayer != null)
    {
      locationAndLayer.Layer = num2;
      Debug.Log((object) ("Now set cached layer to: " + locationAndLayer.Layer.ToString()));
    }
    else
      DataManager.Instance.CachePreviousRun.Add(new DataManager.LocationAndLayer(Location, num2));
  }

  public void FadeSave()
  {
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__17_0()
  {
    this.StartCoroutine((IEnumerator) this.EnterTemple());
  }
}
