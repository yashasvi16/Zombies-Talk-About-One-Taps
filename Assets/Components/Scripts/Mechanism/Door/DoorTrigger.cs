using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Collider _col;
    bool firstTimeOpen = true;
    private void OnTriggerEnter(Collider other)
    {
        if(firstTimeOpen)
        {
            GetComponent<Animator>().SetTrigger("Open");
            _col.enabled = false;
            firstTimeOpen = false;
        }
    }

    public void GameStartCloseDoor()
    {
        GetComponent<Animator>().SetTrigger("Close");
        _col.enabled = true;
    }
}
