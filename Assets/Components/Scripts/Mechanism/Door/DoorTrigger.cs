using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    bool firstTimeOpen = true;
    private void OnTriggerEnter(Collider other)
    {
        if(firstTimeOpen)
        {
            GetComponent<Animator>().SetTrigger("Open");
            firstTimeOpen = false;
        }
    }

    public void GameStartCloseDoor()
    {
        GetComponent<Animator>().SetTrigger("Close");
    }
}
