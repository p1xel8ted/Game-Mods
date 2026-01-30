// Decompiled with JetBrains decompiler
// Type: EnableComponentPerScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class EnableComponentPerScene : MonoBehaviour
{
  [Tooltip("The scene in which components should be enabled.")]
  public string targetSceneName;
  [Tooltip("Components to enable in the target scene and disable in all others.")]
  public Behaviour[] components;
  public bool applyOnStart = true;

  public void Start()
  {
    if (this.applyOnStart)
      this.Apply(SceneManager.GetActiveScene().name);
    SceneManager.activeSceneChanged += (UnityAction<Scene, Scene>) ((oldScene, newScene) => this.Apply(newScene.name));
  }

  public void Apply(string sceneName)
  {
    this.SetEnabled(this.components, sceneName == this.targetSceneName);
  }

  public void SetEnabled(Behaviour[] comps, bool state)
  {
    if (comps == null)
      return;
    foreach (Behaviour comp in comps)
    {
      if ((bool) (Object) comp)
        comp.enabled = state;
    }
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__3_0(Scene oldScene, Scene newScene) => this.Apply(newScene.name);
}
