using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mechanics.RoboticFlows;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrailTracker : MonoBehaviour
{
    public List<Cell> positions = new();
    public float speed = 0.25f;
    

    [Button]
    public void Follow()
    {
        transform.localPosition = positions[0].transform.position;
        
        Sequence sequence = DOTween.Sequence();

        foreach (Cell pos in positions)
        {
            sequence.Append(transform.DOLocalMove(pos.transform.position, speed).SetEase(Ease.Linear));
        }

        sequence.Play();
    }
}
