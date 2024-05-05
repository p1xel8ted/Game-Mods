using System.Collections.Generic;
using System.IO;

namespace AlchemyResearch
{
    public static class ResearchedAlchemyRecipes
    {
        public static readonly Dictionary<string, ResearchedAlchemyRecipe> ResearchedRecipes = new();

        public static void AddCurrentRecipe(string ItemResult)
        {
            var researchedAlchemyRecipe = new ResearchedAlchemyRecipe(AlchemyRecipe.Ingredient1, AlchemyRecipe.Ingredient2, AlchemyRecipe.Ingredient3, ItemResult);
            Logg.Log(
                $"Adding Recipe: {AlchemyRecipe.Ingredient1}|{AlchemyRecipe.Ingredient2}|{AlchemyRecipe.Ingredient3} => {AlchemyRecipe.Result} | WGO: {AlchemyRecipe.WorkstationUnityId} / {AlchemyRecipe.WorkstationObjectId}");
            var key = researchedAlchemyRecipe.GetKey();
            if (!ResearchedRecipes.ContainsKey(key))
                ResearchedRecipes.Add(researchedAlchemyRecipe.GetKey(), researchedAlchemyRecipe);
            AlchemyRecipe.Initialize();
        }

        public static string IsRecipeKnown(string ingredient1 = "empty", string ingredient2 = "empty", string ingredient3 = "empty")
        {
            var key = ResearchedAlchemyRecipe.GetKey(ingredient1, ingredient2, ingredient3);
            return ResearchedRecipes.ContainsKey(key) ? ResearchedRecipes[key].Result : "unknown";
        }

        public static Dictionary<string, Dictionary<string, ResearchedAlchemyRecipe>> ReadRecipesFromFile()
        {
            var dictionary = new Dictionary<string, Dictionary<string, ResearchedAlchemyRecipe>>();
            var key = "1";
            string[] strArray1;
            try
            {
                strArray1 = File.ReadAllLines(MainPatcher.KnownRecipesFilePathAndName);
            }
            catch
            {
                return dictionary;
            }
            if (strArray1.Length == 0)
                return dictionary;
            foreach (var str1 in strArray1)
            {
                var str2 = str1.Trim();
                if (str2.StartsWith(MainPatcher.ParameterComment) || string.IsNullOrEmpty(str2)) continue;
                if (str2.StartsWith(MainPatcher.ParameterSectionBegin) && str2.EndsWith(MainPatcher.ParameterSectionEnd))
                {
                    key = str2.Replace(MainPatcher.ParameterSectionBegin, string.Empty).Replace(MainPatcher.ParameterSectionEnd, string.Empty).Trim();
                    if (!dictionary.ContainsKey(key))
                        dictionary.Add(key, new Dictionary<string, ResearchedAlchemyRecipe>());
                }
                else
                {
                    var strArray2 = str2.Split(MainPatcher.ParameterSeparator);
                    if (strArray2.Length < 4) continue;
                    var researchedAlchemyRecipe = new ResearchedAlchemyRecipe(strArray2[0].Trim(), strArray2[1].Trim(), strArray2[2].Trim(), strArray2[3].Trim());
                    dictionary[key].Add(researchedAlchemyRecipe.GetKey(), researchedAlchemyRecipe);
                }
            }

            using var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
                Logg.Log($"Loaded Recipes for Save game [{enumerator.Current.Key}]: {enumerator.Current.Value.Count}");
            return dictionary;
        }

        public static void WriteRecipesToFile(string SaveGameName)
        {
            var contents = new List<string>();
            var dictionary1 = ReadRecipesFromFile();
            if (!dictionary1.ContainsKey(SaveGameName))
                dictionary1.Add(SaveGameName, new Dictionary<string, ResearchedAlchemyRecipe>());
            using (var enumerator = ResearchedRecipes.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var dictionary2 = dictionary1[SaveGameName];
                    var current = enumerator.Current;
                    var key1 = current.Key;
                    if (dictionary2.ContainsKey(key1)) continue;
                    var dictionary3 = dictionary1[SaveGameName];
                    current = enumerator.Current;
                    var key2 = current.Key;
                    current = enumerator.Current;
                    var researchedAlchemyRecipe = current.Value;
                    dictionary3.Add(key2, researchedAlchemyRecipe);
                }
            }
            using var enumerator1 = dictionary1.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                contents.Add($"[{enumerator1.Current.Key}]");
                using var enumerator2 = enumerator1.Current.Value.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    var current = enumerator2.Current;
                    var ingredient1 = current.Value.Ingredient1;
                    current = enumerator2.Current;
                    var ingredient2 = current.Value.Ingredient2;
                    current = enumerator2.Current;
                    var ingredient3 = current.Value.Ingredient3;
                    current = enumerator2.Current;
                    var result = current.Value.Result;
                    var str = ResearchedAlchemyRecipe.AlchemyRecipeToString(ingredient1, ingredient2, ingredient3, result);
                    contents.Add(str);
                }
            }
            try
            {
                File.WriteAllLines(MainPatcher.KnownRecipesFilePathAndName, contents);
            }
            catch
            {
                // ignored
            }
        }
    }
}