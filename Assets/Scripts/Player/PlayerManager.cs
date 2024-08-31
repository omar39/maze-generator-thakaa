using MazeGeneratorAndSolverDemo.Collectables;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private PlayerUI playerUI;
        private int numberOfMoves = -1;
        private int collectedItems = 0;
        private PlayerController controller;
        public UnityAction<float> OnHealthChanged, OnScoreChanged;
        public int CurrentValidMoves { get{ return numberOfMoves; } }
        public float Health { get { return health; } }
        public int Score { get { return collectedItems; } }
        public PlayerController Controller { get { return controller; } }
        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            controller.OnPlayerMoved += PerformAMove;

            numberOfMoves = GameManager.Instance.ValidMoves;
            
            playerUI.AssignPlayer(this);
        }
        public void TakeDamage(float damage)
        {
            health -= damage;
            OnHealthChanged?.Invoke(health);

            if(health <= 0)
            {
                // lose!
                GameManager.Instance.LoseGame();
            }
        }
        public void GainPoint()
        {
            collectedItems ++;
            OnScoreChanged?.Invoke(collectedItems);
        }
        public void PerformAMove()
        {
            if(!GameManager.Instance.MovesRestricted) return;
            
            numberOfMoves --;
            if(numberOfMoves <= 0)
            {
                // lose!
                GameManager.Instance.LoseGame();
            }
        }
    }

}