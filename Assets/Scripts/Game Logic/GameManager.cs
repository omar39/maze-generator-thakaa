using System.Collections.Generic;
using MazeGeneratorAndSolverDemo.Collectables;
using MazeGeneratorAndSolverDemo.Maze;
using MazeGeneratorAndSolverDemo.Player;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo
{
    public enum GameMode
    {
        Normal,
        CollectByOrder
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Parameters")]
        [SerializeField] private PlayerManager player;
        [SerializeField] private GameMode gameMode;
        [SerializeField] private GameTimer timer;
        [SerializeField] private CollectableArrowIndicator arrowIndicator;
        [SerializeField] private bool gameWithTime = false;
        [SerializeField] private bool limitedMoves = false;

        [Header("UI")]
        [SerializeField] private GameObject mazeSetupUI;
        [SerializeField] private GameObject gameplayUI;

        [Space]
        public UnityEvent OnGameWin;
        [Space]
        public UnityEvent OnGameLost;

        public int ValidMoves { get { return numberOfValidMoves; } }
        public bool MovesRestricted { get { return limitedMoves;} }
        public Collectable NextCollectable { get { return nextCollectable; } }
        private int numberOfValidMoves = -1;
        private int gameTimer = 60;
        private Collectable nextCollectable = null;
        private List<Collectable> collectables = new List<Collectable>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            arrowIndicator.gameObject.SetActive(false);
        }
        public void StartGame()
        {
            InitializeGame();
        }
        public void StartGame(GameMode gameMode, int numberOfValidMoves, bool limitedMoves, int gameTimer, bool gameWithTime)
        {
            this.gameMode = gameMode;
            this.numberOfValidMoves = numberOfValidMoves;
            this.limitedMoves = limitedMoves;
            this.gameTimer = gameTimer;
            this.gameWithTime = gameWithTime;

            InitializeGame();
        }
        private void InitializeGame()
        {
            if(gameWithTime)
            {
                timer.StartTimer(gameTimer);
            }
            if(gameMode == GameMode.CollectByOrder)
            {
                SetOrderedCollectables();
                arrowIndicator.gameObject.SetActive(true);

                nextCollectable = collectables[0];
                arrowIndicator.SetArrowPointsAt(nextCollectable);
            }
            else
            {
                SetNormalCollectable();
            }

            player.transform.position = MazeGenerator.Instance.RootPosition.position;
            player.gameObject.SetActive(true);

            mazeSetupUI.SetActive(false);
            gameplayUI.SetActive(true);
        }
        private void SetOrderedCollectables()
        {
            collectables = MazeGenerator.Instance.Collectables;
            foreach(var collectable in collectables)
            {
                collectable.OnCollected += ProceedToNextCollectable;
            }
        }
        private void SetNormalCollectable()
        {
            collectables = MazeGenerator.Instance.Collectables;
            foreach(var collectable in collectables)
            {
                collectable.OnCollected += Collect;
            }
        }
        private void Collect(Collectable collectable)
        {
            var temp = collectable;
            Destroy(collectable.gameObject);

            collectables.Remove(temp);
            player.GainPoint();

            if(collectables.Count == 0)
            {
                WinGame();
            }
        }
        private void ProceedToNextCollectable(Collectable collectable)
        {
            if(collectable == nextCollectable)
            {
                Destroy(collectable.gameObject);
                collectables.RemoveAt(0);
                if(collectables.Count > 0)
                {
                    nextCollectable = collectables[0];
                    arrowIndicator.SetArrowPointsAt(nextCollectable);
                }
                else
                {
                    // celebrate winning
                    WinGame();
                    arrowIndicator.gameObject.SetActive(false);
                }
                player.GainPoint();
            }
        }
        public void WinGame()
        {
            OnGameWin?.Invoke();
        }
        public void LoseGame()
        {
            OnGameLost?.Invoke();
        }
    }
}
