using System.Collections;
using MazeGeneratorAndSolverDemo.Player;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo.Collectables
{
    public class Collectable : MonoBehaviour
    {
        private bool rotatingEffect = false;

        public UnityAction<Collectable> OnCollected;
        private void OnEnable()
        {
            // make appear effect;
            StartCoroutine(AppearCollectable());
        }
        private void Update()
        {
            if(rotatingEffect)
            {
                transform.Rotate(0, Time.deltaTime * 5f, 0);
            }    
        }
        private IEnumerator AppearCollectable()
        {
            Vector3 targetScale = transform.localScale;
            Vector3 scaleVelocity = new Vector3(.1f, .1f, .1f);

            transform.localScale = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 180, 0);


            yield return new WaitUntil(() =>
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref scaleVelocity, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f);

                return transform.eulerAngles.y <= .1f && transform.localScale.magnitude >= targetScale.magnitude;
            });

            rotatingEffect = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            print("collectable collided");
            // make a reacion
            PlayerManager player;
            if(other.TryGetComponent(out player))
            {
                OnCollected?.Invoke(this);
            }
        }
    }
}

