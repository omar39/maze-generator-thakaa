using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGeneratorAndSolverDemo
{
    public class GameSetupUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown gameModeDropdown;
        [SerializeField] private TMP_InputField numberOfMovesInput, timeDurationInput;
        [SerializeField] private Toggle enableLimitedMoves, allowTime;
        [SerializeField] private Button startGameButton;

        private GameMode selectedGameMode;
        private int numberOfMoves = -1;
        private int timeDuration = -1;

        private void Start()
        {
            // put game modes in dropdown

            gameModeDropdown.ClearOptions();
            foreach(var gameMode in System.Enum.GetValues(typeof(GameMode))) 
            {
                gameModeDropdown.options.Add(new TMP_Dropdown.OptionData(gameMode.ToString()));
            }
            gameModeDropdown.RefreshShownValue();
            gameModeDropdown.onValueChanged.AddListener(OnGameModeSelect);

            numberOfMovesInput.onValueChanged.AddListener(OnNumberOfMovesChanged);
            timeDurationInput.onValueChanged.AddListener(OnTimeDurationChanged);
            startGameButton.onClick.AddListener( StartGame );
        }
        private void OnGameModeSelect(int index)
        {
            selectedGameMode = (GameMode) index;
        }
        private void OnNumberOfMovesChanged(string val)
        {
            int.TryParse(val, out numberOfMoves);
        }
        private void OnTimeDurationChanged(string val)
        {
            int.TryParse(val, out timeDuration);
        }
        private void StartGame()
        {
            GameManager.Instance.StartGame(selectedGameMode, 
                numberOfMoves,
                enableLimitedMoves.isOn,
                timeDuration, allowTime.isOn);
        }
    }

}
