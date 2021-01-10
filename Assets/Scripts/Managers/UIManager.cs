using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Baks.Core.Utils;

namespace Baks.Core.Managers
{
    public class UIManager : Singleton<UIManager> 
    {
        public enum GameState { Playing, GameOver }
        private GameState _currentGameState = GameState.Playing;

        [SerializeField]
        private GameObject m_gameOverPanel = default;

        [SerializeField]
        private TextMeshProUGUI m_pointsText = default;

        [SerializeField]
        [Range(1, 10)]
        [Tooltip("Set the initial points available by default")]
        private int m_points = 5;

        public int Points { get => m_points; set => m_points = value; }

        private void Awake() => m_pointsText.text = $"{m_points}";
        
        public void RestartGame() => SceneManager.LoadScene(0);

        public void IncrementPoints()
        {
            m_points++;
            m_pointsText.text = $"{m_points}";
        }

        public void DecrementPoints()
        {
            if (m_points > 0)
            {
                m_points--;
                m_pointsText.text = $"{m_points}";
            }

            if (m_points == 0)
            {
                _currentGameState = GameState.GameOver;
                m_gameOverPanel.SetActive(true);
            }
        }
    }
}