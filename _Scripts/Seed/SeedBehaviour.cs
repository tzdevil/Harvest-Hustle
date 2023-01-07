using HarvestHustle.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestHustle.Seed
{
    public class SeedBehaviour : MonoBehaviour
    {
        private System.Random random;

        [Header("Event References")]
        [SerializeField] private GameEvents _gameEvents;

        [Header("UI References")]
        [SerializeField] private TMP_Text _seedNameText;
        [SerializeField] private Image _seedSpriteTexture;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _moveNeededText;
        [SerializeField] private TMP_Text _dayNeededText;

        [Header("Seed")]
        [SerializeField] private SeedSO _seed;

        [Header("Seed Information")]
        [SerializeField] private string _seedName;
        [SerializeField] private Sprite _seedSprite;

        [Header("Seed Settings")]
        [SerializeField] private int _cost;
        [SerializeField] private int _income;
        [SerializeField] private int _moveNeeded;
        [SerializeField] private int _dayNeeded;
        [SerializeField] private int _metaValue;

        private void Awake()
        {
            random = new();

            if (_gameEvents == null)
                _gameEvents = GameObject.Find("GameEvents").GetComponent<GameEvents>();
        }

        private void Start()
        {
            if (_seed != null)
                SetSeed(_seed);
        }

        #region Event Manager
        private void OnEnable()
        {
            _gameEvents.OnChangeMeta += OnChangeMeta;
        }

        private void OnDisable()
        {
            _gameEvents.OnChangeMeta -= OnChangeMeta;
        }

        private void OnChangeMeta()
        {
            _metaValue += random.Next(2, 6);
            _cost = random.Next(_seed.CostMin, _seed.CostMax + 1) + _metaValue;
            _income = (int)(_cost * .8f) + random.Next(_seed.IncomeMin, _seed.IncomeMax + 1) + _metaValue;
            
            SetSeedUI();
        }
        #endregion

        public void SetSeed(SeedSO seed)
        {
            _seed = seed;
            _seedName = seed.SeedName;
            _seedSprite = seed.SeedSprite;
            _cost = random.Next(seed.CostMin, seed.CostMax);
            _income = _cost + random.Next(seed.IncomeMin, seed.IncomeMax);
            _moveNeeded = seed.MovesNeededToPlant;
            _dayNeeded = seed.DaysNeededToHarvest;

            SetSeedUI();
        }

        private void SetSeedUI()
        {
            _seedNameText.SetText(_seedName);
            _seedSpriteTexture.sprite = _seedSprite;
            _costText.SetText($"Cost: {_cost}");
            _incomeText.SetText($"Income: {_income}");
            _moveNeededText.SetText($"Moves needed: {_moveNeeded}");
            _dayNeededText.SetText($"Days needed: {_dayNeeded}");
        }

        public void OnClickBuy()
        {
            _gameEvents.RaiseTryingToBuySeed(this, _seedName, _cost, _income, _moveNeeded, _dayNeeded);
        }
    }
}