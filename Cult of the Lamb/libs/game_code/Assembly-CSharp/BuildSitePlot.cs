// Decompiled with JetBrains decompiler
// Type: BuildSitePlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BuildSitePlot : BaseMonoBehaviour
{
  public static List<BuildSitePlot> BuildSitePlots = new List<BuildSitePlot>();
  public GameObject SmokePrefab;
  public GameObject SmokeObject;
  public Interaction Interaction;
  public GameObject PlacementSquare;
  public Structure Structure;
  public Structures_BuildSite _StructureInfo;
  public Vector2Int Bounds = new Vector2Int(1, 1);
  public Transform RotatedObject;
  public List<Worshipper> Worshippers = new List<Worshipper>();
  public UIProgressIndicator _uiProgressIndicator;
  public Vector3 CentrePosition;
  public Coroutine cBuildSmokeRoutine;
  public ParticleSystem[] particleSystems;

  public static bool StructureOfTypeUnderConstruction(global::StructureBrain.TYPES Type)
  {
    foreach (BuildSitePlot buildSitePlot in BuildSitePlot.BuildSitePlots)
    {
      if (buildSitePlot.StructureBrain.Data.ToBuildType == Type)
        return true;
    }
    return false;
  }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public virtual Structures_BuildSite StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_BuildSite;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public void OnEnable()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    BuildSitePlot.BuildSitePlots.Add(this);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnDisable()
  {
    if ((bool) (UnityEngine.Object) this.Structure)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
      this.StructureBrain.OnBuildProgressChanged -= new System.Action(this.OnBuildProgressChanged);
    BuildSitePlot.BuildSitePlots.Remove(this);
  }

  public void Start()
  {
  }

  public void OnBrainAssigned()
  {
    if (StructuresData.BuildDurationGameMinutes(this.StructureBrain.Data.ToBuildType) <= 0)
    {
      this.Bounds = this.StructureBrain.Data.Bounds;
      this.SpawnTiles();
      this.StartCoroutine((IEnumerator) this.AutoBuild());
    }
    else
    {
      this.StructureBrain.OnBuildProgressChanged += new System.Action(this.OnBuildProgressChanged);
      this.StartCoroutine((IEnumerator) this.CheckTutorialRoutine());
      this.Bounds = this.StructureBrain.Data.Bounds;
      this.SpawnTiles();
    }
  }

  public IEnumerator AutoBuild()
  {
    while ((double) Time.timeScale <= 0.0)
      yield return (object) null;
    ++this.StructureBrain.BuildProgress;
  }

  public IEnumerator CheckTutorialRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    switch (this.StructureInfo.ToBuildType)
    {
      case global::StructureBrain.TYPES.REFINERY:
        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.RefiningResources))
          break;
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.RefiningResources);
        break;
      case global::StructureBrain.TYPES.FURNACE_1:
        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Furnace))
          break;
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Furnace, callback: (System.Action) (() =>
        {
          if (DataManager.Instance.ShowCultWarmth)
            return;
          WarmthBar.Instance.Reveal();
        }));
        break;
      case global::StructureBrain.TYPES.SACRIFICE_TABLE:
        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.SacrificeTable))
          break;
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.SacrificeTable);
        break;
    }
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
    if (LetterBox.IsPlaying || this.StructureBrain == null || this.StructureBrain.Data == null)
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
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.gameObject.activeInHierarchy)
      return;
    this.cBuildSmokeRoutine = this.StartCoroutine((IEnumerator) this.BuildSmokeRoutine());
  }

  public IEnumerator BuildSmokeRoutine()
  {
    BuildSitePlot buildSitePlot = this;
    if ((UnityEngine.Object) buildSitePlot.SmokeObject == (UnityEngine.Object) null)
    {
      buildSitePlot.SmokeObject = UnityEngine.Object.Instantiate<GameObject>(buildSitePlot.SmokePrefab, Vector3.zero, Quaternion.Euler(-180f, 0.0f, 0.0f), buildSitePlot.transform);
      buildSitePlot.SmokeObject.transform.localScale = Vector3.one * (float) Mathf.Max(buildSitePlot.Bounds.x, buildSitePlot.Bounds.y);
      buildSitePlot.SmokeObject.transform.localPosition = buildSitePlot.CentrePosition;
      buildSitePlot.particleSystems = buildSitePlot.SmokeObject.GetComponentsInChildren<ParticleSystem>();
      foreach (ParticleSystem particleSystem in buildSitePlot.particleSystems)
        particleSystem.emission.rateOverTimeMultiplier *= (float) Mathf.Max(buildSitePlot.Bounds.x, buildSitePlot.Bounds.y);
    }
    else
    {
      foreach (ParticleSystem particleSystem in buildSitePlot.particleSystems)
        particleSystem.Play();
    }
    yield return (object) new WaitForSeconds(0.5f);
    foreach (ParticleSystem particleSystem in buildSitePlot.particleSystems)
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
    this.CentrePosition = new Vector3(0.0f, (float) this.Bounds.y / 2f);
    this.Interaction.ActivatorOffset = this.CentrePosition;
    this.Interaction.ActivateDistance = (float) this.Bounds.x;
  }

  public void OnDrawGizmos()
  {
    if (this.StructureInfo == null)
      return;
    Utils.DrawCircleXY(this.StructureInfo.Position + new Vector3(0.0f, (float) this.StructureInfo.Bounds.y / 2f), (float) this.StructureInfo.Bounds.x * 0.5f, Color.green);
    Utils.DrawCircleXY(this.StructureInfo.Position + new Vector3(0.0f, (float) this.StructureInfo.Bounds.y / 2f), 0.5f, Color.blue);
    Utils.DrawCircleXY(this.StructureInfo.Position, 0.5f, Color.yellow);
    Utils.DrawCircleXY(this.transform.position, 0.4f, Color.red);
  }

  [CompilerGenerated]
  public void \u003CUpdateBar\u003Eb__25_0()
  {
    this._uiProgressIndicator = (UIProgressIndicator) null;
  }
}
