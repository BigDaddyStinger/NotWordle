using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WordleModel : MonoBehaviour
{

    [Header("Text Files")]
    public TextAsset possibleAnswersAsset;
    public TextAsset allowedWordsAsset;

    private string[] possibleAnswers;
    private string[] allowedWords;
    private HashSet<string> usedGuesses = new HashSet<string>();

    public Cell[,] cells = new Cell[6, 5]; // 6 rows, 5 columns
    public int currentAttempt = 0;
    public string correctAnswer = string.Empty;

    // ...

    void Awake()
    {
        // Split by line breaks
        possibleAnswers = possibleAnswersAsset.text
            .Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToLower())
            .ToArray();

        allowedWords = allowedWordsAsset.text
            .Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToLower())
            .ToArray();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    public void Setup()
    {
        // Create the grid
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                cells[row, col] = new Cell();
            }
        }

        // Choose a random answer
        // (During testing, you could fix an answer like "apple")
        int randIndex = Random.Range(0, possibleAnswers.Length);
        correctAnswer = possibleAnswers[randIndex].Trim().ToLower();

        currentAttempt = 0;
    }

    public class Cell
    {
        public char letter;
        public Color color;

        // Constructor
        public Cell()
        {
            letter = ' ';
            color = Color.white; // default to white
        }
    }

    public bool IsValidGuess(string guess)
    {
        guess = guess.Trim().ToLower();
        if (guess.Length != 5) return false;
        if (!possibleAnswers.Contains(guess) && !allowedWords.Contains(guess)) return false;
        if (usedGuesses.Contains(guess)) return false;
        return true;
    }

    public void RecordGuess(string guess)
    {
        usedGuesses.Add(guess);
    }

    public void UpdateCells(string guess)
    {
        guess = guess.ToLower();

        // Step 1: Mark everything gray initially
        for (int col = 0; col < 5; col++)
        {
            cells[currentAttempt, col].letter = guess[col];
            cells[currentAttempt, col].color = Color.gray;
        }

        // We’ll keep track of how many times each letter occurs in the answer
        // so we know how many leftover letters remain for "yellow" statuses.
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();
        for (int i = 0; i < correctAnswer.Length; i++)
        {
            char c = correctAnswer[i];
            if (!letterCounts.ContainsKey(c)) letterCounts[c] = 0;
            letterCounts[c]++;
        }

        // Step 2: Mark green (exact matches) & decrement letterCounts
        for (int col = 0; col < 5; col++)
        {
            char g = guess[col];
            if (g == correctAnswer[col])
            {
                cells[currentAttempt, col].color = Color.green;
                letterCounts[g]--;
            }
        }

        // Step 3: Mark yellow if letter is in the word and leftover is available
        for (int col = 0; col < 5; col++)
        {
            char g = guess[col];
            // only if not green
            if (cells[currentAttempt, col].color != Color.green)
            {
                if (letterCounts.ContainsKey(g) && letterCounts[g] > 0)
                {
                    cells[currentAttempt, col].color = Color.yellow;
                    letterCounts[g]--;
                }
            }
        }

        currentAttempt++;
    }
}
