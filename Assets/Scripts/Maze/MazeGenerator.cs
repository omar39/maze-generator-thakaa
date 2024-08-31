using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using MazeGeneratorAndSolverDemo.Collectables;
using MazeGeneratorAndSolverDemo.Obstacles;

namespace MazeGeneratorAndSolverDemo.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        private const string OBSTACLES_RESOURCES = "Obstacles";
        public static MazeGenerator Instance { get; private set; } = null;

        [Header("Parameters")]
        [SerializeField] private MazeCell mazeCellPrefab;
        [SerializeField] private Collectable collectablePrefab;

        public UnityAction OnMazeGenerationComplete, OnMazeGenerationReset;
        public List<Collectable> Collectables {get; private set;} = new List<Collectable>();
        public Transform RootPosition { get { return mazeGrid == null ? null : mazeGrid[0, 0].transform ;} }
        public int MazeWidth { get { return mazeWidth; } }
        public int MazeLength { get { return mazeLength; } }

        private List<Vector3> emptyCells = new List<Vector3>();
        private List<Obstacle> obstacles = new List<Obstacle>();
        private int mazeWidth = 0, mazeLength = 0;
        private int numberOfTargtes = 0, numberOfObstacles = 0;
        private MazeCell[,] mazeGrid;
        private Obstacle[] obstaclesPrefabs;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            obstaclesPrefabs = Resources.LoadAll<Obstacle>(OBSTACLES_RESOURCES);
        }
        public void StartMazeGeneration(int mazeWidth, int mazeLength, int numberOfTargets, int numberOfObstacles)
        {
            ClearOldMaze();
            emptyCells = new List<Vector3>();
            Collectables = new List<Collectable>();
            obstacles = new List<Obstacle>();

            this.mazeWidth = mazeWidth;
            this.mazeLength = mazeLength;
            this.numberOfTargtes = numberOfTargets;
            this.numberOfObstacles = numberOfObstacles;

            mazeGrid = new MazeCell[mazeWidth, mazeLength];

            for (int x = 0; x < mazeWidth; x++)
            {
                for (int z = 0; z < mazeLength; z++)
                {
                    mazeGrid[x, z] = Instantiate(mazeCellPrefab, 
                        new Vector3(x, 0, z), Quaternion.identity, transform);
                }
            }

            StartCoroutine( SetupMaze() );
        }
        private void ClearOldMaze()
        {
            OnMazeGenerationReset?.Invoke();
            
            // destroy all children
            for(int i = 0;i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

        }
        private IEnumerator SetupMaze()
        {
            yield return StartCoroutine( GenerateMaze(null, mazeGrid[0, 0]) );

            SpawnObstacles();
            GenerateCollectables();
            
            OnMazeGenerationComplete?.Invoke();
        }
        private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(previousCell, currentCell);
            MazeCell nextCell;

            yield return new WaitForSeconds(.03f);

            do
            {
                nextCell = GetNextUnvisitedCell(currentCell);
                
                if (nextCell != null)
                {
                    emptyCells.Add(nextCell.transform.position);

                    yield return GenerateMaze(currentCell, nextCell);
                }


            } while (nextCell != null);
        }

        private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
        {
            var unvisitedCells = GetUnvisitedCells(currentCell);

            return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
        }

        private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
        {
            int x = (int) currentCell.transform.position.x;
            int z = (int) currentCell.transform.position.z;

            if (x + 1 < mazeWidth)
            {
                var cellToRight = mazeGrid[x + 1, z];
                
                if (cellToRight.IsVisited == false)
                {
                    yield return cellToRight;
                }
            }

            if (x - 1 >= 0)
            {
                var cellToLeft = mazeGrid[x - 1, z];

                if (cellToLeft.IsVisited == false)
                {
                    yield return cellToLeft;
                }
            }

            if (z + 1 < mazeLength)
            {
                var cellToFront = mazeGrid[x, z + 1];

                if (cellToFront.IsVisited == false)
                {
                    yield return cellToFront;
                }
            }

            if (z - 1 >= 0)
            {
                var cellToBack = mazeGrid[x, z - 1];

                if (cellToBack.IsVisited == false)
                {
                    yield return cellToBack;
                }
            }
        }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
        {
            if (previousCell == null)
            {
                return;
            }

            if (previousCell.transform.position.x < currentCell.transform.position.x)
            {
                previousCell.ClearRightWall();
                currentCell.ClearLeftWall();
                return;
            }

            if (previousCell.transform.position.x > currentCell.transform.position.x)
            {
                previousCell.ClearLeftWall();
                currentCell.ClearRightWall();
                return;
            }

            if (previousCell.transform.position.z < currentCell.transform.position.z)
            {
                previousCell.ClearFrontWall();
                currentCell.ClearBackWall();
                return;
            }

            if (previousCell.transform.position.z > currentCell.transform.position.z)
            {
                previousCell.ClearBackWall();
                currentCell.ClearFrontWall();
                return;
            }
        }
        private void GenerateCollectables()
        {
            while ( TryPlaceCollectable(GetRandomEmptyCell()) );
        }
        private Vector3 GetRandomEmptyCell()
        {
            int index = Random.Range(0, emptyCells.Count);
            Vector3 place = emptyCells[index];
            emptyCells.RemoveAt(index);
            return place;
        }
        private bool TryPlaceCollectable(Vector3 place)
        {
            if(Collectables.Count < numberOfTargtes)
            {
                var insance = Instantiate(collectablePrefab, place, Quaternion.identity, transform);
                Collectables.Add(insance);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void SpawnObstacles()
        {
            while (TryPlaceObstacle(GetRandomEmptyCell()));
        }
        private bool TryPlaceObstacle(Vector3 place)
        {
            if(obstacles.Count < numberOfObstacles)
            {
                var randomObstacle = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];

                var instance = Instantiate( randomObstacle, place, Quaternion.identity, transform);
                obstacles.Add(instance);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}