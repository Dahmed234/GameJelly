using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private int score = 0;
    private int fishCaught = 0;
    private TextMeshProUGUI scoreText;
    private AudioSource audioSource;



    public void OnFishCaught(object Sender, EventArgs e)
    {
        fishCaught++;
    }
    void Start()
    {
       scoreText = GetComponent<TextMeshProUGUI>(); 
       audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fishCaught != 0)
        {
            audioSource.Play();
            score += (int) Math.Pow(fishCaught, 1.6);
            fishCaught = 0;
        }
        scoreText.text = score.ToString();
    }
}
