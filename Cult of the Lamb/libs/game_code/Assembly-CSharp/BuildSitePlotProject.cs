// Decompiled with JetBrains decompiler
// Type: BuildSitePlotProject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BuildSitePlotProject : BaseMonoBehaviour
{
  public static List<BuildSitePlotProject> BuildSitePlots = new List<BuildSitePlotProject>();
  public GameObject SmokePrefab;
  public GameObject SmokeObject;
  public Interaction Interaction;
  public GameObject PlacementSquare;
  public Structure Structure;
  public Structures_BuildSiteProject _StructureInfo;
  public Vector2Int Bounds = new Vector2Int(1, 1);
  public Transform RotatedObject;
  public List<Worshipper> Worshippers = new List<Worshipper>();
  public UIProgressIndicator _uiProgressIndicator;
  public Vector3 CentrePosition;
  public Coroutine cBuildSmokeRoutine;
  public ParticleSystem[] particleSystems;

  public static bool StructureOfTypeUnderConstruction(global::StructureBrain.TYPES Type)
  {
    foreach (BuildSitePlotProject buildSitePlot in BuildSitePlotProject.BuildSitePlots)
    {
      if (buildSitePlot.StructureBrain.Data.ToBuildType == Type)
        return true;
    }
    return false;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public virtual Structures_BuildSiteProject StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_BuildSiteProject;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void OnEnable()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    BuildSitePlotProject.BuildSitePlots.Add(this);
  }

  public void OnDisable()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.OnBuildProgressChanged -= new System.Action(this.OnBuildProgressChanged);
    BuildSitePlotProject.BuildSitePlots.Remove(this);
  }

  public void Start()
  {
    this.StructureBrain.OnBuildProgressChanged += new System.Action(this.OnBuildProgressChanged);
    this.OnBuildProgressChanged();
    this.Bounds = this.StructureBrain.Data.Bounds;
    this.SpawnTiles();
  }

  public void OnBrainAssigned()
  {
    this.StructureBrain.OnBuildProgressChanged += new System.Action(this.OnBuildProgressChanged);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnBuildProgressChanged -= new System.Action(this.OnBuildProgressChanged);
  }

  public void UpdateBar()
  {
    if (LetterBox.IsPlaying)
      return;
    float progress = this.StructureBrain.BuildProgress / (float) StructuresData.BuildDurationGameMinutes(this.StructureBrain.Data.ToBuildType);
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
      this._uiProgressIndicator.SetProgress(progress);
  }

  public void OnBuildProgressChanged()
  {
    this.UpdateBar();
    if (this.cBuildSmokeRoutine != null)
      this.StopCoroutine(this.cBuildSmokeRoutine);
    if (!this.gameObject.activeSelf)
      return;
    this.cBuildSmokeRoutine = this.StartCoroutine((IEnumerator) this.BuildSmokeRoutine());
  }

  public IEnumerator BuildSmokeRoutine()
  {
    BuildSitePlotProject buildSitePlotProject = this;
    if ((UnityEngine.Object) buildSitePlotProject.SmokeObject == (UnityEngine.Object) null)
    {
      buildSitePlotProject.SmokeObject = UnityEngine.Object.Instantiate<GameObject>(buildSitePlotProject.SmokePrefab, buildSitePlotProject.transform.position + new Vector3(0.0f, (float) buildSitePlotProject.Bounds.y / 2f, 0.0f), Quaternion.Euler(-180f, 0.0f, 0.0f), buildSitePlotProject.transform);
      buildSitePlotProject.SmokeObject.transform.localScale = Vector3.one * (float) Mathf.Max(buildSitePlotProject.Bounds.x, buildSitePlotProject.Bounds.y);
      buildSitePlotProject.particleSystems = buildSitePlotProject.SmokeObject.GetComponentsInChildren<ParticleSystem>();
      foreach (ParticleSystem particleSystem in buildSitePlotProject.particleSystems)
        particleSystem.emission.rateOverTimeMultiplier *= (float) Mathf.Max(buildSitePlotProject.Bounds.x, buildSitePlotProject.Bounds.y);
    }
    else
    {
      foreach (ParticleSystem particleSystem in buildSitePlotProject.particleSystems)
        particleSystem.Play();
    }
    yield return (object) new WaitForSeconds(0.5f);
    foreach (ParticleSystem particleSystem in buildSitePlotProject.particleSystems)
      particleSystem.Stop();
  }

  public void SpawnTiles()
  {
    int x = -1;
    while (++x < this.Bounds.x)
    {
      int y = -1;
      while (++y < this.Bounds.y)
        UnityEngine.Object.Instantiate<GameObject>(this.PlacementSquare, this.RotatedObject.transform, false).transform.localPosition = new Vector3((float) x, (float) y);
    }
    this.CentrePosition = this.transform.position + new Vector3(0.0f, (float) this.Bounds.y / 2f);
    this.CentrePosition = this.transform.position + new Vector3(0.0f, (float) this.Bounds.y / 2f);
    this.Interaction.ActivatorOffset = this.CentrePosition - this.transform.position;
    this.Interaction.ActivateDistance = (float) this.Bounds.x;
  }

  [CompilerGenerated]
  public void \u003CUpdateBar\u003Eb__23_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }
}
