using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmoothingRingBuffer : IFloatRingBuffer
{
    private int inputIndex;
    private int outputIndex;
    private int bufferSize;
    private float[] bufferContent;
    private int amountOfItemsInBuffer;

    public SmoothingRingBuffer(int bufferSize)
    {
        if (bufferSize <= 0)
        {
            this.bufferSize = 5;
        }
        else
        {
            this.bufferSize = bufferSize;
        }
        bufferContent = new float[this.bufferSize];
    }
    public SmoothingRingBuffer()
    {
        this.bufferSize = 5;
        bufferContent = new float[bufferSize];
    }

    // enqueue new value and get average of all in return
    public float Smooth(float newValue)
    {
        Enqueue(newValue);
        return GetSmoothValue();
    }

    public float GetSmoothValue()
    {
        //calculate average of all values
        float sum = 0;
        for (int index = 0; index < bufferSize - 1; index++)
        {
            sum += bufferContent[index];
        }
        return sum / bufferSize;
    }

    public void Enqueue(float item)
    {
        if (amountOfItemsInBuffer == bufferSize)
        {
            outputIndex = (outputIndex + 1) % bufferSize;
            bufferContent[inputIndex] = item;
            inputIndex = (inputIndex + 1) % bufferSize;
        }
        else
        {
            bufferContent[inputIndex] = item;
            inputIndex = (inputIndex + 1) % bufferSize;
            amountOfItemsInBuffer++;
        }
    }

    public float Dequeue()
    {
        if (amountOfItemsInBuffer == 0)
        {
            return 0;
        }
        float dequeuedItem = bufferContent[outputIndex];
        outputIndex = (outputIndex + 1) % bufferSize;
        amountOfItemsInBuffer--;
        return dequeuedItem;
    }

    public float Peek()
    {
        if (amountOfItemsInBuffer == 0)
        {
            return 0;
        }
        return bufferContent[outputIndex];
    }
}
