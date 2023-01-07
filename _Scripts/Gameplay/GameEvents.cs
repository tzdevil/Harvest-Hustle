using HarvestHustle.Field;
using HarvestHustle.Seed;
using UnityEngine;

namespace HarvestHustle.Gameplay
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void TryingToBuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded);
        public delegate void BuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded);
        public delegate void PlantedSeed(SeedBehaviour seed, int moveNeeded);
        public delegate void TryingToHarvestField(FieldBehaviour field, int income);
        public delegate void DayEnded();
        public delegate void ChangeMeta();
        public delegate void WarningBox(string warning);

        public event TryingToBuySeed OnTryingToBuySeed;
        public event BuySeed OnBuySeed;
        public event PlantedSeed OnPlantedSeed;
        public event TryingToHarvestField OnTryingToHarvestField;
        public event DayEnded OnDayEnded;
        public event ChangeMeta OnChangeMeta;
        public event WarningBox OnWarningBox;

        public void RaiseTryingToBuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded)
            => OnTryingToBuySeed?.Invoke(seed, seedName, cost, income, moveNeeded, dayNeeded);
        public void RaiseBuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded)
            => OnBuySeed?.Invoke(seed, seedName, cost, income, moveNeeded, dayNeeded);
        public void RaisePlantedSeed(SeedBehaviour seed, int moveNeeded)
            => OnPlantedSeed?.Invoke(seed, moveNeeded);
        public void RaiseTryingToHarvestField(FieldBehaviour field, int income)
            => OnTryingToHarvestField?.Invoke(field, income);
        public void RaiseDayEnded()
            => OnDayEnded?.Invoke();
        public void RaiseChangeMeta()
            => OnChangeMeta?.Invoke();
        public void RaiseWarningBox(string warning)
            => OnWarningBox?.Invoke(warning);
    }
}