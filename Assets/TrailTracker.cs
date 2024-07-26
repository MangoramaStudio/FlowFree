using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mechanics.RoboticFlows;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrailTracker : MonoBehaviour
{
    public List<Cell> positions = new();
    [SerializeField] private float speed = 0.25f;
    

    [Button]
    public void Follow()
    {
        
        Sequence sequence = DOTween.Sequence();

        foreach (Cell pos in positions)
        {
            sequence.Append(transform.DOMove(pos.transform.position, speed).SetEase(Ease.Linear));
        }

        sequence.Play();
    }
}
