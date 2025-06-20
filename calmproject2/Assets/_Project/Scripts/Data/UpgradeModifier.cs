namespace SurvivalChaos
{
    [System.Serializable]
    public class UpgradeModifier
    {
        public string stat;
        public float value;

        /// <summary>
        /// Applies this modifier to the provided UpgradeManager.
        /// </summary>
        public void Apply(UpgradeManager manager)
        {
            if (manager == null || string.IsNullOrEmpty(stat))
                return;

            manager.ApplyModifier(stat, value);
        }
    }
}
