// Decompiled with JetBrains decompiler
// Type: BloodStoneStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BloodStoneStructure : Interaction
{
  public float Health = 3f;
  public float TotalHealth = 3f;
  public DropLootOnDeath DropLootOnDeath;
  public Transform[] tToShake;
  public List<Vector2> tShakes;
  public GameObject BuildSiteProgressUIPrefab;
  public BuildSitePlotProgressUI ProgressUI;
  public static System.Action<BloodStoneStructure> PlayerActivatingStart;
  public static System.Action<BloodStoneStructure> PlayerActivatingEnd;
  public static List<BloodStoneStructure> BloodStones = new List<BloodStoneStructure>();
  public Structure Structure;
  public Structures_Rubble _StructureInfo;
  public string sString;
  public bool Activating;
  public float ShowTimer;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Rubble StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Rubble;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.tShakes = new List<Vector2>();
    foreach (Transform transform in this.tToShake)
      this.tShakes.Add(Vector2.zero);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    this.ProgressUI = UnityEngine.Object.Instantiate<GameObject>(CanvasConstants.instance.BuildSiteProgressUIPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<BuildSitePlotProgressUI>();
    this.ProgressUI.gameObject.SetActive(false);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnRemovalProgressChanged += new System.Action<int>(this.OnRemovalProgressChanged);
    this.OnRemovalProgressChanged(-1);
  }

  public override void OnEnableInteraction()
  {
    BloodStoneStructure.BloodStones.Add(this);
    base.OnEnableInteraction();
  }

  public override void OnDisableInteraction()
  {
    BloodStoneStructure.BloodStones.Remove(this);
    base.OnDisableInteraction();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.MineBloodStone;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoBuild());
  }

  public new void OnDestroy()
  {
    System.Action<BloodStoneStructure> playerActivatingEnd = BloodStoneStructure.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(this);
    if (this.StructureInfo.Destroyed)
      this.DropLootOnDeath.Play(this.gameObject);
    this.StructureBrain.OnRemovalProgressChanged -= new System.Action<int>(this.OnRemovalProgressChanged);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if ((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.ProgressUI.gameObject);
    if (!this.Activating)
      return;
    this.StopAllCoroutines();
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void ShowUI()
  {
    if (!this.ProgressUI.gameObject.activeSelf)
      this.ProgressUI.Show();
    this.ShowTimer = 3f;
  }

  public override void Update()
  {
    base.Update();
    int index1 = -1;
    while (++index1 < this.tShakes.Count)
    {
      Vector2 tShake = this.tShakes[index1];
      tShake.y += (float) ((0.0 - (double) tShake.x) * 0.30000001192092896);
      tShake.x += (tShake.y *= 0.7f);
      this.tShakes[index1] = tShake;
    }
    int index2 = -1;
    while (++index2 < this.tToShake.Length)
    {
      if ((UnityEngine.Object) this.tToShake[index2] != (UnityEngine.Object) null)
        this.tToShake[index2].localPosition = new Vector3(this.tShakes[index2].x, this.tToShake[index2].localPosition.y, this.tToShake[index2].localPosition.z);
    }
    if ((double) this.ShowTimer <= 0.0)
      return;
    this.ShowTimer -= Time.deltaTime;
    if ((double) this.ShowTimer > 0.0)
      return;
    this.ProgressUI.Hide();
  }

  public void LateUpdate() => this.ProgressUI.SetPosition(this.transform.position);

  public IEnumerator DoBuild()
  {
    BloodStoneStructure bloodStoneStructure = this;
    System.Action<BloodStoneStructure> playerActivatingStart = BloodStoneStructure.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(bloodStoneStructure);
    bloodStoneStructure.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    bloodStoneStructure.state.facingAngle = Utils.GetAngle(bloodStoneStructure.state.transform.position, bloodStoneStructure.transform.position);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/chop-stone", 0, true);
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(bloodStoneStructure.SimpleSpineAnimator_OnSpineEvent);
    bloodStoneStructure.state.CURRENT_STATE = StateMachine.State.Idle;
    bloodStoneStructure.Activating = false;
    System.Action<BloodStoneStructure> playerActivatingEnd = BloodStoneStructure.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(bloodStoneStructure);
  }

  public void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    if (!(EventName == "Chop"))
      return;
    CameraManager.shakeCamera(0.5f);
    int index = -1;
    while (++index < this.tShakes.Count)
      this.tShakes[index] = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 0.0f);
  }

  public void OnRemovalProgressChanged(int followerID)
  {
    if ((double) this.StructureBrain.RemovalProgress == 0.0)
      this.ProgressUI.gameObject.SetActive(false);
    else
      this.ShowUI();
  }
}
