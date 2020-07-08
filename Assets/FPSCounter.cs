using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public int  FrameRange = 60;
    public Text Text       = null;

    private int   _fpsAverage  = 0;
    private int[] _fpsBuffer   = null;
    private int   _bufferIndex = 0;

    private void Awake()
    {
        if (Text == null)
            this.enabled = false;

        InitBuffer();
    }

    private void Update()
    {
        if (_fpsBuffer.Length != FrameRange)
            InitBuffer();

        UpdateBuffer();
        CalcFPS();

        Text.text = $" FPS: {_fpsAverage.ToString()}";
        
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void InitBuffer()
    {
        if (FrameRange < 1)
            FrameRange = 1;

        _fpsBuffer   = new int[FrameRange];
        _bufferIndex = 0;
    }

    private void UpdateBuffer()
    {
        _fpsBuffer[_bufferIndex++] =  Mathf.FloorToInt(1 / Time.unscaledDeltaTime);
        _bufferIndex               %= FrameRange;
    }

    private void CalcFPS()
    {
        int sum = 0;
        foreach (int value in _fpsBuffer)
            sum += value;
        _fpsAverage = sum / FrameRange;
    }
}