using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestHustle.Gameplay
{
    public class WarningManager : MonoBehaviour
    {
        [Header("Event References")]
        [SerializeField] private GameEvents _gameEvents;

        [Header("Warning References & Settings")]
        [SerializeField] private Image _warningPanel;
        [SerializeField] private TMP_Text _warningText;
        [SerializeField] private float _fadingTime;
        [SerializeField] private Color _warningPanelColor;
        [SerializeField] private Color _warningTextColor;

        private void Awake()
        {
            _warningPanelColor = _warningPanel.color;
            _warningTextColor = _warningText.color;
        }

        #region Event Manager
        private void OnEnable()
        {
            _gameEvents.OnWarningBox += OnWarningBox;
        }

        private void OnDisable()
        {
            _gameEvents.OnWarningBox -= OnWarningBox;
        }

        private void OnWarningBox(string warning)
        {
            _warningText.SetText(warning);
            _fadingTime = 2f;
        }
        #endregion

        private void Update()
        {
            if (_fadingTime >= 0)
            {
                _fadingTime -= Time.deltaTime;
                _warningPanel.color = new Color(_warningPanelColor.r, _warningPanelColor.g, _warningPanelColor.b, _fadingTime);
                _warningText.color = new Color(_warningTextColor.r, _warningTextColor.g, _warningTextColor.b, _fadingTime);
            }
        }
    }
}