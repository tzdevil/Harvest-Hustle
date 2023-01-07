using HarvestHustle.Field;
using HarvestHustle.Seed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestHustle.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Header("Event References")]
        [SerializeField] private GameEvents _gameEvents;

        [Header("UI References")]
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _goldYouWillLoseTomorrowText;
        [SerializeField] private TMP_Text _dayText;
        [SerializeField] private TMP_Text _movesLeftText;
        [SerializeField] private TMP_Text _seedChosenText;
        [SerializeField] private Image _endDayImage;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Game Settings")]
        [SerializeField] private int _day;
        [SerializeField] private int _goldYouWillLoseTomorrow;
        [SerializeField] private int _fieldsAvailableCount;

        [Header("Player")]
        [SerializeField] private float _gold;
        [SerializeField] private float _movesLeft;
        [SerializeField] private float _movesLeftMax;

        [Header("Seed Settings")]
        [SerializeField] private SeedBehaviour _seedSelected;

        [Header("Sprites")]
        [SerializeField] private Sprite _endDayBlue;
        [SerializeField] private Sprite _endDayRed;

        private void Awake()
        {
            if (_gameEvents == null)
                _gameEvents = GameObject.Find("GameEvents").GetComponent<GameEvents>();
        }

        private void Start() 
            => Default();

        private void Default()
        {
            _gold = 100;
            _movesLeft = _movesLeftMax;
            _day = 1;
            _fieldsAvailableCount = 4;
            _goldYouWillLoseTomorrow = 25;

            _endDayImage.sprite = _endDayBlue;

            _goldText.SetText($"<color=#D4986B>Gold:</color> {_gold}");
            _dayText.SetText($"<color=#54B5C3>Day: {_day}</color>");
            _movesLeftText.SetText($"<color=#8EC7CF>Moves Left:</color> {_movesLeft} / {_movesLeftMax}");
            _goldYouWillLoseTomorrowText.SetText($"<color=#AB4949>Gold you will lose tomorrow:</color> {_goldYouWillLoseTomorrow}");
        }

        #region Event Manager
        private void OnEnable()
        {
            _gameEvents.OnTryingToBuySeed += OnTryingToBuySeed;
            _gameEvents.OnPlantedSeed += OnPlantedSeed;
            _gameEvents.OnTryingToHarvestField += OnTryingToHarvestField;
        }

        private void OnDisable()
        {
            _gameEvents.OnTryingToBuySeed -= OnTryingToBuySeed;
            _gameEvents.OnPlantedSeed -= OnPlantedSeed;
            _gameEvents.OnTryingToHarvestField -= OnTryingToHarvestField;
        }

        private void OnTryingToBuySeed(SeedBehaviour seed, string seedName, int cost, int income, int moveNeeded, int dayNeeded)
        {
            if (_fieldsAvailableCount <= 0)
            {
                _gameEvents.RaiseWarningBox("There are no available fields.");
                return;
            }

            if (_movesLeft < moveNeeded)
            {
                _gameEvents.RaiseWarningBox($"You don't have enough moves to plant {moveNeeded - _movesLeft}.");
                return;
            }

            if (_seedSelected != null)
            {
                _gameEvents.RaiseWarningBox("You are already holding a seed.");
                return;
            }

            if (_gold >= cost)
            {
                _seedSelected = seed;
                _gold -= cost;
                _goldText.SetText($"<color=#D4986B>Gold:</color> {_gold}");
                _seedChosenText.SetText($"<color=#B7CAEE>Seed chosen:</color> {seedName}");
                _seedChosenText.gameObject.SetActive(true);
                _gameEvents.RaiseBuySeed(seed, seedName, cost, income, moveNeeded, dayNeeded);
            }
            else
                _gameEvents.RaiseWarningBox("You don't have enough money.");
        }

        private void OnPlantedSeed(SeedBehaviour seed, int moveNeeded)
        {
            _movesLeft -= moveNeeded;
            _fieldsAvailableCount--;
            _seedSelected = null;

            if (_movesLeft <= 0)
                _endDayImage.sprite = _endDayBlue;

            _seedChosenText.gameObject.SetActive(false);
            _movesLeftText.SetText($"<color=#8EC7CF>Moves Left:</color> {_movesLeft} / {_movesLeftMax}");
        }

        private void OnTryingToHarvestField(FieldBehaviour field, int income)
        {
            if (_movesLeft > 0)
            {
                field.HarvestField();
                HarvestedField(income);
            }
            else
                _gameEvents.RaiseWarningBox("You don't have enough moves to harvest (1).");
        }
        #endregion

        // Event Trigger
        public void OnClickDayEnded()
        {
            if (_seedSelected != null)
            {
                _gameEvents.RaiseWarningBox("You have to plant the seed you hold before ending the day.");
                return;
            }

            if (_gold <= 0)
            {
                GameOver();
                return;
            }

            _gold -= _goldYouWillLoseTomorrow;

            if (_gold <= 0)
                _gameEvents.RaiseWarningBox("If the gold doesn't get above 0 today, you will lose.");

            _movesLeft = _movesLeftMax;
            _day++;

            if (_day % 2 == 0)
                _gameEvents.RaiseChangeMeta();

            if (_day % 3 == 0)
                _goldYouWillLoseTomorrow += 25;

            _endDayImage.sprite = _endDayBlue;

            _goldText.SetText($"<color=#D4986B>Gold:</color> {_gold}");
            _dayText.SetText($"<color=#54B5C3>Day: {_day}</color>");
            _movesLeftText.SetText($"<color=#8EC7CF>Moves Left:</color> {_movesLeft} / {_movesLeftMax}");
            _goldYouWillLoseTomorrowText.SetText($"<color=#AB4949>Gold you will lose tomorrow:</color> {_goldYouWillLoseTomorrow}");

            _gameEvents.RaiseDayEnded();
        }

        private void HarvestedField(int income)
        {
            _movesLeft--;
            _fieldsAvailableCount++;
            _gold += income;

            if (_movesLeft <= 0)
                _endDayImage.sprite = _endDayBlue;

            _movesLeftText.SetText($"<color=#8EC7CF>Moves Left:</color> {_movesLeft} / {_movesLeftMax}");
            _goldText.SetText($"<color=#D4986B>Gold:</color> {_gold}");
        }

        private void GameOver() 
            => _gameOverPanel.SetActive(true);
    }
}