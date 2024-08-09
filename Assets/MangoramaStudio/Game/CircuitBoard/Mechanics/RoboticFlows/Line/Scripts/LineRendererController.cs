using System;
using System.Linq;
using MangoramaStudio.Game.CircuitBoard.Mechanics.RoboticFlows.Line.Scripts;
using MangoramaStudio.Scripts.Managers;
using Mechanics.RoboticFlows;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private LineRendererConfig lineRendererConfig;
    [SerializeField] private FlowDrawer flowDrawer;
    [SerializeField] private LineRenderer lineRenderer;

    public LineRenderer GetLineRenderer() => lineRenderer;
    
    public bool hasInNode;

    public void Start()
    {
        var mat = Instantiate(lineRendererConfig.Material);
        var id = flowDrawer.Id;
        var definition = lineRendererConfig.LineDefinitions.Find(x => x.id == id);
        mat.mainTexture = definition.texture;
        lineRenderer.material = mat;
        ToggleEvents(true);
    }

    private void OnDestroy()
    {
        ToggleEvents(false); 
    }

    private void OnDisable()
    {
        ToggleEvents(false);
    }

    private void ToggleEvents(bool isToggled)
    {
        if (isToggled)
        {
            
            GameManager.Instance.EventManager.OnRestartLevel += Clear;
        }
        else
        {
            GameManager.Instance.EventManager.OnRestartLevel -= Clear;
        }
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
        hasInNode = false;
    }

    public void RemoveLine()
    {
        lineRenderer.positionCount--;
    }
    
    public void GoToNode(Cell cell)
    {
        var newCell = flowDrawer.DrawnCells.Peek();
        if (hasInNode)
        {
            SetNewPosition(new Vector3(newCell.transform.position.x,newCell.transform.position.z,0));
        }
        else
        {
            lineRenderer.transform.SetParent(cell.node.transform);
            lineRenderer.transform.position = new Vector3(0, 0.1f, 0);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount-1,new Vector3(newCell.transform.position.x,newCell.transform.position.z,0));      
        }
        
        hasInNode = true;
      
    }

    public void SetNewPosition(Vector3 position)
    {
        var newCell = flowDrawer.DrawnCells.Peek();
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount-1,new Vector3(newCell.transform.position.x,newCell.transform.position.z,0)); 
    }
}
