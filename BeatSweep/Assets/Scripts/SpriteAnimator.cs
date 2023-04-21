using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public SpriteAnimationClip Clip;
    public float frameRate = 10.0f;

    private SpriteRenderer spriteRenderer;

    private float timer;
    private int currentFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (!spriteRenderer || !Clip || Clip.Sprites.Length == 0)
            return;

        timer += Time.deltaTime;

        var frameTick = 1.0f / frameRate;

        while (timer > frameTick)
        {
            timer -= frameTick;
            currentFrame = (currentFrame + 1) % Clip.Sprites.Length;
        }

        spriteRenderer.sprite = Clip.Sprites[currentFrame];
    }
}
