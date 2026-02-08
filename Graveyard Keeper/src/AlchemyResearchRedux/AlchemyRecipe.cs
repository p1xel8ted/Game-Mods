namespace AlchemyResearchRedux
{
    public class AlchemyRecipe
    {

        private static List<AlchemyRecipe> KnownRecipes = [];

        public string CraftString { get; set; } = string.Empty;
        public string Ingredient1 { get; set; } = string.Empty;
        public string Ingredient2 { get; set; } = string.Empty;
        public string Ingredient3 { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        
        internal static AlchemyRecipe GetRecipeResult(string craftString)
        {
            var found = KnownRecipes.Find(recipe => recipe.CraftString == craftString);
            return found ?? new AlchemyRecipe();
        }

        public static void AddRecipe(AlchemyRecipe recipe)
        {
            var exists = KnownRecipes.Find(r => r.CraftString == recipe.CraftString);
            if (exists == null)
            {
                KnownRecipes.Add(recipe); 
                Plugin.LOG.LogInfo($"Added recipe: [{recipe.Ingredient1}, {recipe.Ingredient2}, {recipe.Ingredient3}] -> {recipe.Result}");
            }
            SaveRecipesToFile();
        }

        private static string SavePath => Path.Combine(Application.persistentDataPath, "AlchemyRecipes.json");


        // Serialization Methods
        public static void SaveRecipesToFile()
        {
            var json = JsonConvert.SerializeObject(KnownRecipes, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }


        public static void LoadRecipesFromFile()
        {
            if (!File.Exists(SavePath))
            {
                KnownRecipes = []; // Return an empty list if the file doesn't exist
                return;
            }

            var json = File.ReadAllText(SavePath);
            KnownRecipes = JsonConvert.DeserializeObject<List<AlchemyRecipe>>(json);
        }
    }
}