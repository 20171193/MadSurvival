using System.Collections;
using UnityEngine;

public class Turret_Line_Renderer : MonoBehaviour
{
    LineRenderer temp;
    [SerializeField] float Start_Width;
    [SerializeField] float End_Width;
    Coroutine setfalsecoroutine;

    IEnumerator Set_False_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    private void Start()
    {
        temp = gameObject.GetComponent<LineRenderer>();
        temp.startWidth = Start_Width;
        temp.endWidth = End_Width;
        temp.positionCount = 2;
    }
    public void TargetToLine(Vector3 targetPos)
    {
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, targetPos);
    }
    private void OnEnable()
    {
        setfalsecoroutine = StartCoroutine(Set_False_Coroutine());

    }


}
