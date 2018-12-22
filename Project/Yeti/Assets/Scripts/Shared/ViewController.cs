using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartCoroutine(WaitForLateStart());
    }

    private IEnumerator WaitForLateStart()
    {
        yield return new WaitForSeconds(0.1f);
    }   

    protected virtual void LateStart()
    {

    } 
}
