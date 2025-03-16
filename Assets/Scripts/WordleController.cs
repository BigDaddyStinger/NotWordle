using UnityEngine;
using TMPro;

public class WordleController : MonoBehaviour
{
    public WordleModel model;
    public WordleView view;

    [Header("UI References")]
    public TMP_InputField inputField;

    // You might have a "Submit" Button too, which you wire up via the Inspector.
    // public Button submitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameSetup()
    {
        // 1) Clear the input field
        inputField.text = "";

        // 2) Call model.Setup() to pick a new random word, reset attempts
        model.Setup();

        // 3) Call view.Setup() to clear out the board visually
        view.Setup();
    }

    // Called when user presses "Submit"
    public void SubmitGuess()
    {
        string guess = inputField.text.Trim().ToLower();

        // Check validation
        if (!model.IsValidGuess(guess))
        {
            Debug.Log("Invalid guess. Must be 5 letters, in dictionary, and not repeated.");
            return;
        }

        // Submit to the model
        model.RecordGuess(guess);
        model.UpdateCells(guess);

        // Update the view
        view.UpdateView(model.cells);

        // Check if correct
        if (guess == model.correctAnswer)
        {
            WinGame();
            return;
        }

        if (model.currentAttempt >= 6)
        {
            LoseGame();
            return;
        }

        // If still going, clear the input field for next guess
        inputField.text = "";
    }

    private void WinGame()
    {
        Debug.Log("You Win! The answer was: " + model.correctAnswer);
        // Optionally disable future input or show a "New Game" button.
    }

    private void LoseGame()
    {
        Debug.Log("You Lose! The correct word was: " + model.correctAnswer);
        // Optionally disable input or show a "New Game" button.
    }
}
