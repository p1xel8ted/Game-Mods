// Decompiled with JetBrains decompiler
// Type: NGTools.Errors
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace NGTools;

public class Errors
{
  public const int Server_InternalServerError = 1000;
  public const int Server_Exception = 1001;
  public const int Server_GameObjectNotFound = 3000;
  public const int Server_ComponentNotFound = 3001;
  public const int Server_PathNotResolved = 3002;
  public const int Server_MethodNotFound = 3003;
  public const int Server_InvalidArgument = 3004;
  public const int Server_InvocationFailed = 3008;
  public const int Server_MaterialNotFound = 3005;
  public const int Server_ShaderNotFound = 3006;
  public const int Server_ShaderPropertyNotFound = 3007;
  public const int Scene_Exception = 3500;
  public const int Scene_GameObjectNotFound = 3501;
  public const int Scene_ComponentNotFound = 3502;
  public const int Scene_PathNotResolved = 3503;
  public const int GameConsole_NullDataConsole = 4000;
  public const int CLI_MethodDoesNotReturnString = 4500;
  public const int CLI_RootCommandEmptyAlias = 4501;
  public const int CLI_RootCommandNullBehaviour = 4502;
  public const int CLI_EmptyRootCommand = 4503;
  public const int CLI_ForbiddenCommandOnField = 4504;
  public const int CLI_UnsupportedPropertyType = 4505;
  public const int CLI_ForbiddenCharInName = 4506;
  public const int Camera_RenderTextureFormatNotSupported = 5000;
  public const int Camera_ModuleNotFound = 5001;
  public const int Fav_ResolverNotStatic = 7000;
  public const int Fav_ResolverIsAmbiguous = 7001;
  public const int Fav_ResolverThrownException = 7002;
}
