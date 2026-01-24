namespace Rebirth
{
    internal class RebirthSubCommand : CustomFollowerCommand
    {
        private static int ItemQty => Plugin.TokenCost.Value;
        public override string InternalName => "REBIRTH_SUB_COMMAND";

        public override Sprite CommandIcon { get; } = TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));

        public override string GetTitle(Follower follower)
        {
            return $"Rebirth for {ItemQty} tokens.";
        }

        public RebirthSubCommand()
        {
            SubCommands = FollowerCommandGroups.AreYouSureCommands();
        }

        public override string GetDescription(Follower follower)
        {
            return "Perform a Rebirth using special tokens obtained while on crusades.";
        }

        public override string GetLockedDescription(Follower follower)
        {
            if (DataManager.Instance.Followers_Recruit.Count > 0)
            {
                return "You already have a follower awaiting indoctrination!";
            }
            
            if (Helper.IsOld(follower))
            {
                return "Not enough life essence left to satisfy those below.";
            }
            return "Requires 25 Rebirth tokens to perform.";
        }

        public override bool ShouldAppearFor(Follower follower)
        {
            return SaveData.FollowerBornAgain(follower.Brain._directInfoAccess);
        }

        public override bool IsAvailable(Follower follower)
        {
            if (DataManager.Instance.Followers_Recruit.Count > 0)
            {
                return false;
            }
            
            if (Helper.IsOld(follower))
            {
                return false;
            }

            return Inventory.GetItemQuantity((int) Plugin.RebirthItem) >= ItemQty;
        }


        // ReSharper disable once OptionalParameterHierarchyMismatch
        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            if (finalCommand != FollowerCommands.AreYouSureYes) return;
            interaction.Close(true, reshowMenu: false);
            RebirthFollowerCommand.SpawnRecruit(interaction.follower);
            Inventory.ChangeItemQuantity(Plugin.RebirthItem, -ItemQty);
        }
    }
}