// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_URLChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(4505)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct HTML_URLChanged_t
{
  public const int k_iCallback = 4505;
  public HHTMLBrowser unBrowserHandle;
  public string pchURL;
  public string pchPostData;
  [MarshalAs(UnmanagedType.I1)]
  public bool bIsRedirect;
  public string pchPageTitle;
  [MarshalAs(UnmanagedType.I1)]
  public bool bNewNavigation;
}
