using UnityEngine;
using System.Collections.Generic;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private Lever[] levers = new Lever[8];
    [SerializeField] private GameObject[] letters = new GameObject[8];

    // Define combinations using lever indices (starting from 0)
    private List<int[]> leverCombinations = new List<int[]>
    {
        new int[] {0, 1, 6},       // Combination for letter 1
        new int[] {2, 5, 6},       // Combination for letter 2
        new int[] {1, 3, 4},       // Combination for letter 3
        new int[] {0, 2, 5},       // Combination for letter 4
        new int[] {4, 5, 6},       // Combination for letter 5
        new int[] {0, 3, 4},       // Combination for letter 6
        new int[] {2, 4, 5},       // Combination for letter 7
    };

    void Update()
    {
        CheckLeversAndEnableLetter();
    }

    private void CheckLeversAndEnableLetter()
    {
        for (int i = 0; i < leverCombinations.Count; i++)
        {
            if (IsCombinationActive(leverCombinations[i]))
            {
                EnableOnlyLetter(i);
                return;
            }
        }

        // If no combination matched, disable all letters
        EnableOnlyLetter(-1);
    }

    private bool IsCombinationActive(int[] combination)
    {
        for (int i = 0; i < levers.Length; i++)
        {
            bool shouldBeOn = System.Array.IndexOf(combination, i) != -1;
            if (levers[i].IsLeverOn != shouldBeOn)
            {
                return false;
            }
        }
        return true;
    }

    private void EnableOnlyLetter(int indexToEnable)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i] != null)
            {
                letters[i].SetActive(i == indexToEnable);
            }
        }
    }
}
