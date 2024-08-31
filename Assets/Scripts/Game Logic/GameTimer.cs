using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGeneratorAndSolverDemo
{
    public class GameTimer : MonoBehaviour
    {
        [Tooltip("Duration in seconds")]
        [SerializeField] private int duration = 60;
        [SerializeField] private bool startOnAutomaticly = true;

        [Range(0, .99f)] [Tooltip("After the given percentage passed, an event will trigger. keep the value 0 if you don't want to trigger.")]
        [SerializeField] private float criticalTimePercentage = .9f;
        [SerializeField] private TMP_Text countdownText;

        [Space]
        public UnityEvent OnTimeFinish;

        [Space]
        [Space]
        public UnityEvent OnCriticalTime;

        private Coroutine timerCoroutine;
        private bool isCriticalTimeReached = false;

        private void Start()
        {
            if (startOnAutomaticly)
            {
                timerCoroutine = StartCoroutine(StartCountdown());
            }
        }
        public void StartTimer(int duration = 60)
        {
            this.duration = duration;
            isCriticalTimeReached = false;
            timerCoroutine = StartCoroutine(StartCountdown());
        }
        public void StopTimer()
        {
            if(timerCoroutine != null)
                StopCoroutine( timerCoroutine );

            timerCoroutine = null;
        }
        private IEnumerator StartCountdown()
        {
            int timer = duration;
            while(timer >= 0)
            {
                CheckForCriticalTime(timer);
                string minutes = (timer / 60).ToString("D2");
                string seconds = (timer % 60).ToString("D2");

                countdownText.text = string.Format("{0} : {1}", minutes, seconds);
                yield return new WaitForSeconds(1f);

                timer -= 1;
            }

            OnTimeFinish?.Invoke();
        }
        private void CheckForCriticalTime(float timePassed)
        {
            float timePassedPercentage = (duration - timePassed) / duration;

            if(timePassedPercentage >= criticalTimePercentage && !isCriticalTimeReached)
            {
                isCriticalTimeReached = true;
                OnCriticalTime?.Invoke();
            }
        }
    }
}
