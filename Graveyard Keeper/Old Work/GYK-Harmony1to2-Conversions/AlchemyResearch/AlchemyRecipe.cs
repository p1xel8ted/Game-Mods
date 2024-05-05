namespace AlchemyResearch
{
    public static class AlchemyRecipe
    {
        public static int WorkstationUnityId = -1;
        public static string WorkstationObjectId = string.Empty;
        public static string Ingredient1 = "empty";
        public static string Ingredient2 = "empty";
        public static string Ingredient3 = "empty";
        public static string Result = "empty";

        public static void Initialize()
        {
            WorkstationUnityId = -1;
            WorkstationObjectId = string.Empty;
            Ingredient1 = "empty";
            Ingredient2 = "empty";
            Ingredient3 = "empty";
            Result = "empty";
        }

        public static bool HasValidRecipe() => Ingredient1 != "empty" && !string.IsNullOrEmpty(Ingredient1) && Ingredient2 != "empty" && !string.IsNullOrEmpty(Ingredient2) && Result != "empty" && !string.IsNullOrEmpty(Result);

        public static bool MatchesResult(
          int craftingStationUnityId,
          string craftingStationObjectId,
          string resultItemId)
        {
            if (WorkstationUnityId != craftingStationUnityId || WorkstationObjectId != craftingStationObjectId)
                return false;
            if (resultItemId == Result)
                return true;
            return resultItemId.StartsWith("goo_") && Result.StartsWith("goo_");
        }

        public static string AlchemyRecipeToString(
          string ingredient1,
          string ingredient2,
          string ingredient3,
          string result)
        {
            return $"{ResearchedAlchemyRecipe.GetKey(ingredient1, ingredient2, ingredient3)}|{result}";
        }

        public static string GetCurrentRecipe() =>
            $"{Ingredient1}|{Ingredient2}|{Ingredient3}|{Result}|{WorkstationUnityId}|{WorkstationObjectId}";
    }
}