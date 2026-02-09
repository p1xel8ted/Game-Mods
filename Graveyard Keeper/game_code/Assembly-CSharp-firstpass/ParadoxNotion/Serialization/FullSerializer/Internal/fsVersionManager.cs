// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsVersionManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public static class fsVersionManager
{
  public static Dictionary<Type, fsOption<fsVersionedType>> _cache = new Dictionary<Type, fsOption<fsVersionedType>>();

  public static fsResult GetVersionImportPath(
    string currentVersion,
    fsVersionedType targetVersion,
    out List<fsVersionedType> path)
  {
    path = new List<fsVersionedType>();
    if (!fsVersionManager.GetVersionImportPathRecursive(path, currentVersion, targetVersion))
      return fsResult.Fail($"There is no migration path from \"{currentVersion}\" to \"{targetVersion.VersionString}\"");
    path.Add(targetVersion);
    return fsResult.Success;
  }

  public static bool GetVersionImportPathRecursive(
    List<fsVersionedType> path,
    string currentVersion,
    fsVersionedType current)
  {
    for (int index = 0; index < current.Ancestors.Length; ++index)
    {
      fsVersionedType ancestor = current.Ancestors[index];
      if (ancestor.VersionString == currentVersion || fsVersionManager.GetVersionImportPathRecursive(path, currentVersion, ancestor))
      {
        path.Add(ancestor);
        return true;
      }
    }
    return false;
  }

  public static fsOption<fsVersionedType> GetVersionedType(Type type)
  {
    fsOption<fsVersionedType> versionedType1;
    if (!fsVersionManager._cache.TryGetValue(type, out versionedType1))
    {
      fsObjectAttribute attribute = fsPortableReflection.GetAttribute<fsObjectAttribute>((MemberInfo) type);
      if (attribute != null && (!string.IsNullOrEmpty(attribute.VersionString) || attribute.PreviousModels != null))
      {
        if (attribute.PreviousModels != null && string.IsNullOrEmpty(attribute.VersionString))
          throw new Exception($"fsObject attribute on {type?.ToString()} contains a PreviousModels specifier - it must also include a VersionString modifier");
        fsVersionedType[] fsVersionedTypeArray = new fsVersionedType[attribute.PreviousModels != null ? attribute.PreviousModels.Length : 0];
        for (int index = 0; index < fsVersionedTypeArray.Length; ++index)
        {
          fsOption<fsVersionedType> versionedType2 = fsVersionManager.GetVersionedType(attribute.PreviousModels[index]);
          fsVersionedTypeArray[index] = !versionedType2.IsEmpty ? versionedType2.Value : throw new Exception($"Unable to create versioned type for ancestor {versionedType2.ToString()}; please add an [fsObject(VersionString=\"...\")] attribute");
        }
        fsVersionedType type1 = new fsVersionedType()
        {
          Ancestors = fsVersionedTypeArray,
          VersionString = attribute.VersionString,
          ModelType = type
        };
        fsVersionManager.VerifyUniqueVersionStrings(type1);
        fsVersionManager.VerifyConstructors(type1);
        versionedType1 = fsOption.Just<fsVersionedType>(type1);
      }
      fsVersionManager._cache[type] = versionedType1;
    }
    return versionedType1;
  }

  public static void VerifyConstructors(fsVersionedType type)
  {
    ConstructorInfo[] declaredConstructors = type.ModelType.GetDeclaredConstructors();
    for (int index1 = 0; index1 < type.Ancestors.Length; ++index1)
    {
      Type modelType = type.Ancestors[index1].ModelType;
      bool flag = false;
      for (int index2 = 0; index2 < declaredConstructors.Length; ++index2)
      {
        ParameterInfo[] parameters = declaredConstructors[index2].GetParameters();
        if (parameters.Length == 1 && Type.op_Equality(parameters[0].ParameterType, modelType))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        throw new fsMissingVersionConstructorException(type.ModelType, modelType);
    }
  }

  public static void VerifyUniqueVersionStrings(fsVersionedType type)
  {
    Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
    Queue<fsVersionedType> fsVersionedTypeQueue = new Queue<fsVersionedType>();
    fsVersionedTypeQueue.Enqueue(type);
    while (fsVersionedTypeQueue.Count > 0)
    {
      fsVersionedType fsVersionedType = fsVersionedTypeQueue.Dequeue();
      if (dictionary.ContainsKey(fsVersionedType.VersionString) && Type.op_Inequality(dictionary[fsVersionedType.VersionString], fsVersionedType.ModelType))
        throw new fsDuplicateVersionNameException(dictionary[fsVersionedType.VersionString], fsVersionedType.ModelType, fsVersionedType.VersionString);
      dictionary[fsVersionedType.VersionString] = fsVersionedType.ModelType;
      foreach (fsVersionedType ancestor in fsVersionedType.Ancestors)
        fsVersionedTypeQueue.Enqueue(ancestor);
    }
  }
}
