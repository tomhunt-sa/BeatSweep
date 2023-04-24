using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomChat : MonoBehaviour
{
    [SerializeField] private List<string> chatLines = new();
    [SerializeField] private List<string> fallOverLines = new();

    [SerializeField] private float chanceOfChat = 0.5f;
    
    [SerializeField] private TMP_Text label;
    
    [SerializeField] private int beatsToShow = 2;
    [SerializeField] private int beatsToShowRemaining = 0;
    
    private MS_Character character;
    
    void OnEnable()
    {
        character = GetComponent<MS_Character>();

        if (character)
        {
            BeatEffectManager.onBeat += OnBeat;
            GameState.OnTakeDamage += OnTakeDamage;
        }

        if (label)
            label.text = "";
    }
    
    void OnDisable()
    {
        BeatEffectManager.onBeat -= OnBeat;
        GameState.OnTakeDamage -= OnTakeDamage;
    }

    private void OnBeat()
    {
        if (beatsToShowRemaining > 0)
        {
            beatsToShowRemaining--;
            if (beatsToShowRemaining == 0)
            {
                if (label)
                    label.text = "";
            }
            return;
        }
     
        if (character.State != CharacterState.Walk)
            return;
        
        if (chanceOfChat < Random.value)
            return;
        
        StartNewChat(chatLines);
    }
    
    private void OnTakeDamage(bool fromMine)
    {
        if (fromMine)
        {
            StartNewChat(fallOverLines);
        }
    }

    private void StartNewChat(List<string> lines)
    {
        if (lines.Count == 0)
            return;

        beatsToShowRemaining = beatsToShow;
        
        if (label)
            label.text = lines[Random.Range(0, lines.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
