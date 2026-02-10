// Decompiled with JetBrains decompiler
// Type: Interaction_BiomeDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_BiomeDoor : Interaction
{
  public int FollowerCount = 1;
  public GameObject PlayerPosition;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject DoorToMove;
  public Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4f);
  public BoxCollider2D CollideForDoor;
  public bool Unlocked;
  public string sOpenDoor;
  public bool Used;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Used = false;
    Debug.Log((object) $"DataManager.Instance.UnlockedBossTempleDoor.Contains(MMBiomeGeneration.BiomeGenerator.Instance.DungeonLocation) {DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation).ToString()}  {BiomeGenerator.Instance.DungeonLocation.ToString()}");
    this.Unlocked = DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation);
    this.DoorToMove.transform.localPosition = !this.Unlocked ? Vector3.zero : this.OpenDoorPosition;
    this.OpenDoor();
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public void OnBiomeChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
    BiomeGenerator.Instance.North.Init(GenerateRoom.ConnectionTypes.False);
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

  public override void GetLabel()
  {
    this.Label = this.Unlocked ? "" : $"{this.sOpenDoor} <sprite name=\"icon_Followers\">x{LocalizeIntegration.ReverseText(this.FollowerCount.ToString())}";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.Followers.Count >= this.FollowerCount || CheatConsole.BuildingsFree)
    {
      GameManager.GetInstance().OnConversationNew();
      this.SimpleSetCamera.Play();
      this.playerFarming.GoToAndStop(this.PlayerPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.EnterTemple())));
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator EnterTemple()
  {
    Interaction_BiomeDoor interactionBiomeDoor = this;
    if (!DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation))
    {
      Debug.Log((object) ("ADD ME! " + BiomeGenerator.Instance.DungeonLocation.ToString()));
      DataManager.Instance.UnlockedBossTempleDoor.Add(BiomeGenerator.Instance.DungeonLocation);
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_unlock", interactionBiomeDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", interactionBiomeDoor.gameObject);
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    Vector3 StartingPosition = interactionBiomeDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionBiomeDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + interactionBiomeDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_done", interactionBiomeDoor.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBiomeDoor.SimpleSetCamera.Reset();
    interactionBiomeDoor.Unlocked = true;
    interactionBiomeDoor.OpenDoor();
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(this.ChangeRoom));
  }

  public void ChangeRoom()
  {
    BiomeGenerator.Instance.FirstArrival = true;
    BiomeGenerator.Instance.DoFirstArrivalRoutine = true;
    BiomeGenerator.ChangeRoom(BiomeGenerator.Instance.RoomEntrance.x, BiomeGenerator.Instance.RoomEntrance.y);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__15_0()
  {
    this.StartCoroutine((IEnumerator) this.EnterTemple());
  }
}
