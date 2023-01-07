using UnityEngine;

namespace HarvestHustle.Seed
{
    [CreateAssetMenu(fileName = "New Seed", menuName = "Create New/Seed", order = 51)]
    public class SeedSO : ScriptableObject
    {
        [Header("Seed Information")]
        [SerializeField] private string _seedName;
        [SerializeField] private Sprite _seedSprite;

        [Header("Seed Settings")]
        [SerializeField] private int _costMin;
        [SerializeField] private int _costMax;
        [SerializeField] private int _incomeMin; // income = cost + this.
        [SerializeField] private int _incomeMax;
        [SerializeField] private int _daysNeededToHarvest;
        [SerializeField, Range(1, 4)] private int _movesNeededToPlant;

        #region Properties
        public string SeedName
        {
            get => _seedName;
            set => _seedName = value;
        }

        public Sprite SeedSprite
        {
            get => _seedSprite;
            set => _seedSprite = value;
        }

        public int CostMin
        {
            get => _costMin;
            set => _costMin = value;
        }
        
        public int CostMax
        {
            get => _costMax;
            set => _costMax = value;
        }

        public int IncomeMin
        {
            get => _incomeMin;
            set => _incomeMin = value;
        }

        public int IncomeMax
        {
            get => _incomeMax;
            set => _incomeMax = value;
        }

        public int DaysNeededToHarvest
        {
            get => _daysNeededToHarvest;
            set => _daysNeededToHarvest = value;
        }

        public int MovesNeededToPlant
        {
            get => _movesNeededToPlant;
            set => _movesNeededToPlant = value;
        }
        #endregion
    }
}