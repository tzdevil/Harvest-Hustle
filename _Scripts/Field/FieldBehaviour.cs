using HarvestHustle.Gameplay;
using HarvestHustle.Seed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestHustle.Field
{
    public class FieldBehaviour : MonoBehaviour
    {
        [Header("Event References")]
        [SerializeField] private GameEvents _gameEvents;

        [Header("Field Availability References & Settings")]
        [SerializeField] private GameObject _fieldAvailable;

        [Header("Hovering References")]
        [SerializeField] private GameObject _fieldInfoPanel;
        [SerializeField] private TMP_Text _seedNameText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _daysLeftText;

        [Header("Chosen Seed Settings")]
        [SerializeField] private SeedBehaviour _seed;
        [SerializeField] private string _seedName;
        [SerializeField] private int _income;
        [SerializeField] private int _dayNeeded;
        [SerializeField] private int _moveNeeded;

        [Header("Field Settings")]
        [SerializeField] private Image _fieldSprite;
        [SerializeField] private bool _seedAlreadyPlanted;
        [SerializeField] private bool _readyForHarvest;
        [SerializeField] private int _daysLeft;

        [Header("Sprites")]
        [SerializeField] private Sprite _fieldWithSeed;
        [SerializeField] private Sprite _fieldWithoutSeed;

        private void Awake()
        {
            if (_gameEvents == null)
                _gameEvents = GameObject.Find("GameEvents").GetComponent<GameEvents>();
        }

        #region Event Manager
        private void OnEnable()
        {
            _gameEvents.OnBuySeed += OnBuySeed;
            _gameEvents.OnDayEnded += OnDayEnded;
            _gameEvents.OnPlantedSeed += OnPlantedSeed;
        }

        private void OnDisable()
        {
            _gameEvents.OnBuySeed -= OnBuySeed;
            _gameEvents.OnDayEnded -= OnDayEnded;
            _gameEvents.OnPlantedSeed -= OnPlantedSeed;
        }

        private void OnBuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded)
        {
            if (_seedAlreadyPlanted)
                return;

            _fieldAvailable.SetActive(true);
            _seedName = seedName;
            _seed = seed;
            _income = income;
            _dayNeeded = dayNeeded;
            _moveNeeded = moveNeeded;
        }

        private void OnDayEnded()
        {
            _gameEvents.RaiseWarningBox("A day passed.");

            if (_seed == null)
                return;

            _daysLeft--;

            if (_daysLeft == 0)
                _readyForHarvest = true;

            if (_daysLeft < 0)
                _income = (int)(_income * .8f);
        }

        private void OnPlantedSeed(SeedBehaviour seed, int moveNeeded)
        {
            _fieldAvailable.SetActive(false);

            if (!_seedAlreadyPlanted)
                _seed = null;
        }
        #endregion

        // Event Trigger.
        public void OnClickField()
        {
            if (_seed == null)
                return;

            if (_readyForHarvest)
                TryToHarvestField();
            else if (!_seedAlreadyPlanted)
                PlantSeed();
        }

        private void TryToHarvestField()
            => _gameEvents.RaiseTryingToHarvestField(this, _income);

        public void HarvestField()
        {
            _gameEvents.RaiseWarningBox($"Harvested {_seedName}.");

            _fieldAvailable.SetActive(false);

            _readyForHarvest = false;
            _seed = null;
            _seedAlreadyPlanted = false;
            _fieldSprite.sprite = _fieldWithoutSeed;
        }

        private void PlantSeed()
        {
            _seedAlreadyPlanted = true;
            _daysLeft = _dayNeeded;
            _fieldSprite.sprite = _fieldWithSeed;

            _gameEvents.RaiseWarningBox($"{_seedName} planted.");
            _gameEvents.RaisePlantedSeed(_seed, _moveNeeded);
        }

        public void OnHoverEnterField()
        {
            if (!_seedAlreadyPlanted)
                return;

            _seedNameText.SetText(_seedName);
            _incomeText.SetText($"Income: {_income}");

            if (_daysLeft < 0)
                _daysLeftText.SetText($"{_daysLeft} day late.");
            else if (_daysLeft > 0)
                _daysLeftText.SetText($"Days Left: {_daysLeft}");
            else
                _daysLeftText.SetText("Click to harvest.");

            _fieldInfoPanel.SetActive(true);
        }

        public void OnHoverExitField()
        {
            if (!_seedAlreadyPlanted)
                return;

            _fieldInfoPanel.SetActive(false);
        }
    }
}