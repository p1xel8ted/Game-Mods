using System.Collections.Generic;

namespace AlchemyResearch
{
    public class ResearchedAlchemyRecipe
    {
        public readonly string Ingredient1;
        public readonly string Ingredient2;
        public readonly string Ingredient3;
        public readonly string Result;

        public ResearchedAlchemyRecipe(
          string ingredient1,
          string ingredient2,
          string ingredient3,
          string result)
        {
            Ingredient1 = ingredient1;
            Ingredient2 = ingredient2;
            Ingredient3 = ingredient3;
            Result = result;
        }

        public string GetKey() => GetKey(Ingredient1, Ingredient2, Ingredient3);

        public static string GetKey(string ingredient1, string ingredient2, string ingredient3)
        {
            var stringList = new List<string>(3)
      {
          ingredient1,
          ingredient2,
          ingredient3
      };
            stringList.Sort();
            return $"{stringList[0]}|{stringList[1]}|{stringList[2]}";
        }

        public static string AlchemyRecipeToString(
          string ingredient1,
          string ingredient2,
          string ingredient3,
          string result)
        {
            return $"{GetKey(ingredient1, ingredient2, ingredient3)}|{result}";
        }
    }
}