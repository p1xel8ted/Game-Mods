// Decompiled with JetBrains decompiler
// Type: MA_Bootstrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class MA_Bootstrapper : MonoBehaviour
{
  public void Awake()
  {
  }

  public void OnGUI()
  {
    GUI.Label(new Rect(20f, 40f, 640f, 190f), "This is the Bootstrapper Scene. Set it up in BuildSettings as the first Scene. Then add '_AfterBootstrapperScene' as the second Scene. Hit play. Master Audio is configured in 'persist between Scenes' mode. Finally, click 'Load Game Scene' button and notice how the music doesn't get interruped even though we're changing Scenes. Normally a Bootstrapper Scene would not be seen. We are illustrating how to set up though. Notice that no Sound Groups are set up in Master Audio. Sample music provided by Alchemy Studios. This music 'The Epic Trailer' (longer version) is available on the Asset Store!");
    if (!GUI.Button(new Rect(100f, 150f, 150f, 100f), "Load Game Scene"))
      return;
    SceneManager.LoadScene(1);
  }
}
