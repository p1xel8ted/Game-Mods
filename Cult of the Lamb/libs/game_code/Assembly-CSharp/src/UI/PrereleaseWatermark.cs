// Decompiled with JetBrains decompiler
// Type: src.UI.PrereleaseWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src.UI;

public class PrereleaseWatermark : MonoBehaviour
{
  public GUIStyle _cachedGUIStyle;
  public static bool Hidden;
  public string _username;
  public PrereleaseWatermark.Position _position;
  public float _timer;
  public string _message;
  public const float kMaxTime = 10f;

  public GUIStyle CachedGUIStyle
  {
    get
    {
      if (this._cachedGUIStyle == null)
        this._cachedGUIStyle = new GUIStyle(GUI.skin.box)
        {
          fontSize = Screen.currentResolution.height / 65,
          alignment = TextAnchor.MiddleLeft,
          normal = {
            textColor = new Color(1f, 1f, 1f, 0.5f)
          }
        };
      return this._cachedGUIStyle;
    }
  }

  public enum Position
  {
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft,
  }
}
