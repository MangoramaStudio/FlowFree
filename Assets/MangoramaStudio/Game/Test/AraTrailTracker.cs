using System.Collections.Generic;
using System.Linq;
using Ara;
using DG.Tweening;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MangoramaStudio.Game.Test
{
    public class AraTrailTracker : MonoBehaviour
    {
        [SerializeField] private TrailType trailType;
    [SerializeField] private Node node;
    [SerializeField] private float speed = 0.25f;
    [SerializeField] private AraTrail araTrail;
    public List<Cell> positions = new();

    private float _initialTime;
    private Sequence _flowSequence;
    private Material _instantiatedMaterial;

    private void Start()
    {
        ToggleEvents(true);
        var mat = araTrail.materials[0];
        _instantiatedMaterial = Instantiate(mat);
        araTrail.materials[0] = _instantiatedMaterial;
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
            araTrail.Clear();
            araTrail.gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
            araTrail.enabled = false;
            _instantiatedMaterial.mainTextureOffset = Vector2.zero;

        }
    }

    private void ResetTrail()
    {
        araTrail.gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
        araTrail.Clear();
        _instantiatedMaterial.mainTextureOffset = Vector2.zero;
        araTrail.enabled = true;
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
        _flowSequence.AppendCallback(() =>
        {
            if (trailType == TrailType.Reflection)
            {
                ModifyForReflection();
            }
        });
    }

    private void KillSequence()
    {
        _flowSequence?.Kill();   
    }

    private void ModifyForReflection()
    {
        DOVirtual.Float(0f, 1f, .2f, (x) =>
        {
            _instantiatedMaterial.mainTextureOffset = new Vector2(x, 0);
        });
    }
    }
}