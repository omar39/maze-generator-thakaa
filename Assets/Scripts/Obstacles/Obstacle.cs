using MazeGeneratorAndSolverDemo.Player;
using UnityEngine;

namespace MazeGeneratorAndSolverDemo.Obstacles
{
    public abstract class Obstacle : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        protected abstract void OnPlayerCollision();

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player;
            if (other.TryGetComponent(out player))
            {
                OnPlayerCollision();
                // take damage
                player.TakeDamage(damage);
            }    
        }
    }
}
