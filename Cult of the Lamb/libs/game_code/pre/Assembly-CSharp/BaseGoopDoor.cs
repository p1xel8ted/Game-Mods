// Decompiled with JetBrains decompiler
// Type: BaseGoopDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class BaseGoopDoor : Interaction
{
  public static BaseGoopDoor Instance;
  public bool IsOpen = true;
  public GameObject BlockingCollider;
  public Animator Animator;
  private string doorLockedLabel = "";
  private string sIndoctrinateFollower;
  public SimpleSetCamera SimpleSetCamera;

  public bool LockDoor { get; set; }

  private void Awake() => this.BlockingCollider.SetActive(false);

  private void Start()
  {
    this.Interactable = false;
    this.UpdateLocalisation();
    this.OnNewRecruit();
    if (!DataManager.Instance.BaseGoopDoorLocked)
      return;
    this.doorLockedLabel = DataManager.Instance.BaseGoopDoorLoc;
    this.BlockGoopDoor();
  }

  public void BlockGoopDoor(string loc = "")
  {
    Debug.Log((object) "BlockGoopDoor()".Colour(Color.red));
    this.DoorUp();
    this.LockDoor = true;
    DataManager.Instance.BaseGoopDoorLocked = true;
    if (string.IsNullOrEmpty(loc))
      return;
    DataManager.Instance.BaseGoopDoorLoc = loc;
    this.doorLockedLabel = loc;
  }

  public void UnblockGoopDoor()
  {
    Debug.Log((object) "BlockGoopDoor()".Colour(Color.green));
    this.LockDoor = false;
    this.DoorDown();
    DataManager.Instance.BaseGoopDoorLocked = false;
  }

  public override void OnEnableInteraction()
  {
    BaseGoopDoor.Instance = this;
    base.OnEnableInteraction();
    FollowerRecruit.OnNewRecruit += new System.Action(this.OnNewRecruit);
    if (!(bool) (UnityEngine.Object) BiomeBaseManager.Instance)
      return;
    BiomeBaseManager.Instance.OnNewRecruitRevealed += new System.Action(this.OnNewRecruit);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    FollowerRecruit.OnNewRecruit -= new System.Action(this.OnNewRecruit);
    if (!(bool) (UnityEngine.Object) BiomeBaseManager.Instance)
      return;
    BiomeBaseManager.Instance.OnNewRecruitRevealed -= new System.Action(this.OnNewRecruit);
  }

  private void OnNewRecruit()
  {
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sIndoctrinateFollower = ScriptLocalization.Interactions.IndoctrinateBeforeLeaving;
  }

  public override void GetLabel()
  {
    if (this.LockDoor)
    {
      if (DataManager.Instance.InTutorial && ObjectiveManager.AllObjectiveGroupIDs().Count == 1)
      {
        foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(ObjectiveManager.AllObjectiveGroupIDs()[0]))
        {
          if (DataManager.Instance.Objectives.Contains(objectivesData))
          {
            this.Label = objectivesData.Text;
            break;
          }
        }
      }
      else
        this.Label = string.IsNullOrEmpty(this.doorLockedLabel) ? "" : LocalizationManager.GetTranslation(this.doorLockedLabel);
    }
    else
      this.Label = !this.IsOpen ? this.sIndoctrinateFollower : "";
  }

  public override void IndicateHighlighted()
  {
  }

  public override void EndIndicateHighlighted()
  {
  }

  public void DoorUp(string label = "")
  {
    if (this.LockDoor)
      return;
    if (this.IsOpen)
      this.Animator.Play("GoopWallIntro");
    this.IsOpen = false;
    this.BlockingCollider.SetActive(true);
    this.doorLockedLabel = label;
  }

  public void DoorDown()
  {
    Debug.Log((object) ("LockDoor: " + this.LockDoor.ToString()));
    if (this.LockDoor)
      return;
    if (!this.IsOpen)
      this.Animator.Play("GoopWallDown");
    this.IsOpen = true;
    this.BlockingCollider.SetActive(false);
    this.doorLockedLabel = "";
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || (UnityEngine.Object) collision.gameObject != (UnityEngine.Object) PlayerFarming.Instance.gameObject || this.IsOpen)
      return;
    this.Animator.Play("GoopWallColliding");
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", this.gameObject);
  }

  public void PlayOpenDoorSequence(System.Action Callback)
  {
    this.StartCoroutine((IEnumerator) this.PlayOpenDoorSequenceRoutine(Callback));
  }

  private IEnumerator PlayOpenDoorSequenceRoutine(System.Action Callback)
  {
    BaseGoopDoor baseGoopDoor = this;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    baseGoopDoor.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(2.5f);
    baseGoopDoor.UnblockGoopDoor();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", baseGoopDoor.gameObject);
    yield return (object) new WaitForSeconds(2f);
    baseGoopDoor.SimpleSetCamera.Reset();
    System.Action action = Callback;
    if (action != null)
      action();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }
}
