// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.CameraFader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

public class CameraFader : MonoBehaviour
{
  public static CameraFader _current;
  public float alpha;
  public Texture2D _blackTexture;

  public Texture2D blackTexture
  {
    get
    {
      if ((Object) this._blackTexture == (Object) null)
      {
        this._blackTexture = new Texture2D(1, 1);
        this._blackTexture.SetPixel(1, 1, Color.black);
        this._blackTexture.Apply();
      }
      return this._blackTexture;
    }
  }

  public static CameraFader current
  {
    get
    {
      if ((Object) CameraFader._current == (Object) null)
        CameraFader._current = Object.FindObjectOfType<CameraFader>();
      if ((Object) CameraFader._current == (Object) null)
        CameraFader._current = new GameObject("_CameraFader").AddComponent<CameraFader>();
      return CameraFader._current;
    }
  }

  public void FadeIn(float time) => this.StartCoroutine(this.CoroutineFadeIn(time));

  public void FadeOut(float time) => this.StartCoroutine(this.CoroutineFadeOut(time));

  public IEnumerator CoroutineFadeIn(float time)
  {
    for (this.alpha = 1f; (double) this.alpha > 0.0; this.alpha -= 1f / time * Time.deltaTime)
      yield return (object) null;
  }

  public IEnumerator CoroutineFadeOut(float time)
  {
    for (this.alpha = 0.0f; (double) this.alpha < 1.0; this.alpha += 1f / time * Time.deltaTime)
      yield return (object) null;
  }

  public void OnGUI()
  {
    if ((double) this.alpha <= 0.0)
      return;
    GUI.color = new Color(1f, 1f, 1f, this.alpha);
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), (Texture) this.blackTexture);
    GUI.color = Color.white;
  }
}
