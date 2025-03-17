using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static WordleModel;

public class WordleView : MonoBehaviour
{
    public Transform[] rows; // each row has 5 children squares


    public void Setup()
    {
        // Clear the board
        for (int r = 0; r < rows.Length; r++)
        {
            for (int c = 0; c < rows[r].childCount; c++)
            {
                var square = rows[r].GetChild(c);
                var letterText = square.GetComponentInChildren<TMP_Text>();
                var bgImage = square.GetComponent<Image>();

                letterText.text = "";
                bgImage.color = Color.white;
            }
        }
    }

    public void UpdateView(Cell[,] cells)
    {
        for (int r = 0; r < cells.GetLength(0); r++)
        {
            for (int c = 0; c < cells.GetLength(1); c++)
            {
                var square = rows[r].GetChild(c);
                var letterText = square.GetComponentInChildren<TMP_Text>();
                var bgImage = square.GetComponent<Image>();

                char letter = cells[r, c].letter;
                Color color = cells[r, c].color;

                // If there's a letter, display it, else blank
                letterText.text = letter == ' ' ? "" : letter.ToString().ToUpper();
                bgImage.color = color;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
