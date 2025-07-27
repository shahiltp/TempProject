using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;
    [SerializeField] private AudioClip gameOverSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Card.OnCardClicked += HandleCardFlip; 
        GameBoard.OnMatchFound += HandleMatchFound;
        GameBoard.OnMismatch += HandleMismatch;
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        Card.OnCardClicked -= HandleCardFlip;
        GameBoard.OnMatchFound -= HandleMatchFound;
        GameBoard.OnMismatch -= HandleMismatch;
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleCardFlip(Card card)
    {
        
        if (!card.isFlipped && !card.isMatched)
        {
            PlaySound(flipSound);
        }
    }

    private void HandleMatchFound(int points) => PlaySound(matchSound);
    private void HandleMismatch() => PlaySound(mismatchSound);

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            PlaySound(gameOverSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
