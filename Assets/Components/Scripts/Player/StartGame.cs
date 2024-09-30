using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] Transform _raycastPostion;
    [SerializeField] LayerMask _crystalLayer;
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_raycastPostion.position, transform.forward, out hit, 1f, _crystalLayer))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                GameManager.Instance.StartGame();
                UIManager.Instance.HideCommand();
                UIManager.Instance.ShowInstruction();
            }
            else
            {
                UIManager.Instance.ShowCommand();
            }
        }
        else
        {
            UIManager.Instance.HideCommand();
        }
    }
}
