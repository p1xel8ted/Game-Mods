// Decompiled with JetBrains decompiler
// Type: JobAssignMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class JobAssignMenu : BaseMonoBehaviour
{
  public GameObject ImagePrefab;
  private StateMachine PlayerState;
  public RectTransform RightArrow;
  public RectTransform LeftArrow;
  public TextMeshProUGUI Name;
  public TextMeshProUGUI Description;
  private int current_selection;
  private float selectionDelay;
  private List<Image> icons;
  private WorkPlace workPlace;
  private GameObject CurrentWorkerIcon;

  private int CURRENT_SELECTION
  {
    get => this.current_selection;
    set
    {
      this.current_selection = value;
      if (this.current_selection < 0)
        this.current_selection = DataManager.Instance.Followers.Count - 1;
      if (this.current_selection > DataManager.Instance.Followers.Count - 1)
        this.current_selection = 0;
      this.selectionDelay = 0.2f;
    }
  }

  public virtual void Show(string Name, string Description, WorkPlace workPlace)
  {
    this.Name.text = Name;
    this.Description.text = Description;
    this.workPlace = workPlace;
    this.gameObject.SetActive(true);
  }

  private void OnEnable()
  {
    if ((Object) GameObject.FindGameObjectWithTag("Player") != (Object) null)
    {
      this.PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<StateMachine>();
      this.PlayerState.CURRENT_STATE = StateMachine.State.InActive;
    }
    this.AddCurrentWorshipperID();
    this.icons = new List<Image>();
    int index = -1;
    int num1 = -1;
    int num2 = -1;
    while (++index < DataManager.Instance.Followers.Count)
    {
      if (index % 6 == 0)
      {
        ++num1;
        num2 = 0;
      }
      else
        ++num2;
      GameObject gameObject = Object.Instantiate<GameObject>(this.ImagePrefab, Vector3.zero, Quaternion.identity);
      gameObject.transform.parent = this.gameObject.transform;
      gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (220 * num2 - 525), this.LeftArrow.transform.localPosition.y + (float) (num1 * -250));
      gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
      FollowerInfo follower = DataManager.Instance.Followers[index];
      gameObject.GetComponent<WorshipperIcon>().Name.text = follower.Name;
      this.icons.Add(gameObject.GetComponent<Image>());
    }
    this.CURRENT_SELECTION = 0;
  }

  private void AddCurrentWorshipperID()
  {
    WorshipperInfoManager infoManagerByJobId = WorshipperInfoManager.GetWorshipperInfoManagerByJobID(this.workPlace.ID);
    if (!((Object) infoManagerByJobId != (Object) null))
      return;
    this.CurrentWorkerIcon = Object.Instantiate<GameObject>(this.ImagePrefab, Vector3.zero, Quaternion.identity);
    this.CurrentWorkerIcon.transform.parent = this.gameObject.transform;
    this.CurrentWorkerIcon.GetComponent<RectTransform>().localPosition = new Vector3(600f, 300f);
    WorshipperIcon component = this.CurrentWorkerIcon.GetComponent<WorshipperIcon>();
    component.Name.text = infoManagerByJobId.v_i.Name;
    component.Icon.color = new Color(infoManagerByJobId.v_i.color_r, infoManagerByJobId.v_i.color_g, infoManagerByJobId.v_i.color_b);
  }

  private void OnDisable()
  {
    foreach (Component icon in this.icons)
      Object.Destroy((Object) icon.gameObject);
    this.icons.Clear();
    Object.Destroy((Object) this.CurrentWorkerIcon);
    this.CurrentWorkerIcon = (GameObject) null;
    if (!((Object) this.PlayerState != (Object) null))
      return;
    this.PlayerState.CURRENT_STATE = StateMachine.State.Idle;
  }

  private void Update()
  {
    this.selectionDelay -= Time.deltaTime;
    if ((double) InputManager.UI.GetHorizontalAxis() > 0.30000001192092896 && (double) this.selectionDelay < 0.0)
      ++this.CURRENT_SELECTION;
    if ((double) InputManager.UI.GetHorizontalAxis() < -0.30000001192092896 && (double) this.selectionDelay < 0.0)
      --this.CURRENT_SELECTION;
    if ((double) InputManager.UI.GetHorizontalAxis() > -0.30000001192092896 && (double) InputManager.UI.GetHorizontalAxis() < 0.30000001192092896 && (double) InputManager.UI.GetVerticalAxis() > -0.30000001192092896 && (double) InputManager.UI.GetVerticalAxis() < 0.30000001192092896)
      this.selectionDelay = 0.0f;
    for (int index = 0; index < this.icons.Count; ++index)
    {
      if (index == this.CURRENT_SELECTION)
        this.icons[index].transform.localScale = new Vector3(1.5f, 1.5f);
    }
    if (InputManager.UI.GetCancelButtonUp())
      this.gameObject.SetActive(false);
    if (!InputManager.UI.GetAcceptButtonUp())
      return;
    this.gameObject.SetActive(false);
    if (!((Object) this.PlayerState != (Object) null))
      return;
    this.AssignWorshipper();
  }

  public void AssignWorshipper()
  {
    Worshipper.GetWorshipperByJobID(this.workPlace.ID);
    Worshipper.ClearJob(this.workPlace.ID, 0);
  }
}
