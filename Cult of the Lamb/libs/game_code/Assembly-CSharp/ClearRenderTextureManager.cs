// Decompiled with JetBrains decompiler
// Type: ClearRenderTextureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class ClearRenderTextureManager : MonoBehaviour
{
  [SerializeField]
  [Tooltip("Assign the same RenderTexture used by the SnowFootstepsController (or other effect scripts).")]
  public RenderTexture _renderTextureToClear;
  [CompilerGenerated]
  public static ClearRenderTextureManager \u003CInstance\u003Ek__BackingField;

  public static ClearRenderTextureManager Instance
  {
    get => ClearRenderTextureManager.\u003CInstance\u003Ek__BackingField;
    set => ClearRenderTextureManager.\u003CInstance\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    Debug.Log((object) $"!!! {this.gameObject.name} Awake() START !!!", (Object) this.gameObject);
    if ((Object) ClearRenderTextureManager.Instance == (Object) null)
    {
      ClearRenderTextureManager.Instance = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
      Debug.Log((object) $"!!! {this.gameObject.name} registered as Singleton and marked DontDestroyOnLoad !!!", (Object) this.gameObject);
    }
    else
    {
      if (!((Object) ClearRenderTextureManager.Instance != (Object) this))
        return;
      Debug.LogWarning((object) $"!!! Duplicate {this.gameObject.name} detected. Destroying this instance. !!!", (Object) this.gameObject);
      Object.Destroy((Object) this.gameObject);
    }
  }

  public void OnEnable()
  {
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
    Debug.Log((object) "ClearRenderTextureManager enabled, subscribed to sceneLoaded.");
  }

  public void OnDisable()
  {
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
    Debug.Log((object) "ClearRenderTextureManager disabled, unsubscribed from sceneLoaded.");
  }

  public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log((object) $"Scene '{scene.name}' loaded (Mode: {mode}). Attempting to clear assigned RenderTexture.");
    ClearRenderTextureManager.ClearRenderTexture(this._renderTextureToClear);
  }

  public void ClearRenderTexture()
  {
    ClearRenderTextureManager.ClearRenderTexture(this._renderTextureToClear);
  }

  public static void ClearRenderTexture(RenderTexture renderTextureToClear)
  {
    if ((Object) renderTextureToClear == (Object) null)
    {
      Debug.LogWarning((object) "RenderTexture to clear is not assigned or missing in the manager.");
    }
    else
    {
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = renderTextureToClear;
      GL.Clear(true, true, Color.clear);
      RenderTexture.active = active;
      Debug.Log((object) $"RenderTexture '{renderTextureToClear.name}' cleared by ClearRenderTextureManager.");
    }
  }
}
