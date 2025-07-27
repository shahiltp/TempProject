using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    public static event Action<int> OnMatchFound; // int: points
    public static event Action OnMismatch;
    public static event Action OnAllMatchesFound;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform gridContainer;

    private List<Card> cards = new List<Card>();
    private Card firstFlippedCard;
    private Card secondFlippedCard;
    internal static bool isChecking = false;
    private int matchesFound = 0;
    private int totalMatches = 0;
    private List<Card> allCards = new List<Card>();
    private Queue<Card> flippedCardsQueue = new Queue<Card>();

    private void OnEnable()
    {
        GameManager.OnGameStarted += SetupBoard;
        GameManager.OnGameRestarted += SetupBoard;
        Card.OnCardClicked += HandleCardClicked;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= SetupBoard;
        GameManager.OnGameRestarted -= SetupBoard;
        Card.OnCardClicked -= HandleCardClicked;
    }

    private void SetupBoard(int rows, int cols)
    {
        ClearBoard();
        totalMatches = (rows * cols) / 2;
        matchesFound = 0;

        GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = cols;
        }

        GenerateGrid(rows, cols);
        
        StartCoroutine(ProcessFlippedCardsCoroutine());
    }
    private void SetupBoard()
    {
        SetupBoard(GameManager.Instance.rows, GameManager.Instance.cols);
    }

    private void GenerateGrid(int rows, int cols)
    {
        List<int> cardValues = GenerateCardValues(totalMatches);
        Shuffle(cardValues);

        for (int i = 0; i < cardValues.Count; i++)
        {
            GameObject newCardObj = Instantiate(cardPrefab, gridContainer);
            Card newCard = newCardObj.GetComponent<Card>();
            newCard.Initialize(cardValues[i]);
            allCards.Add(newCard);
        }
    }
    
    private void HandleCardClicked(Card card)
    {
        if (card.isFlipped || card.isMatched) return;

        card.FlipUp();
        flippedCardsQueue.Enqueue(card);
    }
    
    private IEnumerator ProcessFlippedCardsCoroutine()
    {
        while (matchesFound < totalMatches)
        {
            if (flippedCardsQueue.Count >= 2)
            {
                
                Card card1 = flippedCardsQueue.Dequeue();
                Card card2 = flippedCardsQueue.Dequeue();

                
                yield return new WaitForSeconds(0.75f);
                
                if (card1.cardValue == card2.cardValue)
                {
                    // Matched man
                    card1.OnMatchFound();
                    card2.OnMatchFound();
                    matchesFound++;
                    OnMatchFound?.Invoke(10); 
                    if (matchesFound == totalMatches)
                    {
                        OnAllMatchesFound?.Invoke();
                        yield break; 
                    }
                }
                else
                {
                    card1.FlipDown();
                    card2.FlipDown();
                    OnMismatch?.Invoke();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ClearBoard()
    {
        StopAllCoroutines();
        foreach (Card card in allCards)
        {
            Destroy(card.gameObject);
        }
        allCards.Clear();
        flippedCardsQueue.Clear();
    }

    private List<int> GenerateCardValues(int numPairs)
    {
        List<int> values = new List<int>();
        for (int i = 0; i < numPairs; i++)
        {
            values.Add(i);
            values.Add(i);
        }
        return values;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
