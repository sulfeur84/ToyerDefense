using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBase : MonoBehaviour
{
    private GameObject towerPreview = null;
    
    public void OnMouseEnter()
    {
        Debug.Log("dddddddddd");
        if(GameManager.TowerTypeSelected == null) return;

        towerPreview = Instantiate(GameManager.TowerTypeSelected.GetTowerPreviewGo(), transform.position, transform.rotation);
    }

    public void OnMouseExit()
    {
        Debug.Log("fffffffffffffffff");
        if(towerPreview != null)
        {
            Destroy(towerPreview);
            towerPreview = null;
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("hhhhhhhhhhhhhhhh");
        if(GameManager.TowerTypeSelected == null) return;
        
        Destroy(towerPreview);

        GameManager.TowerInstantiateA(transform);
        
        Destroy(this);
    }
}
