using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloatRingBuffer
{
    float Dequeue();
    void Enqueue(float item);
    float Peek();
}
