using UnityEngine;

public class SpecialAction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Action()
    {
        this.GetComponent<FollowerStats>().isRecruited = true;
    }
}
