using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBase : MonoBehaviour
{
    private GameObject towerPreview = null;
    
    public void OnMouseEnter()
    {
        if(GameManager.TowerTypeSelected == null) return;

        towerPreview = Instantiate(GameManager.TowerTypeSelected.GetTowerPreviewGo(), transform.GetChild(0).position, transform.rotation);
    }

    public void OnMouseExit()
    {
        if(towerPreview != null)
        {
            Destroy(towerPreview);
            towerPreview = null;
        }
    }

    public void OnMouseDown()
    {
        if(GameManager.TowerTypeSelected == null) return;
        
        Destroy(towerPreview);

        GameManager.TowerInstantiateA(transform.GetChild(0));
        
        Destroy(this);
    }
}
