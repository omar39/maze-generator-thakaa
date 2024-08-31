using TMPro;
using UnityEngine;
namespace MazeGeneratorAndSolverDemo.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText, scoreText, validMoves;
        private PlayerManager player;

        public void AssignPlayer(PlayerManager player)
        {
            this.player = player;

            if(GameManager.Instance.MovesRestricted)
                player.Controller.OnPlayerMoved += UpdateNumberOfMoves;

            player.OnHealthChanged += UpdateHealthUI;
            player.OnScoreChanged += UpdateScoreUI;

            healthText.text = player.Health.ToString();
            scoreText.text = player.Score.ToString();
            validMoves.text = player.CurrentValidMoves == -1 ? " - " : player.CurrentValidMoves.ToString();
        }
        private void UpdateHealthUI(float health)
        {
            healthText.text = health.ToString();
        }
        private void UpdateScoreUI(float score)
        {
            scoreText.text = score.ToString();
        }
        private void UpdateNumberOfMoves()
        {
            validMoves.text = player.CurrentValidMoves.ToString();
        }
    }

}