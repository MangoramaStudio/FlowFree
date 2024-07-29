using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrailTracker : MonoBehaviour
{
    [SerializeField] private Node node;
    [SerializeField] private float speed = 0.25f;
    [SerializeField] private TrailRenderer trailRenderer;
    public List<Cell> positions = new();

    private float _initialTime;
    private Sequence _flowSequence;

    private void Start()
    {
        _initialTime = trailRenderer.time;
        ToggleEvents(true);
        
    }


    private void OnDisable()
    {
        ToggleEvents(false);
    }

    private void OnDestroy()
    {
       ToggleEvents(false);
    }

    private void ToggleEvents(bool isToggled)
    {
        var eventManager = GameManager.Instance.EventManager;
        if (isToggled)
        {
            eventManager.OnCompleteFlow += GetCellsList;
            eventManager.OnClearCell += ClearTrail;
            eventManager.OnClearDisconnectedCell += ClearTrail;
            eventManager.OnResetFlow += ClearTrail;

        }
        else
        {
            eventManager.OnCompleteFlow -= GetCellsList;
            eventManager.OnClearCell -= ClearTrail;
            eventManager.OnClearDisconnectedCell -= ClearTrail;
            eventManager.OnResetFlow -= ClearTrail;

        }
    }

    private void ClearTrail(FlowDrawer flowDrawer)
    {
        if (IsMatchedWithDrawer(flowDrawer))
        {
            KillSequence();
            trailRenderer.Clear();
            trailRenderer.gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
            trailRenderer.enabled = false;
        }
    }

    private void ResetTrail()
    {
       
        trailRenderer.gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
        trailRenderer.Clear();
        trailRenderer.enabled = true;
    }
    
    private void GetCellsList(FlowDrawer flowDrawer,Node selectedNode)
    {
        if (IsMatchedWithDrawer(flowDrawer))
        {
            ResetTrail();
            positions.Clear();
            positions = flowDrawer.DrawnCells.ToList();
            if (node == selectedNode)
            {
                var cell = node.GetComponentInParent<Cell>();
                var cellPosition = new Vector2Int(cell.x, cell.y);
                var firstCellPosition = new Vector2Int(positions[0].x, positions[0].y);
                if (cellPosition != firstCellPosition)
                {
                    positions.Reverse();
                }
                Follow();     
            }
        }
    }

    private bool IsMatchedWithDrawer(FlowDrawer flowDrawer)
    {
        return node.Id == flowDrawer.Id;
    }

    [Button]
    public void Follow()
    {
       
         _flowSequence = DOTween.Sequence();
        foreach (Cell pos in positions)
        {
            var newPos = new Vector3(pos.transform.position.x, 1.5f, pos.transform.position.z);
            _flowSequence.Append(transform.DOMove(newPos, speed).SetEase(Ease.Linear));
        }
        _flowSequence.Play();
    }

    private void KillSequence()
    {
        _flowSequence?.Kill();   
    }
}
