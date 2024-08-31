using UnityEngine;

namespace MazeGeneratorAndSolverDemo.Obstacles
{
    public class PassiveObstacle : Obstacle
    {
        protected override void OnPlayerCollision()
        {
            print("Passive Obstacle");
        }
    }
}
