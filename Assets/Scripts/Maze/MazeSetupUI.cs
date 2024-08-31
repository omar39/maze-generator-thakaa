using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGeneratorAndSolverDemo.Maze
{
    public class MazeSetupUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputWidth, inputLength, collectablesNumber, obstaclesNumber;
        [SerializeField] private int widthConstraint, lengthConstraint;
        [SerializeField] private Button generateButton;
        private int widthVal = 0, lengthVal = 0, collectablesVal = 0, obstaclesVal = 0;

        private void Start()
        {
            generateButton.interactable = false;
            generateButton.onClick.AddListener(GenerateMaze);

            inputWidth.onValueChanged.AddListener(OnWidthValueChanged);
            inputLength.onValueChanged.AddListener(OnLengthValueChanged);
            collectablesNumber.onValueChanged.AddListener(OnCollectablesValueChanged);
            obstaclesNumber.onValueChanged.AddListener(OnObstaclesValueChanged);

            MazeGenerator.Instance.OnMazeGenerationComplete += () => generateButton.interactable = true;
        }
        private void TryEnableGenerateButton()
        {
            bool validInput = widthConstraint >= widthVal &&
            lengthConstraint >= lengthVal &&
            widthVal > 0 &&
            lengthVal > 0 && 
            collectablesVal > 0 &&
            obstaclesVal > 0;
                     
            if(validInput)
            {
                generateButton.interactable = true;
            }
            else
            {
                generateButton.interactable = false;
            }
        }
        private void OnWidthValueChanged(string val)
        {
            if(int.TryParse(val, out widthVal))
            {
                TryEnableGenerateButton();
            }     
        }
        private void OnLengthValueChanged(string val)
        {
            if(int.TryParse(val, out lengthVal))
            {
                TryEnableGenerateButton();
            }
        }
        private void OnCollectablesValueChanged(string val)
        {
            if(int.TryParse(val, out collectablesVal))
            {
                TryEnableGenerateButton();
            }
        }
        private void OnObstaclesValueChanged(string val)
        {
            if(int.TryParse(val, out obstaclesVal))
            {
                TryEnableGenerateButton();
            }
        }
        private void GenerateMaze()
        {
            MazeGenerator.Instance.StartMazeGeneration(widthVal, lengthVal, collectablesVal, obstaclesVal);

            generateButton.interactable = false;
        }
    }

}