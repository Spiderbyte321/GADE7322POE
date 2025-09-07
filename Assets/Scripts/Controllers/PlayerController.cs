using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RaycastHit Hit = new RaycastHit();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            HandleMouseClick();
        }
    }

    private void HandleMouseClick()
    {
        Iinteractable ClickedObject;
        if(Hit.collider.TryGetComponent<Iinteractable>(out ClickedObject)) 
            ClickedObject.Interact();
    }
}
