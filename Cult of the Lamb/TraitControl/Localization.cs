namespace TraitControl;

internal static class Localization
{
    // ── Config Descriptions: Trait Replacement ──
    internal static string DescNoNegativeTraits => Get("DescNoNegativeTraits");
    internal static string DescApplyToExisting => Get("DescApplyToExisting");
    internal static string DescUseUnlockedTraits => Get("DescUseUnlockedTraits");
    internal static string DescUseAllTraits => Get("DescUseAllTraits");
    internal static string DescPreferExclusive => Get("DescPreferExclusive");
    internal static string DescPreserveMutated => Get("DescPreserveMutated");
    internal static string DescMinTraits => Get("DescMinTraits");
    internal static string DescMaxTraits => Get("DescMaxTraits");
    internal static string DescRandomizeReindoc => Get("DescRandomizeReindoc");
    internal static string DescTraitReroll => Get("DescTraitReroll");
    internal static string DescProtectTraitCount => Get("DescProtectTraitCount");
    internal static string DescRerollableAltar => Get("DescRerollableAltar");

    // ── Config Descriptions: Unique Traits ──
    internal static string DescAllowMultipleUnique => Get("DescAllowMultipleUnique");
    internal static string UniqueTraitDesc(string traitName, string source) => string.Format(Get("UniqueTraitDesc"), traitName, source);
    internal static string GuaranteeTraitDesc(string traitName) => string.Format(Get("GuaranteeTraitDesc"), traitName);
    internal static string SourceSpecialReward => Get("SourceSpecialReward");
    internal static string SourceCrossover => Get("SourceCrossover");
    internal static string SourceBishopConvert => Get("SourceBishopConvert");

    // ── Config Descriptions: Notifications ──
    internal static string DescShowRemoving => Get("DescShowRemoving");
    internal static string DescShowAdding => Get("DescShowAdding");
    internal static string DescShowReroll => Get("DescShowReroll");

    // ── Config Descriptions: Trait Weights ──
    internal static string DescEnableWeights => Get("DescEnableWeights");
    internal static string DescIncludeEventTraits => Get("DescIncludeEventTraits");
    internal static string DescResetSettings => Get("DescResetSettings");
    internal static string TraitWeightDesc => Get("TraitWeightDesc");

    // ── Config Descriptions: Trait Categories ──
    internal static string CategoryFoundIn => Get("CategoryFoundIn");
    internal static string CategoryGrantedOther => Get("CategoryGrantedOther");

    // ── Config Display Names: Trait Replacement ──
    internal static string NameEnableTraitReplacement => Get("NameEnableTraitReplacement");
    internal static string NameApplyToExisting => Get("NameApplyToExisting");
    internal static string NameUseUnlockedTraits => Get("NameUseUnlockedTraits");
    internal static string NameUseAllTraits => Get("NameUseAllTraits");
    internal static string NamePreferExclusive => Get("NamePreferExclusive");
    internal static string NamePreserveMutated => Get("NamePreserveMutated");
    internal static string NameMinTraits => Get("NameMinTraits");
    internal static string NameMaxTraits => Get("NameMaxTraits");
    internal static string NameRandomizeReindoc => Get("NameRandomizeReindoc");
    internal static string NameTraitReroll => Get("NameTraitReroll");
    internal static string NameProtectTraitCount => Get("NameProtectTraitCount");
    internal static string NameRerollableAltar => Get("NameRerollableAltar");

    // ── Config Display Names: Unique Traits ──
    internal static string NameAllowMultipleUnique => Get("NameAllowMultipleUnique");
    internal static string NameIncludeTrait(string traitName) => string.Format(Get("NameIncludeTrait"), traitName);
    internal static string NameGuaranteeTrait(string traitName) => string.Format(Get("NameGuaranteeTrait"), traitName);

    // ── Config Display Names: Notifications ──
    internal static string NameShowRemoving => Get("NameShowRemoving");
    internal static string NameShowAdding => Get("NameShowAdding");
    internal static string NameShowReroll => Get("NameShowReroll");

    // ── Config Display Names: Trait Weights ──
    internal static string NameEnableWeights => Get("NameEnableWeights");
    internal static string NameIncludeEventTraits => Get("NameIncludeEventTraits");

    // ── Config Display Names: Reset ──
    internal static string NameResetSettings => Get("NameResetSettings");

    // ── IMGUI Text ──
    internal static string WarningModifyAll => Get("WarningModifyAll");
    internal static string WarningNecklaceLoss => Get("WarningNecklaceLoss");
    internal static string ButtonConfirm => Get("ButtonConfirm");
    internal static string ButtonCancel => Get("ButtonCancel");
    internal static string ToggleEnabled => Get("ToggleEnabled");
    internal static string ToggleDisabled => Get("ToggleDisabled");
    internal static string ButtonResetAll => Get("ButtonResetAll");
    internal static string ConfirmResetAll => Get("ConfirmResetAll");
    internal static string ButtonYes => Get("ButtonYes");
    internal static string ButtonNo => Get("ButtonNo");

    // ── Notifications ──
    internal static string NotifyTraitsRerolled(string name, int oldCount, int newCount) => string.Format(Get("NotifyTraitsRerolled"), name, oldCount, newCount);

    private static string Get(string key)
    {
        var lang = I2.Loc.LocalizationManager.CurrentLanguage ?? "English";
        if (Strings.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var value))
        {
            return value;
        }
        // Fallback to English
        return Strings["English"].TryGetValue(key, out var fallback) ? fallback : key;
    }

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        {
            "English", new()
            {
                // Trait Replacement
                { "DescNoNegativeTraits", "Replace negative traits with positive ones. By default only affects NEW followers. Enable 'Apply To Existing Followers' to also modify current followers." },
                { "DescApplyToExisting", "When enabled, trait replacement also applies to existing followers (not just new ones). Disabling will restore original traits." },
                { "DescUseUnlockedTraits", "Only use traits you have unlocked. Applies to both trait replacement and new follower trait selection." },
                { "DescUseAllTraits", "Merge all trait pools into one, bypassing vanilla's normal/rare split. Without this, vanilla assigns traits from separate pools (normal pool by default, ~10% chance of rare pool per trait). Enable this for trait weights to have full control over distribution. Unique traits require their individual toggles." },
                { "DescPreferExclusive", "When replacing negative traits, exclusive traits (like Lazy) are replaced with their positive counterpart (Industrious) instead of a random trait." },
                { "DescPreserveMutated", "When enabled, Rot (Mutated) and Cursed (Zombie) followers will not have their traits removed. These are special state traits — Rot followers are useful for certain rituals, and Cursed followers gain their trait through resurrection." },
                { "DescMinTraits", "Minimum number of traits new followers will have. Vanilla is 2." },
                { "DescMaxTraits", "Maximum number of traits new followers will have. Vanilla is 3. Limited to 8 due to UI constraints." },
                { "DescRandomizeReindoc", "When re-indoctrinating an existing follower (at the altar), randomize their traits using the configured min/max. Vanilla re-indoctrination only changes appearance/name." },
                { "DescTraitReroll", "Adds the Re-educate command to normal followers. Using it will re-roll their traits using the configured min/max and weights." },
                { "DescProtectTraitCount", "When rerolling traits (via reeducation or reindoctrination), ensure the follower doesn't end up with fewer traits than they started with." },
                { "DescRerollableAltar", "When using the Exorcism Altar, re-selecting a follower shows different trait results each time instead of the same result per day." },

                // Unique Traits
                { "DescAllowMultipleUnique", "Allow multiple followers to have the same unique trait (Immortal, Disciple, etc.). Normally only one follower can have each unique trait." },
                { "UniqueTraitDesc", "Allow the {0} trait ({1}) to appear in trait pools." },
                { "GuaranteeTraitDesc", "New followers will always receive the {0} trait (ignores weights). Only one follower can have this trait." },
                { "SourceSpecialReward", "normally a special reward" },
                { "SourceCrossover", "crossover reward" },
                { "SourceBishopConvert", "normally granted when converting a bishop" },

                // Notifications config
                { "DescShowRemoving", "Show notifications when trait replacement removes negative traits." },
                { "DescShowAdding", "Show notifications when trait replacement adds positive traits." },
                { "DescShowReroll", "Show a notification when a follower's traits are rerolled via reeducation or reindoctrination." },

                // Trait Weights
                { "DescEnableWeights", "Enable weighted random selection for new followers. Weights affect trait selection within each pool. For full control over all traits, enable 'Use All Traits Pool' - otherwise vanilla's normal/rare pool split still applies (rare pool has ~10% chance per trait). Set a weight to 0 to disable a trait." },
                { "DescIncludeEventTraits", "Include traits normally granted through gameplay events (marriage, parenting, criminal, missionary, etc.) in the weights list. Only applies when 'Use All Traits Pool' is enabled. Warning: This can result in nonsensical assignments (e.g., ProudParent on followers who have never had children)." },
                { "DescResetSettings", "Click to reset all settings to defaults (vanilla behavior)." },
                { "TraitWeightDesc", "Weight: Higher = more likely relative to other traits. Set to 0 to disable. Default is 1.0. With ~85 traits at weight 1: weight 10 ~ 10%, weight 50 ~ 37%, weight 100 ~ 54%." },

                // Trait Categories
                { "CategoryFoundIn", "Found in: {0}" },
                { "CategoryGrantedOther", "Granted via other means (doctrines, rituals, events, etc.)" },

                // IMGUI
                { "WarningModifyAll", "WARNING: This will modify ALL existing followers!" },
                { "WarningNecklaceLoss", "Traits from necklaces may be lost on restore." },
                { "ButtonConfirm", "Confirm" },
                { "ButtonCancel", "Cancel" },
                { "ToggleEnabled", "Enabled" },
                { "ToggleDisabled", "Disabled" },
                { "ButtonResetAll", "Reset All Settings" },
                { "ConfirmResetAll", "Are you sure? This will reset all settings to defaults." },
                { "ButtonYes", "Yes" },
                { "ButtonNo", "No" },

                // Notifications
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color>'s traits rerolled! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Enable Trait Replacement" },
                { "NameApplyToExisting", "Apply To Existing Followers" },
                { "NameUseUnlockedTraits", "Use Unlocked Traits Only" },
                { "NameUseAllTraits", "Use All Traits Pool" },
                { "NamePreferExclusive", "Prefer Exclusive Counterparts" },
                { "NamePreserveMutated", "Preserve Rot & Cursed" },
                { "NameMinTraits", "Minimum Traits" },
                { "NameMaxTraits", "Maximum Traits" },
                { "NameRandomizeReindoc", "Randomize on Re-indoctrination" },
                { "NameTraitReroll", "Trait Reroll via Reeducation" },
                { "NameProtectTraitCount", "Protect Trait Count" },
                { "NameRerollableAltar", "Re-rollable Altar Traits" },
                { "NameAllowMultipleUnique", "Allow Multiple Unique Traits" },
                { "NameIncludeTrait", "Include {0}" },
                { "NameGuaranteeTrait", "    └ Guarantee {0}" },
                { "NameShowRemoving", "Show When Removing" },
                { "NameShowAdding", "Show When Adding" },
                { "NameShowReroll", "Show On Reroll" },
                { "NameEnableWeights", "Enable Trait Weights" },
                { "NameIncludeEventTraits", "Include Event Traits" },
                { "NameResetSettings", "Reset All Settings" }
            }
        },
        {
            "Japanese", new()
            {
                { "DescNoNegativeTraits", "ネガティブな特性をポジティブなものに置き換えます。デフォルトでは新しいフォロワーのみに適用されます。「既存のフォロワーに適用」を有効にすると、現在のフォロワーも変更されます。" },
                { "DescApplyToExisting", "有効にすると、特性の置き換えが既存のフォロワーにも適用されます。無効にすると元の特性が復元されます。" },
                { "DescUseUnlockedTraits", "解放済みの特性のみを使用します。特性の置き換えと新しいフォロワーの特性選択の両方に適用されます。" },
                { "DescUseAllTraits", "すべての特性プールを1つに統合し、バニラの通常/レアの分割をバイパスします。これがないと、バニラは別々のプールから特性を割り当てます（デフォルトは通常プール、特性ごとに約10%の確率でレアプール）。特性ウェイトで完全に制御するにはこれを有効にしてください。ユニーク特性は個別のトグルが必要です。" },
                { "DescPreferExclusive", "ネガティブ特性を置き換える際、排他的特性（怠惰など）はランダムな特性ではなく、ポジティブな対になる特性（勤勉）に置き換えられます。" },
                { "DescPreserveMutated", "有効にすると、ロット（変異）フォロワーの特性は削除されません。ロットフォロワーは特定の儀式に役立ちます。" },
                { "DescMinTraits", "新しいフォロワーが持つ特性の最小数。バニラは2です。" },
                { "DescMaxTraits", "新しいフォロワーが持つ特性の最大数。バニラは3です。UIの制約により8が上限です。" },
                { "DescRandomizeReindoc", "既存のフォロワーを再教化する際（祭壇で）、設定されたmin/maxを使用して特性をランダム化します。バニラの再教化は外見/名前のみ変更します。" },
                { "DescTraitReroll", "通常のフォロワーに再教育コマンドを追加します。使用すると、設定されたmin/maxとウェイトを使用して特性がリロールされます。" },
                { "DescProtectTraitCount", "特性をリロールする際（再教育または再教化）、フォロワーの特性数が開始時より少なくならないようにします。" },
                { "DescRerollableAltar", "エクソシズム祭壇使用時、フォロワーを再選択するたびに異なる特性結果が表示されます（1日1回の固定結果ではなく）。" },
                { "DescAllowMultipleUnique", "複数のフォロワーが同じユニーク特性（不死、弟子など）を持つことを許可します。通常、各ユニーク特性は1人のフォロワーのみが持てます。" },
                { "UniqueTraitDesc", "{0}特性（{1}）が特性プールに出現することを許可します。" },
                { "GuaranteeTraitDesc", "新しいフォロワーは必ず{0}特性を受け取ります（ウェイトを無視）。この特性を持てるのは1人のフォロワーのみです。" },
                { "SourceSpecialReward", "通常は特別な報酬" },
                { "SourceCrossover", "クロスオーバー報酬" },
                { "SourceBishopConvert", "通常はビショップを改宗させた際に付与" },
                { "DescShowRemoving", "特性の置き換えがネガティブ特性を削除した際に通知を表示します。" },
                { "DescShowAdding", "特性の置き換えがポジティブ特性を追加した際に通知を表示します。" },
                { "DescShowReroll", "再教育または再教化でフォロワーの特性がリロールされた際に通知を表示します。" },
                { "DescEnableWeights", "新しいフォロワーに重み付きランダム選択を有効にします。ウェイトは各プール内の特性選択に影響します。すべての特性を完全に制御するには「すべての特性プールを使用」を有効にしてください。そうでない場合、バニラの通常/レアプール分割が適用されます（レアプールは特性ごとに約10%の確率）。ウェイトを0に設定すると特性が無効になります。" },
                { "DescIncludeEventTraits", "ゲームプレイイベント（結婚、育児、犯罪、宣教師など）で付与される特性をウェイトリストに含めます。「すべての特性プールを使用」が有効な場合のみ適用されます。警告：不自然な割り当て（子供がいないフォロワーにProudParentなど）が発生する可能性があります。" },
                { "DescResetSettings", "クリックしてすべての設定をデフォルト（バニラの動作）にリセットします。" },
                { "TraitWeightDesc", "ウェイト：高いほど他の特性に比べて出現しやすくなります。0に設定すると無効になります。デフォルトは1.0です。約85の特性がウェイト1の場合：ウェイト10 ~ 10%、ウェイト50 ~ 37%、ウェイト100 ~ 54%。" },
                { "CategoryFoundIn", "所属: {0}" },
                { "CategoryGrantedOther", "その他の方法で付与（教義、儀式、イベントなど）" },
                { "WarningModifyAll", "警告：既存のすべてのフォロワーが変更されます！" },
                { "WarningNecklaceLoss", "ネックレスから付与された特性は復元時に失われる可能性があります。" },
                { "ButtonConfirm", "確認" },
                { "ButtonCancel", "キャンセル" },
                { "ToggleEnabled", "有効" },
                { "ToggleDisabled", "無効" },
                { "ButtonResetAll", "すべての設定をリセット" },
                { "ConfirmResetAll", "よろしいですか？すべての設定がデフォルトにリセットされます。" },
                { "ButtonYes", "はい" },
                { "ButtonNo", "いいえ" },
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color>の特性がリロールされました！（{1} \u2192 {2}）" },

                // Display Names
                { "NameEnableTraitReplacement", "特性置換を有効化" },
                { "NameApplyToExisting", "既存の信者に適用" },
                { "NameUseUnlockedTraits", "解除済み特性のみ使用" },
                { "NameUseAllTraits", "全特性プール使用" },
                { "NamePreferExclusive", "専用対応特性を優先" },
                { "NamePreserveMutated", "腐敗信者を保持" },
                { "NameMinTraits", "最小特性数" },
                { "NameMaxTraits", "最大特性数" },
                { "NameRandomizeReindoc", "再教化時にランダム化" },
                { "NameTraitReroll", "再教育で特性リロール" },
                { "NameProtectTraitCount", "特性数を保護" },
                { "NameRerollableAltar", "祭壇特性を再ロール可能に" },
                { "NameAllowMultipleUnique", "固有特性の複数所持を許可" },
                { "NameIncludeTrait", "{0}を含む" },
                { "NameGuaranteeTrait", "    └ {0}を保証" },
                { "NameShowRemoving", "削除時に表示" },
                { "NameShowAdding", "追加時に表示" },
                { "NameShowReroll", "リロール時に表示" },
                { "NameEnableWeights", "特性重み付けを有効化" },
                { "NameIncludeEventTraits", "イベント特性を含む" },
                { "NameResetSettings", "全設定をリセット" }
            }
        },
        {
            "Russian", new()
            {
                { "DescNoNegativeTraits", "Заменяет негативные черты на позитивные. По умолчанию влияет только на НОВЫХ последователей. Включите 'Применить к существующим' для изменения текущих последователей." },
                { "DescApplyToExisting", "Замена черт также применяется к существующим последователям (не только новым). Отключение восстановит исходные черты." },
                { "DescUseUnlockedTraits", "Использовать только разблокированные черты. Применяется к замене черт и выбору черт для новых последователей." },
                { "DescUseAllTraits", "Объединить все пулы черт в один, обходя ванильное разделение на обычные/редкие. Без этого ванильная игра назначает черты из отдельных пулов (обычный пул по умолчанию, ~10% шанс редкого пула на черту). Включите для полного контроля весов. Уникальные черты требуют индивидуальных переключателей." },
                { "DescPreferExclusive", "При замене негативных черт эксклюзивные черты (Ленивый) заменяются позитивной парой (Трудолюбивый) вместо случайной черты." },
                { "DescPreserveMutated", "Рот (Мутированные) последователи не лишатся своей черты. Рот-последователи механически уникальны и полезны для ритуалов." },
                { "DescMinTraits", "Минимальное количество черт для новых последователей. Ванильное значение: 2." },
                { "DescMaxTraits", "Максимальное количество черт для новых последователей. Ванильное значение: 3. Ограничено 8 из-за интерфейса." },
                { "DescRandomizeReindoc", "При реиндоктринации последователя (на алтаре) рандомизировать черты используя настроенные min/max. Ванильная реиндоктринация меняет только внешность/имя." },
                { "DescTraitReroll", "Добавляет команду 'Перевоспитать' обычным последователям. Перебрасывает черты используя настроенные min/max и веса." },
                { "DescProtectTraitCount", "При перебросе черт (перевоспитание или реиндоктринация) последователь не получит меньше черт, чем было изначально." },
                { "DescRerollableAltar", "При использовании Алтаря Экзорцизма повторный выбор последователя показывает разные результаты каждый раз (а не одинаковые за день)." },
                { "DescAllowMultipleUnique", "Разрешить нескольким последователям иметь одну и ту же уникальную черту (Бессмертный, Ученик и т.д.)." },
                { "UniqueTraitDesc", "Разрешить черте {0} ({1}) появляться в пулах черт." },
                { "GuaranteeTraitDesc", "Новые последователи всегда получают черту {0} (игнорирует веса). Только один последователь может иметь эту черту." },
                { "SourceSpecialReward", "обычно особая награда" },
                { "SourceCrossover", "кроссовер-награда" },
                { "SourceBishopConvert", "обычно даётся при обращении епископа" },
                { "DescShowRemoving", "Показывать уведомления при удалении негативных черт." },
                { "DescShowAdding", "Показывать уведомления при добавлении позитивных черт." },
                { "DescShowReroll", "Показывать уведомление при перебросе черт через перевоспитание или реиндоктринацию." },
                { "DescEnableWeights", "Включить взвешенный случайный выбор для новых последователей. Веса влияют на выбор черт в каждом пуле. Для полного контроля включите 'Использовать все пулы черт' -иначе ванильное разделение на обычный/редкий пул сохраняется (редкий пул ~10% шанс на черту). Установите вес 0, чтобы отключить черту." },
                { "DescIncludeEventTraits", "Включить черты, обычно получаемые через игровые события (брак, родительство, преступления, миссионерство и т.д.) в список весов. Применяется только при включённом 'Использовать все пулы черт'. Предупреждение: может привести к нелогичным назначениям." },
                { "DescResetSettings", "Нажмите для сброса всех настроек к значениям по умолчанию (ванильное поведение)." },
                { "TraitWeightDesc", "Вес: Выше = вероятнее относительно других черт. 0 = отключено. По умолчанию 1.0. При ~85 чертах с весом 1: вес 10 ~ 10%, вес 50 ~ 37%, вес 100 ~ 54%." },
                { "CategoryFoundIn", "Найдено в: {0}" },
                { "CategoryGrantedOther", "Даётся другими способами (доктрины, ритуалы, события и т.д.)" },
                { "WarningModifyAll", "ВНИМАНИЕ: Все существующие последователи будут изменены!" },
                { "WarningNecklaceLoss", "Черты от ожерелий могут быть потеряны при восстановлении." },
                { "ButtonConfirm", "Подтвердить" },
                { "ButtonCancel", "Отмена" },
                { "ToggleEnabled", "Включено" },
                { "ToggleDisabled", "Выключено" },
                { "ButtonResetAll", "Сбросить все настройки" },
                { "ConfirmResetAll", "Вы уверены? Все настройки будут сброшены к значениям по умолчанию." },
                { "ButtonYes", "Да" },
                { "ButtonNo", "Нет" },
                { "NotifyTraitsRerolled", "Черты <color=#FFD201>{0}</color> переброшены! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Включить замену черт" },
                { "NameApplyToExisting", "Применить к существующим" },
                { "NameUseUnlockedTraits", "Только разблокированные черты" },
                { "NameUseAllTraits", "Все черты" },
                { "NamePreferExclusive", "Предпочитать эксклюзивные" },
                { "NamePreserveMutated", "Сохранять гнилых последователей" },
                { "NameMinTraits", "Минимум черт" },
                { "NameMaxTraits", "Максимум черт" },
                { "NameRandomizeReindoc", "Рандомизировать при переобращении" },
                { "NameTraitReroll", "Переброс черт через перевоспитание" },
                { "NameProtectTraitCount", "Защитить количество черт" },
                { "NameRerollableAltar", "Переброс черт алтаря" },
                { "NameAllowMultipleUnique", "Несколько уникальных черт" },
                { "NameIncludeTrait", "Включить {0}" },
                { "NameGuaranteeTrait", "    └ Гарантировать {0}" },
                { "NameShowRemoving", "Показывать при удалении" },
                { "NameShowAdding", "Показывать при добавлении" },
                { "NameShowReroll", "Показывать при перебросе" },
                { "NameEnableWeights", "Включить веса черт" },
                { "NameIncludeEventTraits", "Включить черты событий" },
                { "NameResetSettings", "Сбросить все настройки" }
            }
        },
        {
            "French", new()
            {
                { "DescNoNegativeTraits", "Remplace les traits négatifs par des positifs. Par défaut, n'affecte que les NOUVEAUX suiveurs. Activez 'Appliquer aux suiveurs existants' pour modifier les suiveurs actuels." },
                { "DescApplyToExisting", "Le remplacement des traits s'applique aussi aux suiveurs existants (pas seulement les nouveaux). Désactiver restaurera les traits originaux." },
                { "DescUseUnlockedTraits", "Utiliser uniquement les traits débloqués. S'applique au remplacement et à la sélection des traits des nouveaux suiveurs." },
                { "DescUseAllTraits", "Fusionner tous les pools de traits en un seul, contournant la séparation normal/rare du jeu. Sans cela, le jeu assigne les traits depuis des pools séparés (pool normal par défaut, ~10% de chance du pool rare par trait). Activez ceci pour un contrôle total des poids. Les traits uniques nécessitent leurs propres boutons." },
                { "DescPreferExclusive", "Lors du remplacement des traits négatifs, les traits exclusifs (comme Paresseux) sont remplacés par leur contrepartie positive (Industrieux) au lieu d'un trait aléatoire." },
                { "DescPreserveMutated", "Les suiveurs Rot (Mutés) ne perdront pas leur trait. Les suiveurs Rot sont mécaniquement distincts et utiles pour certains rituels." },
                { "DescMinTraits", "Nombre minimum de traits des nouveaux suiveurs. Vanilla : 2." },
                { "DescMaxTraits", "Nombre maximum de traits des nouveaux suiveurs. Vanilla : 3. Limité à 8 pour des raisons d'interface." },
                { "DescRandomizeReindoc", "Lors de la réendoctrinement d'un suiveur (à l'autel), randomiser ses traits avec les min/max configurés. Le réendoctrinement vanilla ne change que l'apparence/le nom." },
                { "DescTraitReroll", "Ajoute la commande Rééduquer aux suiveurs normaux. L'utiliser relance leurs traits avec les min/max et poids configurés." },
                { "DescProtectTraitCount", "Lors du reroll des traits (rééducation ou réendoctrinement), le suiveur ne se retrouvera pas avec moins de traits qu'au départ." },
                { "DescRerollableAltar", "Avec l'Autel d'Exorcisme, resélectionner un suiveur affiche des résultats différents à chaque fois au lieu du même résultat par jour." },
                { "DescAllowMultipleUnique", "Permettre à plusieurs suiveurs d'avoir le même trait unique (Immortel, Disciple, etc.)." },
                { "UniqueTraitDesc", "Permettre au trait {0} ({1}) d'apparaître dans les pools de traits." },
                { "GuaranteeTraitDesc", "Les nouveaux suiveurs recevront toujours le trait {0} (ignore les poids). Un seul suiveur peut avoir ce trait." },
                { "SourceSpecialReward", "normalement une récompense spéciale" },
                { "SourceCrossover", "récompense crossover" },
                { "SourceBishopConvert", "normalement accordé en convertissant un évêque" },
                { "DescShowRemoving", "Afficher les notifications lors de la suppression de traits négatifs." },
                { "DescShowAdding", "Afficher les notifications lors de l'ajout de traits positifs." },
                { "DescShowReroll", "Afficher une notification quand les traits d'un suiveur sont relancés via rééducation ou réendoctrinement." },
                { "DescEnableWeights", "Activer la sélection aléatoire pondérée pour les nouveaux suiveurs. Les poids affectent la sélection dans chaque pool. Pour un contrôle total, activez 'Utiliser tous les pools de traits' -sinon la séparation normal/rare du jeu s'applique (pool rare ~10% par trait). Mettez un poids à 0 pour désactiver un trait." },
                { "DescIncludeEventTraits", "Inclure les traits normalement accordés par des événements de jeu (mariage, parentalité, criminel, missionnaire, etc.) dans la liste des poids. S'applique uniquement avec 'Utiliser tous les pools de traits'. Attention : peut causer des assignations absurdes." },
                { "DescResetSettings", "Cliquez pour réinitialiser tous les paramètres par défaut (comportement vanilla)." },
                { "TraitWeightDesc", "Poids : Plus élevé = plus probable par rapport aux autres traits. 0 = désactivé. Défaut : 1.0. Avec ~85 traits à poids 1 : poids 10 ~ 10%, poids 50 ~ 37%, poids 100 ~ 54%." },
                { "CategoryFoundIn", "Trouvé dans : {0}" },
                { "CategoryGrantedOther", "Accordé par d'autres moyens (doctrines, rituels, événements, etc.)" },
                { "WarningModifyAll", "ATTENTION : Tous les suiveurs existants seront modifiés !" },
                { "WarningNecklaceLoss", "Les traits des colliers pourraient être perdus lors de la restauration." },
                { "ButtonConfirm", "Confirmer" },
                { "ButtonCancel", "Annuler" },
                { "ToggleEnabled", "Activé" },
                { "ToggleDisabled", "Désactivé" },
                { "ButtonResetAll", "Réinitialiser tous les paramètres" },
                { "ConfirmResetAll", "Êtes-vous sûr ? Tous les paramètres seront réinitialisés par défaut." },
                { "ButtonYes", "Oui" },
                { "ButtonNo", "Non" },
                { "NotifyTraitsRerolled", "Traits de <color=#FFD201>{0}</color> relancés ! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Activer remplacement des traits" },
                { "NameApplyToExisting", "Appliquer aux suivants existants" },
                { "NameUseUnlockedTraits", "Traits débloqués uniquement" },
                { "NameUseAllTraits", "Tous les traits" },
                { "NamePreferExclusive", "Préférer les exclusifs" },
                { "NamePreserveMutated", "Préserver suivants pourris" },
                { "NameMinTraits", "Traits minimum" },
                { "NameMaxTraits", "Traits maximum" },
                { "NameRandomizeReindoc", "Aléatoire lors de réendoctrinement" },
                { "NameTraitReroll", "Relance de traits par rééducation" },
                { "NameProtectTraitCount", "Protéger le nombre de traits" },
                { "NameRerollableAltar", "Traits d'autel relançables" },
                { "NameAllowMultipleUnique", "Plusieurs traits uniques" },
                { "NameIncludeTrait", "Inclure {0}" },
                { "NameGuaranteeTrait", "    └ Garantir {0}" },
                { "NameShowRemoving", "Afficher lors de suppression" },
                { "NameShowAdding", "Afficher lors d'ajout" },
                { "NameShowReroll", "Afficher lors de relance" },
                { "NameEnableWeights", "Activer poids des traits" },
                { "NameIncludeEventTraits", "Inclure traits d'événements" },
                { "NameResetSettings", "Réinitialiser tous les paramètres" }
            }
        },
        {
            "German", new()
            {
                { "DescNoNegativeTraits", "Ersetzt negative Eigenschaften durch positive. Standardmäßig nur für NEUE Anhänger. Aktivieren Sie 'Auf bestehende Anhänger anwenden', um auch aktuelle Anhänger zu aendern." },
                { "DescApplyToExisting", "Eigenschaftsersetzung gilt auch für bestehende Anhänger (nicht nur neue). Deaktivieren stellt die ursprünglichen Eigenschaften wieder her." },
                { "DescUseUnlockedTraits", "Nur freigeschaltete Eigenschaften verwenden. Gilt für Eigenschaftsersetzung und neue Anhänger-Auswahl." },
                { "DescUseAllTraits", "Alle Eigenschaftspools zu einem zusammenführen und die Vanilla-Normal/Selten-Aufteilung umgehen. Ohne dies weist Vanilla Eigenschaften aus getrennten Pools zu (Normalpool standardmäßig, ~10% Chance auf Seltenpool pro Eigenschaft). Aktivieren Sie dies für volle Gewichtskontrolle. Einzigartige Eigenschaften benötigen individuelle Schalter." },
                { "DescPreferExclusive", "Beim Ersetzen negativer Eigenschaften werden exklusive Eigenschaften (wie Faul) durch ihr positives Gegenstück (Fleißig) statt einer zufälligen Eigenschaft ersetzt." },
                { "DescPreserveMutated", "Rot (Mutierte) Anhänger verlieren ihre Eigenschaft nicht. Rot-Anhänger sind mechanisch einzigartig und nützlich für bestimmte Rituale." },
                { "DescMinTraits", "Minimale Anzahl an Eigenschaften für neue Anhänger. Vanilla: 2." },
                { "DescMaxTraits", "Maximale Anzahl an Eigenschaften für neue Anhänger. Vanilla: 3. Auf 8 begrenzt (UI-Einschränkungen)." },
                { "DescRandomizeReindoc", "Bei der Re-Indoktrinierung eines Anhängers (am Altar) werden Eigenschaften mit den konfigurierten Min/Max randomisiert. Vanilla-Re-Indoktrinierung ändert nur Aussehen/Name." },
                { "DescTraitReroll", "Fügt normalen Anhängern den Umerziehen-Befehl hinzu. Würfelt Eigenschaften mit konfigurierten Min/Max und Gewichten neu." },
                { "DescProtectTraitCount", "Beim Neuwürfeln (Umerziehung oder Re-Indoktrinierung) erhält der Anhänger nicht weniger Eigenschaften als zuvor." },
                { "DescRerollableAltar", "Am Exorzismus-Altar zeigt die erneute Auswahl eines Anhängers jedes Mal andere Ergebnisse statt desselben Ergebnisses pro Tag." },
                { "DescAllowMultipleUnique", "Mehreren Anhängern erlauben, dieselbe einzigartige Eigenschaft zu haben (Unsterblich, Schüler usw.)." },
                { "UniqueTraitDesc", "Erlaubt der Eigenschaft {0} ({1}), in Eigenschaftspools zu erscheinen." },
                { "GuaranteeTraitDesc", "Neue Anhänger erhalten immer die Eigenschaft {0} (ignoriert Gewichte). Nur ein Anhänger kann diese Eigenschaft haben." },
                { "SourceSpecialReward", "normalerweise eine besondere Belohnung" },
                { "SourceCrossover", "Crossover-Belohnung" },
                { "SourceBishopConvert", "normalerweise beim Bekehren eines Bischofs gewährt" },
                { "DescShowRemoving", "Benachrichtigungen anzeigen, wenn negative Eigenschaften entfernt werden." },
                { "DescShowAdding", "Benachrichtigungen anzeigen, wenn positive Eigenschaften hinzugefügt werden." },
                { "DescShowReroll", "Benachrichtigung anzeigen, wenn Eigenschaften eines Anhängers durch Umerziehung oder Re-Indoktrinierung neu gewürfelt werden." },
                { "DescEnableWeights", "Gewichtete Zufallsauswahl für neue Anhänger aktivieren. Gewichte beeinflussen die Eigenschaftsauswahl innerhalb jedes Pools. Fuer volle Kontrolle 'Alle Eigenschaftspools verwenden' aktivieren - sonst gilt Vanillas Normal/Selten-Aufteilung (Seltenpool ~10% pro Eigenschaft). Gewicht 0 = deaktiviert." },
                { "DescIncludeEventTraits", "Normalerweise durch Spielereignisse vergebene Eigenschaften (Heirat, Elternschaft, Kriminelle, Missionare usw.) in die Gewichtsliste aufnehmen. Nur bei aktiviertem 'Alle Eigenschaftspools verwenden'. Warnung: Kann unsinnige Zuweisungen verursachen." },
                { "DescResetSettings", "Klicken, um alle Einstellungen auf Standard zurückzusetzen (Vanilla-Verhalten)." },
                { "TraitWeightDesc", "Gewicht: Höher = wahrscheinlicher relativ zu anderen Eigenschaften. 0 = deaktiviert. Standard: 1.0. Bei ~85 Eigenschaften mit Gewicht 1: Gewicht 10 ~ 10%, Gewicht 50 ~ 37%, Gewicht 100 ~ 54%." },
                { "CategoryFoundIn", "Gefunden in: {0}" },
                { "CategoryGrantedOther", "Vergeben durch andere Mittel (Doktrinen, Rituale, Ereignisse usw.)" },
                { "WarningModifyAll", "WARNUNG: Alle bestehenden Anhänger werden geändert!" },
                { "WarningNecklaceLoss", "Eigenschaften von Halsketten könnten bei der Wiederherstellung verloren gehen." },
                { "ButtonConfirm", "Bestätigen" },
                { "ButtonCancel", "Abbrechen" },
                { "ToggleEnabled", "Aktiviert" },
                { "ToggleDisabled", "Deaktiviert" },
                { "ButtonResetAll", "Alle Einstellungen zurücksetzen" },
                { "ConfirmResetAll", "Sind Sie sicher? Alle Einstellungen werden auf Standard zurückgesetzt." },
                { "ButtonYes", "Ja" },
                { "ButtonNo", "Nein" },
                { "NotifyTraitsRerolled", "Eigenschaften von <color=#FFD201>{0}</color> neu gewürfelt! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Eigenschaftsersetzung aktivieren" },
                { "NameApplyToExisting", "Auf vorhandene Anhänger anwenden" },
                { "NameUseUnlockedTraits", "Nur freigeschaltete Eigenschaften" },
                { "NameUseAllTraits", "Alle Eigenschaften" },
                { "NamePreferExclusive", "Exklusive bevorzugen" },
                { "NamePreserveMutated", "Verrottete Anhänger bewahren" },
                { "NameMinTraits", "Minimale Eigenschaften" },
                { "NameMaxTraits", "Maximale Eigenschaften" },
                { "NameRandomizeReindoc", "Zufällig bei Neuindoktrinierung" },
                { "NameTraitReroll", "Eigenschaftsneuverteilung per Umerziehung" },
                { "NameProtectTraitCount", "Eigenschaftsanzahl schützen" },
                { "NameRerollableAltar", "Altar-Eigenschaften neu würfeln" },
                { "NameAllowMultipleUnique", "Mehrere einzigartige Eigenschaften" },
                { "NameIncludeTrait", "{0} einschließen" },
                { "NameGuaranteeTrait", "    └ {0} garantieren" },
                { "NameShowRemoving", "Beim Entfernen anzeigen" },
                { "NameShowAdding", "Beim Hinzufügen anzeigen" },
                { "NameShowReroll", "Beim Neuverteilen anzeigen" },
                { "NameEnableWeights", "Eigenschaftsgewichtung aktivieren" },
                { "NameIncludeEventTraits", "Event-Eigenschaften einschließen" },
                { "NameResetSettings", "Alle Einstellungen zurücksetzen" }
            }
        },
        {
            "Spanish", new()
            {
                { "DescNoNegativeTraits", "Reemplaza rasgos negativos por positivos. Por defecto solo afecta a NUEVOS seguidores. Activa 'Aplicar a seguidores existentes' para modificar los actuales." },
                { "DescApplyToExisting", "El reemplazo de rasgos también se aplica a seguidores existentes (no solo nuevos). Desactivar restaurará los rasgos originales." },
                { "DescUseUnlockedTraits", "Usar solo rasgos desbloqueados. Se aplica al reemplazo y selección de rasgos de nuevos seguidores." },
                { "DescUseAllTraits", "Fusionar todos los pools de rasgos en uno, omitiendo la separación normal/raro del juego. Sin esto, el juego asigna rasgos de pools separados (pool normal por defecto, ~10% de probabilidad del pool raro por rasgo). Actívalo para control total de pesos. Los rasgos únicos requieren sus interruptores individuales." },
                { "DescPreferExclusive", "Al reemplazar rasgos negativos, los rasgos exclusivos (como Perezoso) se reemplazan por su contraparte positiva (Industrioso) en lugar de un rasgo aleatorio." },
                { "DescPreserveMutated", "Los seguidores Rot (Mutados) no perderán su rasgo. Los seguidores Rot son mecánicamente distintos y útiles para ciertos rituales." },
                { "DescMinTraits", "Número mínimo de rasgos de nuevos seguidores. Vanilla: 2." },
                { "DescMaxTraits", "Número máximo de rasgos de nuevos seguidores. Vanilla: 3. Limitado a 8 por restricciones de interfaz." },
                { "DescRandomizeReindoc", "Al reindoctrinar a un seguidor (en el altar), aleatorizar sus rasgos usando los min/max configurados. La reindoctrinación vanilla solo cambia apariencia/nombre." },
                { "DescTraitReroll", "Añade el comando Reeducar a seguidores normales. Relanza sus rasgos usando los min/max y pesos configurados." },
                { "DescProtectTraitCount", "Al relanzar rasgos (reeducación o reindoctrinación), el seguidor no terminará con menos rasgos de los que tenía." },
                { "DescRerollableAltar", "En el Altar de Exorcismo, reseleccionar un seguidor muestra resultados diferentes cada vez en lugar del mismo resultado por día." },
                { "DescAllowMultipleUnique", "Permitir que múltiples seguidores tengan el mismo rasgo único (Inmortal, Discípulo, etc.)." },
                { "UniqueTraitDesc", "Permitir que el rasgo {0} ({1}) aparezca en los pools de rasgos." },
                { "GuaranteeTraitDesc", "Los nuevos seguidores siempre recibirán el rasgo {0} (ignora pesos). Solo un seguidor puede tener este rasgo." },
                { "SourceSpecialReward", "normalmente una recompensa especial" },
                { "SourceCrossover", "recompensa crossover" },
                { "SourceBishopConvert", "normalmente otorgado al convertir un obispo" },
                { "DescShowRemoving", "Mostrar notificaciones al eliminar rasgos negativos." },
                { "DescShowAdding", "Mostrar notificaciones al añadir rasgos positivos." },
                { "DescShowReroll", "Mostrar notificación cuando los rasgos de un seguidor se relanzan por reeducación o reindoctrinación." },
                { "DescEnableWeights", "Activar selección aleatoria ponderada para nuevos seguidores. Los pesos afectan la selección dentro de cada pool. Para control total, activa 'Usar todos los pools' -si no, se aplica la separación normal/raro del juego (pool raro ~10% por rasgo). Peso 0 = desactivado." },
                { "DescIncludeEventTraits", "Incluir rasgos normalmente otorgados por eventos de juego (matrimonio, paternidad, criminal, misionero, etc.) en la lista de pesos. Solo aplica con 'Usar todos los pools de rasgos'. Advertencia: puede causar asignaciones absurdas." },
                { "DescResetSettings", "Clic para restablecer todos los ajustes a los valores predeterminados (comportamiento vanilla)." },
                { "TraitWeightDesc", "Peso: Mayor = más probable respecto a otros rasgos. 0 = desactivado. Predeterminado: 1.0. Con ~85 rasgos a peso 1: peso 10 ~ 10%, peso 50 ~ 37%, peso 100 ~ 54%." },
                { "CategoryFoundIn", "Encontrado en: {0}" },
                { "CategoryGrantedOther", "Otorgado por otros medios (doctrinas, rituales, eventos, etc.)" },
                { "WarningModifyAll", "¡ADVERTENCIA: Todos los seguidores existentes serán modificados!" },
                { "WarningNecklaceLoss", "Los rasgos de collares podrían perderse al restaurar." },
                { "ButtonConfirm", "Confirmar" },
                { "ButtonCancel", "Cancelar" },
                { "ToggleEnabled", "Activado" },
                { "ToggleDisabled", "Desactivado" },
                { "ButtonResetAll", "Restablecer todos los ajustes" },
                { "ConfirmResetAll", "¿Estás seguro? Todos los ajustes se restablecerán a los valores predeterminados." },
                { "ButtonYes", "Sí" },
                { "ButtonNo", "No" },
                { "NotifyTraitsRerolled", "¡Rasgos de <color=#FFD201>{0}</color> relanzados! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Activar reemplazo de rasgos" },
                { "NameApplyToExisting", "Aplicar a seguidores existentes" },
                { "NameUseUnlockedTraits", "Solo rasgos desbloqueados" },
                { "NameUseAllTraits", "Todos los rasgos" },
                { "NamePreferExclusive", "Preferir exclusivos" },
                { "NamePreserveMutated", "Preservar seguidores podridos" },
                { "NameMinTraits", "Rasgos mínimos" },
                { "NameMaxTraits", "Rasgos máximos" },
                { "NameRandomizeReindoc", "Aleatorizar al reindoctrinar" },
                { "NameTraitReroll", "Relanzar rasgos por reeducación" },
                { "NameProtectTraitCount", "Proteger cantidad de rasgos" },
                { "NameRerollableAltar", "Rasgos de altar relanzables" },
                { "NameAllowMultipleUnique", "Varios rasgos únicos" },
                { "NameIncludeTrait", "Incluir {0}" },
                { "NameGuaranteeTrait", "    └ Garantizar {0}" },
                { "NameShowRemoving", "Mostrar al eliminar" },
                { "NameShowAdding", "Mostrar al agregar" },
                { "NameShowReroll", "Mostrar al relanzar" },
                { "NameEnableWeights", "Activar pesos de rasgos" },
                { "NameIncludeEventTraits", "Incluir rasgos de eventos" },
                { "NameResetSettings", "Restablecer toda la configuración" }
            }
        },
        {
            "Portuguese (Brazil)", new()
            {
                { "DescNoNegativeTraits", "Substitui traços negativos por positivos. Por padrão, afeta apenas NOVOS seguidores. Ative 'Aplicar a seguidores existentes' para modificar os atuais." },
                { "DescApplyToExisting", "A substituição de traços também se aplica a seguidores existentes (não apenas novos). Desativar restaurará os traços originais." },
                { "DescUseUnlockedTraits", "Usar apenas traços desbloqueados. Aplica-se à substituição e seleção de traços de novos seguidores." },
                { "DescUseAllTraits", "Mesclar todos os pools de traços em um, ignorando a divisão normal/raro do jogo. Sem isso, o jogo atribui traços de pools separados (pool normal por padrão, ~10% de chance do pool raro por traço). Ative para controle total de pesos. Traços únicos requerem seus interruptores individuais." },
                { "DescPreferExclusive", "Ao substituir traços negativos, traços exclusivos (como Preguiçoso) são substituídos por sua contraparte positiva (Industrioso) em vez de um traço aleatório." },
                { "DescPreserveMutated", "Seguidores Rot (Mutados) não perderão seu traço. Seguidores Rot são mecanicamente distintos e úteis para certos rituais." },
                { "DescMinTraits", "Número mínimo de traços de novos seguidores. Vanilla: 2." },
                { "DescMaxTraits", "Número máximo de traços de novos seguidores. Vanilla: 3. Limitado a 8 por restrições de interface." },
                { "DescRandomizeReindoc", "Ao reindoutrinar um seguidor (no altar), aleatorizar seus traços usando os min/max configurados. A reindoutrinação vanilla só muda aparência/nome." },
                { "DescTraitReroll", "Adiciona o comando Reeducar a seguidores normais. Rerrola seus traços usando os min/max e pesos configurados." },
                { "DescProtectTraitCount", "Ao rerrolar traços (reeducação ou reindoutrinação), o seguidor não ficará com menos traços do que tinha." },
                { "DescRerollableAltar", "No Altar de Exorcismo, resselecionar um seguidor mostra resultados diferentes a cada vez em vez do mesmo resultado por dia." },
                { "DescAllowMultipleUnique", "Permitir que múltiplos seguidores tenham o mesmo traço único (Imortal, Discípulo, etc.)." },
                { "UniqueTraitDesc", "Permitir que o traço {0} ({1}) apareça nos pools de traços." },
                { "GuaranteeTraitDesc", "Novos seguidores sempre receberão o traço {0} (ignora pesos). Apenas um seguidor pode ter este traço." },
                { "SourceSpecialReward", "normalmente uma recompensa especial" },
                { "SourceCrossover", "recompensa crossover" },
                { "SourceBishopConvert", "normalmente concedido ao converter um bispo" },
                { "DescShowRemoving", "Mostrar notificações ao remover traços negativos." },
                { "DescShowAdding", "Mostrar notificações ao adicionar traços positivos." },
                { "DescShowReroll", "Mostrar notificação quando os traços de um seguidor são rerrolados por reeducação ou reindoutrinação." },
                { "DescEnableWeights", "Ativar seleção aleatória ponderada para novos seguidores. Os pesos afetam a seleção dentro de cada pool. Para controle total, ative 'Usar todos os pools' -senão a divisão normal/raro do jogo se aplica (pool raro ~10% por traço). Peso 0 = desativado." },
                { "DescIncludeEventTraits", "Incluir traços normalmente concedidos por eventos de jogo (casamento, paternidade, criminal, missionário, etc.) na lista de pesos. Só se aplica com 'Usar todos os pools de traços'. Aviso: pode causar atribuições absurdas." },
                { "DescResetSettings", "Clique para redefinir todas as configurações para o padrão (comportamento vanilla)." },
                { "TraitWeightDesc", "Peso: Maior = mais provável em relação a outros traços. 0 = desativado. Padrão: 1.0. Com ~85 traços a peso 1: peso 10 ~ 10%, peso 50 ~ 37%, peso 100 ~ 54%." },
                { "CategoryFoundIn", "Encontrado em: {0}" },
                { "CategoryGrantedOther", "Concedido por outros meios (doutrinas, rituais, eventos, etc.)" },
                { "WarningModifyAll", "AVISO: Todos os seguidores existentes serão modificados!" },
                { "WarningNecklaceLoss", "Traços de colares podem ser perdidos ao restaurar." },
                { "ButtonConfirm", "Confirmar" },
                { "ButtonCancel", "Cancelar" },
                { "ToggleEnabled", "Ativado" },
                { "ToggleDisabled", "Desativado" },
                { "ButtonResetAll", "Redefinir todas as configurações" },
                { "ConfirmResetAll", "Tem certeza? Todas as configurações serão redefinidas para o padrão." },
                { "ButtonYes", "Sim" },
                { "ButtonNo", "Não" },
                { "NotifyTraitsRerolled", "Traços de <color=#FFD201>{0}</color> rerrolados! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Ativar substituição de traços" },
                { "NameApplyToExisting", "Aplicar a seguidores existentes" },
                { "NameUseUnlockedTraits", "Apenas traços desbloqueados" },
                { "NameUseAllTraits", "Todos os traços" },
                { "NamePreferExclusive", "Preferir exclusivos" },
                { "NamePreserveMutated", "Preservar seguidores podres" },
                { "NameMinTraits", "Traços mínimos" },
                { "NameMaxTraits", "Traços máximos" },
                { "NameRandomizeReindoc", "Aleatorizar ao redoutrinar" },
                { "NameTraitReroll", "Rerolar traços via reeducação" },
                { "NameProtectTraitCount", "Proteger quantidade de traços" },
                { "NameRerollableAltar", "Traços de altar rerroláveis" },
                { "NameAllowMultipleUnique", "Vários traços únicos" },
                { "NameIncludeTrait", "Incluir {0}" },
                { "NameGuaranteeTrait", "    └ Garantir {0}" },
                { "NameShowRemoving", "Mostrar ao remover" },
                { "NameShowAdding", "Mostrar ao adicionar" },
                { "NameShowReroll", "Mostrar ao rerolar" },
                { "NameEnableWeights", "Ativar pesos de traços" },
                { "NameIncludeEventTraits", "Incluir traços de eventos" },
                { "NameResetSettings", "Redefinir todas as configurações" }
            }
        },
        {
            "Chinese (Simplified)", new()
            {
                { "DescNoNegativeTraits", "将负面特质替换为正面特质。默认仅影响新追随者。启用'应用到现有追随者'以修改当前追随者。" },
                { "DescApplyToExisting", "启用后，特质替换也适用于现有追随者（不仅是新追随者）。禁用将恢复原始特质。" },
                { "DescUseUnlockedTraits", "仅使用已解锁的特质。适用于特质替换和新追随者特质选择。" },
                { "DescUseAllTraits", "将所有特质池合并为一个，绕过原版的普通/稀有分割。没有此项时，游戏从不同池分配特质（默认普通池，每个特质约10%概率来自稀有池）。启用此项可完全控制权重。唯一特质需要单独开关。" },
                { "DescPreferExclusive", "替换负面特质时，排他特质（如懒惰）会被替换为其正面对应特质（勤劳）而非随机特质。" },
                { "DescPreserveMutated", "启用后，腐烂（变异）追随者不会失去其特质。腐烂追随者在机制上独特，对某些仪式有用。" },
                { "DescMinTraits", "新追随者的最少特质数。原版为2。" },
                { "DescMaxTraits", "新追随者的最多特质数。原版为3。受UI限制最多为8。" },
                { "DescRandomizeReindoc", "在祭坛重新教化追随者时，使用配置的最小/最大值随机化其特质。原版重新教化只改变外观/名字。" },
                { "DescTraitReroll", "为普通追随者添加再教育命令。使用后将根据配置的最小/最大值和权重重新掷骰特质。" },
                { "DescProtectTraitCount", "重新掷骰特质时（通过再教育或重新教化），确保追随者的特质数不会少于初始数量。" },
                { "DescRerollableAltar", "使用驱魔祭坛时，重新选择追随者会每次显示不同结果，而非每天固定结果。" },
                { "DescAllowMultipleUnique", "允许多个追随者拥有相同的唯一特质（不朽、门徒等）。通常每个唯一特质只能有一个追随者拥有。" },
                { "UniqueTraitDesc", "允许{0}特质（{1}）出现在特质池中。" },
                { "GuaranteeTraitDesc", "新追随者将始终获得{0}特质（忽略权重）。只有一个追随者可以拥有此特质。" },
                { "SourceSpecialReward", "通常为特殊奖励" },
                { "SourceCrossover", "联动奖励" },
                { "SourceBishopConvert", "通常在转化主教时获得" },
                { "DescShowRemoving", "移除负面特质时显示通知。" },
                { "DescShowAdding", "添加正面特质时显示通知。" },
                { "DescShowReroll", "通过再教育或重新教化重新掷骰追随者特质时显示通知。" },
                { "DescEnableWeights", "为新追随者启用加权随机选择。权重影响每个池内的特质选择。要完全控制所有特质，请启用'使用所有特质池' - 否则原版的普通/稀有池分割仍然适用（稀有池每个特质约10%概率）。将权重设为0可禁用特质。" },
                { "DescIncludeEventTraits", "将通常通过游戏事件（婚姻、育儿、犯罪、传教士等）授予的特质纳入权重列表。仅在启用'使用所有特质池'时适用。警告：可能导致不合理的分配。" },
                { "DescResetSettings", "点击将所有设置重置为默认值（原版行为）。" },
                { "TraitWeightDesc", "权重：越高 = 相对其他特质越可能出现。0 = 禁用。默认1.0。约85个特质权重为1时：权重10 ~ 10%，权重50 ~ 37%，权重100 ~ 54%。" },
                { "CategoryFoundIn", "所属: {0}" },
                { "CategoryGrantedOther", "通过其他方式获得（教义、仪式、事件等）" },
                { "WarningModifyAll", "警告：所有现有追随者都将被修改！" },
                { "WarningNecklaceLoss", "恢复时可能丢失项链赋予的特质。" },
                { "ButtonConfirm", "确认" },
                { "ButtonCancel", "取消" },
                { "ToggleEnabled", "已启用" },
                { "ToggleDisabled", "已禁用" },
                { "ButtonResetAll", "重置所有设置" },
                { "ConfirmResetAll", "确定吗？所有设置将重置为默认值。" },
                { "ButtonYes", "是" },
                { "ButtonNo", "否" },
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color>的特质已重新掷骰！（{1} \u2192 {2}）" },

                // Display Names
                { "NameEnableTraitReplacement", "启用特质替换" },
                { "NameApplyToExisting", "应用于现有信徒" },
                { "NameUseUnlockedTraits", "仅使用已解锁特质" },
                { "NameUseAllTraits", "使用所有特质" },
                { "NamePreferExclusive", "优先专属特质" },
                { "NamePreserveMutated", "保留腐化信徒" },
                { "NameMinTraits", "最少特质数" },
                { "NameMaxTraits", "最多特质数" },
                { "NameRandomizeReindoc", "重新教化时随机" },
                { "NameTraitReroll", "通过再教育重掷特质" },
                { "NameProtectTraitCount", "保护特质数量" },
                { "NameRerollableAltar", "可重掷祭坛特质" },
                { "NameAllowMultipleUnique", "允许多个独特特质" },
                { "NameIncludeTrait", "包含 {0}" },
                { "NameGuaranteeTrait", "    └ 保证 {0}" },
                { "NameShowRemoving", "移除时显示" },
                { "NameShowAdding", "添加时显示" },
                { "NameShowReroll", "重掷时显示" },
                { "NameEnableWeights", "启用特质权重" },
                { "NameIncludeEventTraits", "包含事件特质" },
                { "NameResetSettings", "重置所有设置" }
            }
        },
        {
            "Chinese (Traditional)", new()
            {
                { "DescNoNegativeTraits", "將負面特質替換為正面特質。預設僅影響新追隨者。啟用「套用到現有追隨者」以修改當前追隨者。" },
                { "DescApplyToExisting", "啟用後，特質替換也適用於現有追隨者（不僅是新追隨者）。停用將恢復原始特質。" },
                { "DescUseUnlockedTraits", "僅使用已解鎖的特質。適用於特質替換和新追隨者特質選擇。" },
                { "DescUseAllTraits", "將所有特質池合併為一個，繞過原版的普通/稀有分割。沒有此項時，遊戲從不同池分配特質（預設普通池，每個特質約10%機率來自稀有池）。啟用此項可完全控制權重。唯一特質需要單獨開關。" },
                { "DescPreferExclusive", "替換負面特質時，排他特質（如懶惰）會被替換為其正面對應特質（勤勞）而非隨機特質。" },
                { "DescPreserveMutated", "啟用後，腐爛（變異）追隨者不會失去其特質。腐爛追隨者在機制上獨特，對某些儀式有用。" },
                { "DescMinTraits", "新追隨者的最少特質數。原版為2。" },
                { "DescMaxTraits", "新追隨者的最多特質數。原版為3。受UI限制最多為8。" },
                { "DescRandomizeReindoc", "在祭壇重新教化追隨者時，使用設定的最小/最大值隨機化其特質。原版重新教化只改變外觀/名字。" },
                { "DescTraitReroll", "為普通追隨者新增再教育命令。使用後將根據設定的最小/最大值和權重重新擲骰特質。" },
                { "DescProtectTraitCount", "重新擲骰特質時（透過再教育或重新教化），確保追隨者的特質數不會少於初始數量。" },
                { "DescRerollableAltar", "使用驅魔祭壇時，重新選擇追隨者會每次顯示不同結果，而非每天固定結果。" },
                { "DescAllowMultipleUnique", "允許多個追隨者擁有相同的唯一特質（不朽、門徒等）。通常每個唯一特質只能有一個追隨者擁有。" },
                { "UniqueTraitDesc", "允許{0}特質（{1}）出現在特質池中。" },
                { "GuaranteeTraitDesc", "新追隨者將始終獲得{0}特質（忽略權重）。只有一個追隨者可以擁有此特質。" },
                { "SourceSpecialReward", "通常為特殊獎勵" },
                { "SourceCrossover", "聯動獎勵" },
                { "SourceBishopConvert", "通常在轉化主教時獲得" },
                { "DescShowRemoving", "移除負面特質時顯示通知。" },
                { "DescShowAdding", "新增正面特質時顯示通知。" },
                { "DescShowReroll", "透過再教育或重新教化重新擲骰追隨者特質時顯示通知。" },
                { "DescEnableWeights", "為新追隨者啟用加權隨機選擇。權重影響每個池內的特質選擇。要完全控制所有特質，請啟用「使用所有特質池」 -否則原版的普通/稀有池分割仍然適用（稀有池每個特質約10%機率）。將權重設為0可停用特質。" },
                { "DescIncludeEventTraits", "將通常透過遊戲事件（婚姻、育兒、犯罪、傳教士等）授予的特質納入權重列表。僅在啟用「使用所有特質池」時適用。警告：可能導致不合理的分配。" },
                { "DescResetSettings", "點擊將所有設定重置為預設值（原版行為）。" },
                { "TraitWeightDesc", "權重：越高 = 相對其他特質越可能出現。0 = 停用。預設1.0。約85個特質權重為1時：權重10 ~ 10%，權重50 ~ 37%，權重100 ~ 54%。" },
                { "CategoryFoundIn", "所屬: {0}" },
                { "CategoryGrantedOther", "透過其他方式獲得（教義、儀式、事件等）" },
                { "WarningModifyAll", "警告：所有現有追隨者都將被修改！" },
                { "WarningNecklaceLoss", "恢復時可能遺失項鏈賦予的特質。" },
                { "ButtonConfirm", "確認" },
                { "ButtonCancel", "取消" },
                { "ToggleEnabled", "已啟用" },
                { "ToggleDisabled", "已停用" },
                { "ButtonResetAll", "重置所有設定" },
                { "ConfirmResetAll", "確定嗎？所有設定將重置為預設值。" },
                { "ButtonYes", "是" },
                { "ButtonNo", "否" },
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color>的特質已重新擲骰！（{1} \u2192 {2}）" },

                // Display Names
                { "NameEnableTraitReplacement", "啟用特質替換" },
                { "NameApplyToExisting", "套用至現有信徒" },
                { "NameUseUnlockedTraits", "僅使用已解鎖特質" },
                { "NameUseAllTraits", "使用所有特質" },
                { "NamePreferExclusive", "優先專屬特質" },
                { "NamePreserveMutated", "保留腐化信徒" },
                { "NameMinTraits", "最少特質數" },
                { "NameMaxTraits", "最多特質數" },
                { "NameRandomizeReindoc", "重新教化時隨機" },
                { "NameTraitReroll", "透過再教育重擲特質" },
                { "NameProtectTraitCount", "保護特質數量" },
                { "NameRerollableAltar", "可重擲祭壇特質" },
                { "NameAllowMultipleUnique", "允許多個獨特特質" },
                { "NameIncludeTrait", "包含 {0}" },
                { "NameGuaranteeTrait", "    └ 保證 {0}" },
                { "NameShowRemoving", "移除時顯示" },
                { "NameShowAdding", "新增時顯示" },
                { "NameShowReroll", "重擲時顯示" },
                { "NameEnableWeights", "啟用特質權重" },
                { "NameIncludeEventTraits", "包含事件特質" },
                { "NameResetSettings", "重設所有設定" }
            }
        },
        {
            "Korean", new()
            {
                { "DescNoNegativeTraits", "부정적 특성을 긍정적 특성으로 교체합니다. 기본적으로 새 추종자에게만 적용됩니다. '기존 추종자에게 적용'을 활성화하면 현재 추종자도 변경됩니다." },
                { "DescApplyToExisting", "활성화하면 특성 교체가 기존 추종자에게도 적용됩니다 (새 추종자뿐만 아니라). 비활성화하면 원래 특성이 복원됩니다." },
                { "DescUseUnlockedTraits", "해금된 특성만 사용합니다. 특성 교체와 새 추종자 특성 선택 모두에 적용됩니다." },
                { "DescUseAllTraits", "모든 특성 풀을 하나로 병합하여 바닐라의 일반/희귀 분할을 우회합니다. 이것 없이는 게임이 별도 풀에서 특성을 할당합니다 (기본 일반 풀, 특성당 ~10% 확률로 희귀 풀). 가중치를 완전히 제어하려면 활성화하세요. 고유 특성은 개별 토글이 필요합니다." },
                { "DescPreferExclusive", "부정적 특성 교체 시, 배타적 특성(게으름 등)은 랜덤 특성 대신 긍정적 대응 특성(근면)으로 교체됩니다." },
                { "DescPreserveMutated", "활성화하면 로트(변이) 추종자의 특성이 제거되지 않습니다. 로트 추종자는 특정 의식에 유용합니다." },
                { "DescMinTraits", "새 추종자의 최소 특성 수. 바닐라: 2." },
                { "DescMaxTraits", "새 추종자의 최대 특성 수. 바닐라: 3. UI 제한으로 최대 8." },
                { "DescRandomizeReindoc", "추종자를 재교화할 때 (제단에서) 설정된 최소/최대값으로 특성을 무작위화합니다. 바닐라 재교화는 외모/이름만 변경합니다." },
                { "DescTraitReroll", "일반 추종자에게 재교육 명령을 추가합니다. 사용하면 설정된 최소/최대값과 가중치로 특성이 리롤됩니다." },
                { "DescProtectTraitCount", "특성 리롤 시 (재교육 또는 재교화), 추종자가 시작 시보다 적은 특성을 갖지 않도록 합니다." },
                { "DescRerollableAltar", "퇴마 제단 사용 시, 추종자를 다시 선택하면 매일 같은 결과 대신 매번 다른 결과가 표시됩니다." },
                { "DescAllowMultipleUnique", "여러 추종자가 같은 고유 특성(불멸, 제자 등)을 가질 수 있도록 허용합니다." },
                { "UniqueTraitDesc", "{0} 특성({1})이 특성 풀에 나타나도록 허용합니다." },
                { "GuaranteeTraitDesc", "새 추종자는 항상 {0} 특성을 받습니다 (가중치 무시). 한 추종자만 이 특성을 가질 수 있습니다." },
                { "SourceSpecialReward", "보통 특별 보상" },
                { "SourceCrossover", "크로스오버 보상" },
                { "SourceBishopConvert", "보통 주교 전향 시 부여" },
                { "DescShowRemoving", "부정적 특성 제거 시 알림을 표시합니다." },
                { "DescShowAdding", "긍정적 특성 추가 시 알림을 표시합니다." },
                { "DescShowReroll", "재교육 또는 재교화로 추종자 특성이 리롤될 때 알림을 표시합니다." },
                { "DescEnableWeights", "새 추종자에 대한 가중 무작위 선택을 활성화합니다. 가중치는 각 풀 내 특성 선택에 영향을 줍니다. 완전한 제어를 위해 '모든 특성 풀 사용'을 활성화하세요 -그렇지 않으면 바닐라의 일반/희귀 풀 분할이 적용됩니다 (희귀 풀 특성당 ~10%). 가중치 0 = 비활성화." },
                { "DescIncludeEventTraits", "게임플레이 이벤트(결혼, 육아, 범죄, 선교사 등)로 부여되는 특성을 가중치 목록에 포함합니다. '모든 특성 풀 사용' 활성화 시에만 적용됩니다. 경고: 부적절한 할당이 발생할 수 있습니다." },
                { "DescResetSettings", "클릭하여 모든 설정을 기본값으로 초기화합니다 (바닐라 동작)." },
                { "TraitWeightDesc", "가중치: 높을수록 다른 특성보다 더 자주 나타남. 0 = 비활성화. 기본값 1.0. 약 85개 특성이 가중치 1일 때: 가중치 10 ~ 10%, 가중치 50 ~ 37%, 가중치 100 ~ 54%." },
                { "CategoryFoundIn", "소속: {0}" },
                { "CategoryGrantedOther", "기타 수단으로 부여 (교리, 의식, 이벤트 등)" },
                { "WarningModifyAll", "경고: 모든 기존 추종자가 수정됩니다!" },
                { "WarningNecklaceLoss", "목걸이로 부여된 특성은 복원 시 손실될 수 있습니다." },
                { "ButtonConfirm", "확인" },
                { "ButtonCancel", "취소" },
                { "ToggleEnabled", "활성화" },
                { "ToggleDisabled", "비활성화" },
                { "ButtonResetAll", "모든 설정 초기화" },
                { "ConfirmResetAll", "확실합니까? 모든 설정이 기본값으로 초기화됩니다." },
                { "ButtonYes", "예" },
                { "ButtonNo", "아니오" },
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color>의 특성이 리롤되었습니다! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "특성 교체 활성화" },
                { "NameApplyToExisting", "기존 추종자에게 적용" },
                { "NameUseUnlockedTraits", "해금된 특성만 사용" },
                { "NameUseAllTraits", "모든 특성 사용" },
                { "NamePreferExclusive", "전용 특성 우선" },
                { "NamePreserveMutated", "부패한 추종자 보존" },
                { "NameMinTraits", "최소 특성 수" },
                { "NameMaxTraits", "최대 특성 수" },
                { "NameRandomizeReindoc", "재교화 시 무작위화" },
                { "NameTraitReroll", "재교육으로 특성 리롤" },
                { "NameProtectTraitCount", "특성 수 보호" },
                { "NameRerollableAltar", "제단 특성 리롤 가능" },
                { "NameAllowMultipleUnique", "여러 고유 특성 허용" },
                { "NameIncludeTrait", "{0} 포함" },
                { "NameGuaranteeTrait", "    └ {0} 보장" },
                { "NameShowRemoving", "제거 시 표시" },
                { "NameShowAdding", "추가 시 표시" },
                { "NameShowReroll", "리롤 시 표시" },
                { "NameEnableWeights", "특성 가중치 활성화" },
                { "NameIncludeEventTraits", "이벤트 특성 포함" },
                { "NameResetSettings", "모든 설정 초기화" }
            }
        },
        {
            "Italian", new()
            {
                { "DescNoNegativeTraits", "Sostituisce i tratti negativi con quelli positivi. Per impostazione predefinita influisce solo sui NUOVI seguaci. Attiva 'Applica ai seguaci esistenti' per modificare anche quelli attuali." },
                { "DescApplyToExisting", "La sostituzione dei tratti si applica anche ai seguaci esistenti (non solo ai nuovi). Disattivare ripristinerà i tratti originali." },
                { "DescUseUnlockedTraits", "Usa solo i tratti sbloccati. Si applica alla sostituzione e alla selezione dei tratti dei nuovi seguaci." },
                { "DescUseAllTraits", "Unisci tutti i pool di tratti in uno, aggirando la divisione normale/raro del gioco. Senza questa opzione, il gioco assegna tratti da pool separati (pool normale di default, ~10% di probabilità del pool raro per tratto). Attiva per il pieno controllo dei pesi. I tratti unici richiedono i propri interruttori." },
                { "DescPreferExclusive", "Quando si sostituiscono tratti negativi, i tratti esclusivi (come Pigro) vengono sostituiti con la controparte positiva (Industrioso) anziché un tratto casuale." },
                { "DescPreserveMutated", "I seguaci Rot (Mutati) non perderanno il loro tratto. I seguaci Rot sono meccanicamente distinti e utili per certi rituali." },
                { "DescMinTraits", "Numero minimo di tratti dei nuovi seguaci. Vanilla: 2." },
                { "DescMaxTraits", "Numero massimo di tratti dei nuovi seguaci. Vanilla: 3. Limitato a 8 per vincoli dell'interfaccia." },
                { "DescRandomizeReindoc", "Durante la re-indottrinazione di un seguace (all'altare), randomizza i tratti usando i min/max configurati. La re-indottrinazione vanilla cambia solo aspetto/nome." },
                { "DescTraitReroll", "Aggiunge il comando Rieducare ai seguaci normali. Rilancia i tratti usando i min/max e pesi configurati." },
                { "DescProtectTraitCount", "Durante il rilancio dei tratti (rieducazione o re-indottrinazione), il seguace non finirà con meno tratti di quanti ne avesse." },
                { "DescRerollableAltar", "Nell'Altare dell'Esorcismo, riselezionare un seguace mostra risultati diversi ogni volta invece dello stesso risultato giornaliero." },
                { "DescAllowMultipleUnique", "Permetti a più seguaci di avere lo stesso tratto unico (Immortale, Discepolo, ecc.)." },
                { "UniqueTraitDesc", "Permetti al tratto {0} ({1}) di apparire nei pool di tratti." },
                { "GuaranteeTraitDesc", "I nuovi seguaci riceveranno sempre il tratto {0} (ignora i pesi). Solo un seguace può avere questo tratto." },
                { "SourceSpecialReward", "normalmente una ricompensa speciale" },
                { "SourceCrossover", "ricompensa crossover" },
                { "SourceBishopConvert", "normalmente concesso convertendo un vescovo" },
                { "DescShowRemoving", "Mostra notifiche quando vengono rimossi tratti negativi." },
                { "DescShowAdding", "Mostra notifiche quando vengono aggiunti tratti positivi." },
                { "DescShowReroll", "Mostra notifica quando i tratti di un seguace vengono rilanciati tramite rieducazione o re-indottrinazione." },
                { "DescEnableWeights", "Attiva la selezione casuale ponderata per i nuovi seguaci. I pesi influenzano la selezione all'interno di ogni pool. Per il pieno controllo, attiva 'Usa tutti i pool di tratti' -altrimenti la divisione normale/raro del gioco si applica (pool raro ~10% per tratto). Peso 0 = disattivato." },
                { "DescIncludeEventTraits", "Includi tratti normalmente concessi da eventi di gioco (matrimonio, genitorialità, criminale, missionario, ecc.) nella lista dei pesi. Si applica solo con 'Usa tutti i pool di tratti'. Attenzione: può causare assegnazioni assurde." },
                { "DescResetSettings", "Clicca per ripristinare tutte le impostazioni ai valori predefiniti (comportamento vanilla)." },
                { "TraitWeightDesc", "Peso: Più alto = più probabile rispetto ad altri tratti. 0 = disattivato. Predefinito: 1.0. Con ~85 tratti a peso 1: peso 10 ~ 10%, peso 50 ~ 37%, peso 100 ~ 54%." },
                { "CategoryFoundIn", "Trovato in: {0}" },
                { "CategoryGrantedOther", "Concesso con altri mezzi (dottrine, rituali, eventi, ecc.)" },
                { "WarningModifyAll", "ATTENZIONE: Tutti i seguaci esistenti verranno modificati!" },
                { "WarningNecklaceLoss", "I tratti delle collane potrebbero andare persi durante il ripristino." },
                { "ButtonConfirm", "Conferma" },
                { "ButtonCancel", "Annulla" },
                { "ToggleEnabled", "Attivato" },
                { "ToggleDisabled", "Disattivato" },
                { "ButtonResetAll", "Ripristina tutte le impostazioni" },
                { "ConfirmResetAll", "Sei sicuro? Tutte le impostazioni verranno ripristinate ai valori predefiniti." },
                { "ButtonYes", "Sì" },
                { "ButtonNo", "No" },
                { "NotifyTraitsRerolled", "Tratti di <color=#FFD201>{0}</color> rilanciati! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Attiva sostituzione tratti" },
                { "NameApplyToExisting", "Applica ai seguaci esistenti" },
                { "NameUseUnlockedTraits", "Solo tratti sbloccati" },
                { "NameUseAllTraits", "Tutti i tratti" },
                { "NamePreferExclusive", "Preferisci esclusivi" },
                { "NamePreserveMutated", "Preserva seguaci marci" },
                { "NameMinTraits", "Tratti minimi" },
                { "NameMaxTraits", "Tratti massimi" },
                { "NameRandomizeReindoc", "Casualizza al reindottrinamento" },
                { "NameTraitReroll", "Rilancio tratti tramite rieducazione" },
                { "NameProtectTraitCount", "Proteggi numero di tratti" },
                { "NameRerollableAltar", "Tratti altare rilanciabili" },
                { "NameAllowMultipleUnique", "Più tratti unici" },
                { "NameIncludeTrait", "Includi {0}" },
                { "NameGuaranteeTrait", "    └ Garantisci {0}" },
                { "NameShowRemoving", "Mostra durante rimozione" },
                { "NameShowAdding", "Mostra durante aggiunta" },
                { "NameShowReroll", "Mostra durante rilancio" },
                { "NameEnableWeights", "Attiva pesi dei tratti" },
                { "NameIncludeEventTraits", "Includi tratti eventi" },
                { "NameResetSettings", "Ripristina tutte le impostazioni" }
            }
        },
        {
            "Dutch", new()
            {
                { "DescNoNegativeTraits", "Vervangt negatieve eigenschappen door positieve. Standaard alleen voor NIEUWE volgelingen. Schakel 'Toepassen op bestaande volgelingen' in om ook huidige volgelingen te wijzigen." },
                { "DescApplyToExisting", "Eigenschap-vervanging geldt ook voor bestaande volgelingen (niet alleen nieuwe). Uitschakelen herstelt de originele eigenschappen." },
                { "DescUseUnlockedTraits", "Alleen ontgrendelde eigenschappen gebruiken. Geldt voor eigenschap-vervanging en selectie van nieuwe volgelingen." },
                { "DescUseAllTraits", "Alle eigenschap-pools samenvoegen tot één, waarmee de vanilla normaal/zeldzaam-splitsing wordt omzeild. Zonder dit wijst het spel eigenschappen toe uit aparte pools (normaal pool standaard, ~10% kans op zeldzame pool per eigenschap). Schakel in voor volledige gewichtscontrole. Unieke eigenschappen vereisen individuele schakelaars." },
                { "DescPreferExclusive", "Bij vervanging van negatieve eigenschappen worden exclusieve eigenschappen (zoals Lui) vervangen door hun positieve tegenhanger (IJverig) in plaats van een willekeurige eigenschap." },
                { "DescPreserveMutated", "Rot (Gemuteerde) volgelingen verliezen hun eigenschap niet. Rot-volgelingen zijn mechanisch uniek en nuttig voor bepaalde rituelen." },
                { "DescMinTraits", "Minimum aantal eigenschappen voor nieuwe volgelingen. Vanilla: 2." },
                { "DescMaxTraits", "Maximum aantal eigenschappen voor nieuwe volgelingen. Vanilla: 3. Beperkt tot 8 vanwege UI-beperkingen." },
                { "DescRandomizeReindoc", "Bij her-indoctrinatie van een volgeling (bij het altaar) worden eigenschappen gerandomiseerd met de ingestelde min/max. Vanilla her-indoctrinatie wijzigt alleen uiterlijk/naam." },
                { "DescTraitReroll", "Voegt het Heropvoeden-commando toe aan normale volgelingen. Rolt eigenschappen opnieuw met ingestelde min/max en gewichten." },
                { "DescProtectTraitCount", "Bij het opnieuw rollen van eigenschappen (heropvoeding of her-indoctrinatie) krijgt de volgeling niet minder eigenschappen dan voorheen." },
                { "DescRerollableAltar", "Bij het Exorcisme-altaar toont het opnieuw selecteren van een volgeling elke keer andere resultaten in plaats van hetzelfde resultaat per dag." },
                { "DescAllowMultipleUnique", "Sta meerdere volgelingen toe dezelfde unieke eigenschap te hebben (Onsterfelijk, Discipel, enz.)." },
                { "UniqueTraitDesc", "Sta de {0} eigenschap ({1}) toe om in eigenschap-pools te verschijnen." },
                { "GuaranteeTraitDesc", "Nieuwe volgelingen ontvangen altijd de {0} eigenschap (negeert gewichten). Slechts één volgeling kan deze eigenschap hebben." },
                { "SourceSpecialReward", "normaal een speciale beloning" },
                { "SourceCrossover", "crossover-beloning" },
                { "SourceBishopConvert", "normaal verleend bij het bekeren van een bisschop" },
                { "DescShowRemoving", "Toon meldingen bij het verwijderen van negatieve eigenschappen." },
                { "DescShowAdding", "Toon meldingen bij het toevoegen van positieve eigenschappen." },
                { "DescShowReroll", "Toon melding wanneer eigenschappen van een volgeling opnieuw worden gerold via heropvoeding of her-indoctrinatie." },
                { "DescEnableWeights", "Schakel gewogen willekeurige selectie in voor nieuwe volgelingen. Gewichten beïnvloeden de selectie binnen elke pool. Voor volledige controle, schakel 'Alle eigenschap-pools gebruiken' in -anders geldt vanilla's normaal/zeldzaam-splitsing (zeldzame pool ~10% per eigenschap). Gewicht 0 = uitgeschakeld." },
                { "DescIncludeEventTraits", "Neem eigenschappen op die normaal via gameplay-gebeurtenissen worden verleend (huwelijk, ouderschap, crimineel, missionaris, enz.) in de gewichtenlijst. Alleen van toepassing met 'Alle eigenschap-pools gebruiken'. Waarschuwing: kan onlogische toewijzingen veroorzaken." },
                { "DescResetSettings", "Klik om alle instellingen naar standaard te herstellen (vanilla-gedrag)." },
                { "TraitWeightDesc", "Gewicht: Hoger = waarschijnlijker t.o.v. andere eigenschappen. 0 = uitgeschakeld. Standaard: 1.0. Bij ~85 eigenschappen met gewicht 1: gewicht 10 ~ 10%, gewicht 50 ~ 37%, gewicht 100 ~ 54%." },
                { "CategoryFoundIn", "Gevonden in: {0}" },
                { "CategoryGrantedOther", "Verleend via andere middelen (doctrines, rituelen, gebeurtenissen, enz.)" },
                { "WarningModifyAll", "WAARSCHUWING: Alle bestaande volgelingen worden gewijzigd!" },
                { "WarningNecklaceLoss", "Eigenschappen van kettingen kunnen verloren gaan bij herstel." },
                { "ButtonConfirm", "Bevestigen" },
                { "ButtonCancel", "Annuleren" },
                { "ToggleEnabled", "Ingeschakeld" },
                { "ToggleDisabled", "Uitgeschakeld" },
                { "ButtonResetAll", "Alle instellingen herstellen" },
                { "ConfirmResetAll", "Weet u het zeker? Alle instellingen worden naar standaard hersteld." },
                { "ButtonYes", "Ja" },
                { "ButtonNo", "Nee" },
                { "NotifyTraitsRerolled", "Eigenschappen van <color=#FFD201>{0}</color> opnieuw gerold! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Schakel eigenschapvervanging in" },
                { "NameApplyToExisting", "Toepassen op bestaande volgers" },
                { "NameUseUnlockedTraits", "Alleen ontgrendelde eigenschappen" },
                { "NameUseAllTraits", "Alle eigenschappen" },
                { "NamePreferExclusive", "Geef voorkeur aan exclusieve" },
                { "NamePreserveMutated", "Behoud verrotte volgers" },
                { "NameMinTraits", "Minimum eigenschappen" },
                { "NameMaxTraits", "Maximum eigenschappen" },
                { "NameRandomizeReindoc", "Willekeurig bij herindoctrinatie" },
                { "NameTraitReroll", "Eigenschappen herrollen via heropvoeding" },
                { "NameProtectTraitCount", "Bescherm aantal eigenschappen" },
                { "NameRerollableAltar", "Altaareigenschappen opnieuw rollen" },
                { "NameAllowMultipleUnique", "Meerdere unieke eigenschappen" },
                { "NameIncludeTrait", "Inclusief {0}" },
                { "NameGuaranteeTrait", "    └ Garandeer {0}" },
                { "NameShowRemoving", "Toon bij verwijderen" },
                { "NameShowAdding", "Toon bij toevoegen" },
                { "NameShowReroll", "Toon bij herrollen" },
                { "NameEnableWeights", "Schakel eigenschapgewichten in" },
                { "NameIncludeEventTraits", "Inclusief gebeurteniseigenschappen" },
                { "NameResetSettings", "Reset alle instellingen" }
            }
        },
        {
            "Turkish", new()
            {
                { "DescNoNegativeTraits", "Olumsuz özellikleri olumlu olanlarla değiştirir. Varsayılan olarak yalnızca YENİ takipçileri etkiler. Mevcut takipçileri de değiştirmek için 'Mevcut Takipçilere Uygula'yı etkinleştirin." },
                { "DescApplyToExisting", "Etkinleştirildiğinde, özellik değiştirme mevcut takipçilere de uygulanır (yalnızca yenilere değil). Devre dışı bırakma orijinal özellikleri geri yükler." },
                { "DescUseUnlockedTraits", "Yalnızca açılmış özellikleri kullanır. Özellik değiştirme ve yeni takipçi özellik seçimi için geçerlidir." },
                { "DescUseAllTraits", "Tüm özellik havuzlarını birleştirerek oyunun normal/nadir ayrımını atlar. Bu olmadan oyun ayrı havuzlardan özellik atar (varsayılan normal havuz, özellik başına ~%10 nadir havuz şansı). Ağırlıkları tam kontrol etmek için etkinleştirin. Benzersiz özellikler bireysel düğmeler gerektirir." },
                { "DescPreferExclusive", "Olumsuz özellik değiştirirken, özel özellikler (Tembel gibi) rastgele bir özellik yerine olumlu karşılığı (Çalışkan) ile değiştirilir." },
                { "DescPreserveMutated", "Rot (Mutasyonlu) takipçiler özelliklerini kaybetmez. Rot takipçiler belirli ritüeller için faydalıdır." },
                { "DescMinTraits", "Yeni takipçilerin minimum özellik sayısı. Vanilla: 2." },
                { "DescMaxTraits", "Yeni takipçilerin maksimum özellik sayısı. Vanilla: 3. Arayüz kısıtlamaları nedeniyle 8 ile sınırlı." },
                { "DescRandomizeReindoc", "Bir takipçiyi yeniden doktrine ederken (sunakta), yapılandırılmış min/maks ile özelliklerini rastgeleleştir. Vanilla yeniden doktrinasyon yalnızca görünümü/adı değiştirir." },
                { "DescTraitReroll", "Normal takipçilere Yeniden Eğit komutunu ekler. Yapılandırılmış min/maks ve ağırlıkları kullanarak özellikleri yeniden atar." },
                { "DescProtectTraitCount", "Özellik yeniden atma sırasında (yeniden eğitim veya yeniden doktrinasyon), takipçinin başlangıçtan daha az özellikle kalmamasını sağlar." },
                { "DescRerollableAltar", "Şeytan Çıkarma Sunağı'nda takipçiyi yeniden seçmek her seferinde günlük aynı sonuç yerine farklı sonuçlar gösterir." },
                { "DescAllowMultipleUnique", "Birden fazla takipçinin aynı benzersiz özelliğe (Ölümsüz, Mürit vb.) sahip olmasına izin ver." },
                { "UniqueTraitDesc", "{0} özelliğinin ({1}) özellik havuzlarında görünmesine izin ver." },
                { "GuaranteeTraitDesc", "Yeni takipçiler her zaman {0} özelliğini alacak (ağırlıkları yok sayar). Bu özelliğe yalnızca bir takipçi sahip olabilir." },
                { "SourceSpecialReward", "normalde özel bir ödül" },
                { "SourceCrossover", "çapraz ödül" },
                { "SourceBishopConvert", "normalde bir piskoposu dönüştürürken verilir" },
                { "DescShowRemoving", "Olumsuz özellikler kaldırıldığında bildirim göster." },
                { "DescShowAdding", "Olumlu özellikler eklendiğinde bildirim göster." },
                { "DescShowReroll", "Yeniden eğitim veya yeniden doktrinasyon ile takipçi özellikleri yeniden atandığında bildirim göster." },
                { "DescEnableWeights", "Yeni takipçiler için ağırlıklı rastgele seçimi etkinleştir. Ağırlıklar her havuz içindeki özellik seçimini etkiler. Tam kontrol için 'Tüm Özellik Havuzlarını Kullan'ı etkinleştirin -aksi halde oyunun normal/nadir havuz ayrımı geçerlidir (nadir havuz özellik başına ~%10). Ağırlık 0 = devre dışı." },
                { "DescIncludeEventTraits", "Normalde oyun olayları (evlilik, ebeveynlik, suçlu, misyoner vb.) ile verilen özellikleri ağırlık listesine dahil et. Yalnızca 'Tüm Özellik Havuzlarını Kullan' etkinken geçerlidir. Uyarı: mantıksız atamalara neden olabilir." },
                { "DescResetSettings", "Tüm ayarları varsayılana sıfırlamak için tıklayın (vanilla davranışı)." },
                { "TraitWeightDesc", "Ağırlık: Yüksek = diğer özelliklere göre daha olası. 0 = devre dışı. Varsayılan: 1.0. ~85 özellik ağırlık 1'de: ağırlık 10 ~ %10, ağırlık 50 ~ %37, ağırlık 100 ~ %54." },
                { "CategoryFoundIn", "Bulunduğu yer: {0}" },
                { "CategoryGrantedOther", "Diğer yollarla verilir (doktrinler, ritüeller, olaylar vb.)" },
                { "WarningModifyAll", "UYARI: Tüm mevcut takipçiler değiştirilecek!" },
                { "WarningNecklaceLoss", "Kolyelerden gelen özellikler geri yüklemede kaybolabilir." },
                { "ButtonConfirm", "Onayla" },
                { "ButtonCancel", "İptal" },
                { "ToggleEnabled", "Etkin" },
                { "ToggleDisabled", "Devre Dışı" },
                { "ButtonResetAll", "Tüm Ayarları Sıfırla" },
                { "ConfirmResetAll", "Emin misiniz? Tüm ayarlar varsayılana sıfırlanacak." },
                { "ButtonYes", "Evet" },
                { "ButtonNo", "Hayır" },
                { "NotifyTraitsRerolled", "<color=#FFD201>{0}</color> özellikleri yeniden atandı! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Özellik değiştirmeyi etkinleştir" },
                { "NameApplyToExisting", "Mevcut takipçilere uygula" },
                { "NameUseUnlockedTraits", "Sadece açılmış özellikler" },
                { "NameUseAllTraits", "Tüm özellikler" },
                { "NamePreferExclusive", "Özel özellikleri tercih et" },
                { "NamePreserveMutated", "Çürümüş takipçileri koru" },
                { "NameMinTraits", "Minimum özellikler" },
                { "NameMaxTraits", "Maksimum özellikler" },
                { "NameRandomizeReindoc", "Yeniden doktrine etmede rastgele" },
                { "NameTraitReroll", "Yeniden eğitimle özellik atama" },
                { "NameProtectTraitCount", "Özellik sayısını koru" },
                { "NameRerollableAltar", "Sunak özellikleri yeniden atanabilir" },
                { "NameAllowMultipleUnique", "Birden fazla benzersiz özellik" },
                { "NameIncludeTrait", "{0} dahil et" },
                { "NameGuaranteeTrait", "    └ {0} garanti et" },
                { "NameShowRemoving", "Kaldırırken göster" },
                { "NameShowAdding", "Eklerken göster" },
                { "NameShowReroll", "Yeniden atarken göster" },
                { "NameEnableWeights", "Özellik ağırlıklarını etkinleştir" },
                { "NameIncludeEventTraits", "Etkinlik özelliklerini dahil et" },
                { "NameResetSettings", "Tüm ayarları sıfırla" }
            }
        },
        {
            "French (Canadian)", new()
            {
                { "DescNoNegativeTraits", "Remplace les traits négatifs par des positifs. Par défaut, n'affecte que les NOUVEAUX suiveurs. Activez 'Appliquer aux suiveurs existants' pour modifier les suiveurs actuels." },
                { "DescApplyToExisting", "Le remplacement des traits s'applique aussi aux suiveurs existants (pas seulement les nouveaux). Désactiver restaurera les traits originaux." },
                { "DescUseUnlockedTraits", "Utiliser uniquement les traits débloqués. S'applique au remplacement et à la sélection des traits des nouveaux suiveurs." },
                { "DescUseAllTraits", "Fusionner tous les pools de traits en un seul, contournant la séparation normal/rare du jeu. Sans cela, le jeu assigne les traits depuis des pools séparés (pool normal par défaut, ~10% de chance du pool rare par trait). Activez ceci pour un contrôle total des poids. Les traits uniques nécessitent leurs propres boutons." },
                { "DescPreferExclusive", "Lors du remplacement des traits négatifs, les traits exclusifs (comme Paresseux) sont remplacés par leur contrepartie positive (Industrieux) au lieu d'un trait aléatoire." },
                { "DescPreserveMutated", "Les suiveurs Rot (Mutés) ne perdront pas leur trait. Les suiveurs Rot sont mécaniquement distincts et utiles pour certains rituels." },
                { "DescMinTraits", "Nombre minimum de traits des nouveaux suiveurs. Vanilla : 2." },
                { "DescMaxTraits", "Nombre maximum de traits des nouveaux suiveurs. Vanilla : 3. Limité à 8 pour des raisons d'interface." },
                { "DescRandomizeReindoc", "Lors de la réendoctrinement d'un suiveur (à l'autel), randomiser ses traits avec les min/max configurés. Le réendoctrinement vanilla ne change que l'apparence/le nom." },
                { "DescTraitReroll", "Ajoute la commande Rééduquer aux suiveurs normaux. L'utiliser relance leurs traits avec les min/max et poids configurés." },
                { "DescProtectTraitCount", "Lors du reroll des traits (rééducation ou réendoctrinement), le suiveur ne se retrouvera pas avec moins de traits qu'au départ." },
                { "DescRerollableAltar", "Avec l'Autel d'Exorcisme, resélectionner un suiveur affiche des résultats différents à chaque fois au lieu du même résultat par jour." },
                { "DescAllowMultipleUnique", "Permettre à plusieurs suiveurs d'avoir le même trait unique (Immortel, Disciple, etc.)." },
                { "UniqueTraitDesc", "Permettre au trait {0} ({1}) d'apparaître dans les pools de traits." },
                { "GuaranteeTraitDesc", "Les nouveaux suiveurs recevront toujours le trait {0} (ignore les poids). Un seul suiveur peut avoir ce trait." },
                { "SourceSpecialReward", "normalement une récompense spéciale" },
                { "SourceCrossover", "récompense crossover" },
                { "SourceBishopConvert", "normalement accordé en convertissant un évêque" },
                { "DescShowRemoving", "Afficher les notifications lors de la suppression de traits négatifs." },
                { "DescShowAdding", "Afficher les notifications lors de l'ajout de traits positifs." },
                { "DescShowReroll", "Afficher une notification quand les traits d'un suiveur sont relancés via rééducation ou réendoctrinement." },
                { "DescEnableWeights", "Activer la sélection aléatoire pondérée pour les nouveaux suiveurs. Les poids affectent la sélection dans chaque pool. Pour un contrôle total, activez 'Utiliser tous les pools de traits' -sinon la séparation normal/rare du jeu s'applique (pool rare ~10% par trait). Mettez un poids à 0 pour désactiver un trait." },
                { "DescIncludeEventTraits", "Inclure les traits normalement accordés par des événements de jeu (mariage, parentalité, criminel, missionnaire, etc.) dans la liste des poids. S'applique uniquement avec 'Utiliser tous les pools de traits'. Attention : peut causer des assignations absurdes." },
                { "DescResetSettings", "Cliquez pour réinitialiser tous les paramètres par défaut (comportement vanilla)." },
                { "TraitWeightDesc", "Poids : Plus élevé = plus probable par rapport aux autres traits. 0 = désactivé. Défaut : 1.0. Avec ~85 traits à poids 1 : poids 10 ~ 10%, poids 50 ~ 37%, poids 100 ~ 54%." },
                { "CategoryFoundIn", "Trouvé dans : {0}" },
                { "CategoryGrantedOther", "Accordé par d'autres moyens (doctrines, rituels, événements, etc.)" },
                { "WarningModifyAll", "ATTENTION : Tous les suiveurs existants seront modifiés !" },
                { "WarningNecklaceLoss", "Les traits des colliers pourraient être perdus lors de la restauration." },
                { "ButtonConfirm", "Confirmer" },
                { "ButtonCancel", "Annuler" },
                { "ToggleEnabled", "Activé" },
                { "ToggleDisabled", "Désactivé" },
                { "ButtonResetAll", "Réinitialiser tous les paramètres" },
                { "ConfirmResetAll", "Êtes-vous sûr ? Tous les paramètres seront réinitialisés par défaut." },
                { "ButtonYes", "Oui" },
                { "ButtonNo", "Non" },
                { "NotifyTraitsRerolled", "Traits de <color=#FFD201>{0}</color> relancés ! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "Activer remplacement des traits" },
                { "NameApplyToExisting", "Appliquer aux suivants existants" },
                { "NameUseUnlockedTraits", "Traits débloqués uniquement" },
                { "NameUseAllTraits", "Tous les traits" },
                { "NamePreferExclusive", "Préférer les exclusifs" },
                { "NamePreserveMutated", "Préserver suivants pourris" },
                { "NameMinTraits", "Traits minimum" },
                { "NameMaxTraits", "Traits maximum" },
                { "NameRandomizeReindoc", "Aléatoire lors de réendoctrinement" },
                { "NameTraitReroll", "Relance de traits par rééducation" },
                { "NameProtectTraitCount", "Protéger le nombre de traits" },
                { "NameRerollableAltar", "Traits d'autel relançables" },
                { "NameAllowMultipleUnique", "Plusieurs traits uniques" },
                { "NameIncludeTrait", "Inclure {0}" },
                { "NameGuaranteeTrait", "    └ Garantir {0}" },
                { "NameShowRemoving", "Afficher lors de suppression" },
                { "NameShowAdding", "Afficher lors d'ajout" },
                { "NameShowReroll", "Afficher lors de relance" },
                { "NameEnableWeights", "Activer poids des traits" },
                { "NameIncludeEventTraits", "Inclure traits d'événements" },
                { "NameResetSettings", "Réinitialiser tous les paramètres" }
            }
        },
        {
            "Arabic", new()
            {
                { "DescNoNegativeTraits", "استبدال السمات السلبية بسمات إيجابية. بشكل افتراضي يؤثر فقط على الأتباع الجدد. فعّل 'تطبيق على الأتباع الحاليين' لتعديل الأتباع الحاليين أيضاً." },
                { "DescApplyToExisting", "عند التفعيل، يتم تطبيق استبدال السمات على الأتباع الحاليين أيضاً (ليس فقط الجدد). إلغاء التفعيل سيستعيد السمات الأصلية." },
                { "DescUseUnlockedTraits", "استخدم فقط السمات المفتوحة. ينطبق على استبدال السمات واختيار سمات الأتباع الجدد." },
                { "DescUseAllTraits", "دمج جميع مجموعات السمات في واحدة، متجاوزاً تقسيم اللعبة عادي/نادر. بدون هذا، تعيّن اللعبة سمات من مجموعات منفصلة (المجموعة العادية افتراضياً، ~10% فرصة للمجموعة النادرة لكل سمة). فعّل هذا للتحكم الكامل بالأوزان. السمات الفريدة تتطلب مفاتيح فردية." },
                { "DescPreferExclusive", "عند استبدال السمات السلبية، يتم استبدال السمات الحصرية (مثل كسول) بنظيرها الإيجابي (مجتهد) بدلاً من سمة عشوائية." },
                { "DescPreserveMutated", "أتباع الروت (المتحولون) لن يفقدوا سمتهم. أتباع الروت مميزون ميكانيكياً ومفيدون لبعض الطقوس." },
                { "DescMinTraits", "الحد الأدنى لعدد سمات الأتباع الجدد. الأصلي: 2." },
                { "DescMaxTraits", "الحد الأقصى لعدد سمات الأتباع الجدد. الأصلي: 3. محدود بـ 8 بسبب قيود الواجهة." },
                { "DescRandomizeReindoc", "عند إعادة تلقين تابع (عند المذبح)، يتم تعشير سماته باستخدام الحد الأدنى/الأقصى المكوّن. إعادة التلقين الأصلية تغيّر المظهر/الاسم فقط." },
                { "DescTraitReroll", "يضيف أمر إعادة التعليم للأتباع العاديين. يعيد رمي سماتهم باستخدام الحد الأدنى/الأقصى والأوزان المكوّنة." },
                { "DescProtectTraitCount", "عند إعادة رمي السمات (إعادة التعليم أو إعادة التلقين)، لن يحصل التابع على سمات أقل مما بدأ به." },
                { "DescRerollableAltar", "عند استخدام مذبح طرد الأرواح، إعادة اختيار تابع تُظهر نتائج مختلفة في كل مرة بدلاً من نفس النتيجة يومياً." },
                { "DescAllowMultipleUnique", "السماح لعدة أتباع بامتلاك نفس السمة الفريدة (خالد، تلميذ، إلخ)." },
                { "UniqueTraitDesc", "السماح لسمة {0} ({1}) بالظهور في مجموعات السمات." },
                { "GuaranteeTraitDesc", "سيحصل الأتباع الجدد دائماً على سمة {0} (يتجاهل الأوزان). تابع واحد فقط يمكنه امتلاك هذه السمة." },
                { "SourceSpecialReward", "عادةً مكافأة خاصة" },
                { "SourceCrossover", "مكافأة تعاون" },
                { "SourceBishopConvert", "عادةً تُمنح عند تحويل أسقف" },
                { "DescShowRemoving", "إظهار الإشعارات عند إزالة السمات السلبية." },
                { "DescShowAdding", "إظهار الإشعارات عند إضافة السمات الإيجابية." },
                { "DescShowReroll", "إظهار إشعار عند إعادة رمي سمات تابع عبر إعادة التعليم أو إعادة التلقين." },
                { "DescEnableWeights", "تفعيل الاختيار العشوائي المرجّح للأتباع الجدد. الأوزان تؤثر على اختيار السمات في كل مجموعة. للتحكم الكامل، فعّل 'استخدام جميع مجموعات السمات' -وإلا ينطبق تقسيم المجموعة العادي/النادر (~10% لكل سمة). الوزن 0 = معطّل." },
                { "DescIncludeEventTraits", "تضمين السمات الممنوحة عادةً عبر أحداث اللعب (زواج، أبوة، إجرام، تبشير، إلخ) في قائمة الأوزان. ينطبق فقط مع 'استخدام جميع مجموعات السمات'. تحذير: قد ينتج عنه تعيينات غير منطقية." },
                { "DescResetSettings", "انقر لإعادة تعيين جميع الإعدادات إلى القيم الافتراضية (السلوك الأصلي)." },
                { "TraitWeightDesc", "الوزن: أعلى = أكثر احتمالاً مقارنة بالسمات الأخرى. 0 = معطّل. الافتراضي: 1.0. مع ~85 سمة بوزن 1: وزن 10 ~ 10%، وزن 50 ~ 37%، وزن 100 ~ 54%." },
                { "CategoryFoundIn", "موجود في: {0}" },
                { "CategoryGrantedOther", "يُمنح بطرق أخرى (عقائد، طقوس، أحداث، إلخ)" },
                { "WarningModifyAll", "تحذير: سيتم تعديل جميع الأتباع الحاليين!" },
                { "WarningNecklaceLoss", "قد تُفقد سمات القلادات عند الاستعادة." },
                { "ButtonConfirm", "تأكيد" },
                { "ButtonCancel", "إلغاء" },
                { "ToggleEnabled", "مفعّل" },
                { "ToggleDisabled", "معطّل" },
                { "ButtonResetAll", "إعادة تعيين جميع الإعدادات" },
                { "ConfirmResetAll", "هل أنت متأكد؟ ستتم إعادة تعيين جميع الإعدادات إلى القيم الافتراضية." },
                { "ButtonYes", "نعم" },
                { "ButtonNo", "لا" },
                { "NotifyTraitsRerolled", "تم إعادة رمي سمات <color=#FFD201>{0}</color>! ({1} \u2192 {2})" },

                // Display Names
                { "NameEnableTraitReplacement", "تفعيل استبدال السمات" },
                { "NameApplyToExisting", "تطبيق على الأتباع الحاليين" },
                { "NameUseUnlockedTraits", "السمات المفتوحة فقط" },
                { "NameUseAllTraits", "جميع السمات" },
                { "NamePreferExclusive", "تفضيل الحصرية" },
                { "NamePreserveMutated", "الحفاظ على الأتباع الفاسدين" },
                { "NameMinTraits", "الحد الأدنى من السمات" },
                { "NameMaxTraits", "الحد الأقصى من السمات" },
                { "NameRandomizeReindoc", "عشوائي عند إعادة التلقين" },
                { "NameTraitReroll", "إعادة رمي السمات عبر إعادة التعليم" },
                { "NameProtectTraitCount", "حماية عدد السمات" },
                { "NameRerollableAltar", "سمات المذبح قابلة للرمي" },
                { "NameAllowMultipleUnique", "سمات فريدة متعددة" },
                { "NameIncludeTrait", "تضمين {0}" },
                { "NameGuaranteeTrait", "    └ ضمان {0}" },
                { "NameShowRemoving", "إظهار عند الإزالة" },
                { "NameShowAdding", "إظهار عند الإضافة" },
                { "NameShowReroll", "إظهار عند إعادة الرمي" },
                { "NameEnableWeights", "تفعيل أوزان السمات" },
                { "NameIncludeEventTraits", "تضمين سمات الأحداث" },
                { "NameResetSettings", "إعادة تعيين جميع الإعدادات" }
            }
        }
    };
}
