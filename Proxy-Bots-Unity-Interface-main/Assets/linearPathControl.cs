using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linearPathControl : MonoBehaviour
{
    public bool enable = false;

    public float movementSpeed = 0.05f;

    private int stageIndex = 0;

    IEnumerator pathAnimator()
    {
        while (true)
        {
            if (enable)
            {
                stageIndex = 1;

                yield return new WaitForSeconds(11);

                stageIndex = 0;

                yield return new WaitForSeconds(3);

                stageIndex = 2;

                yield return new WaitForSeconds(9);

                stageIndex = 0;

                yield return new WaitForSeconds(3);

                stageIndex = 3;

                yield return new WaitForSeconds(11);

                stageIndex = 0;

                yield return new WaitForSeconds(3);

                stageIndex = 4;

                yield return new WaitForSeconds(9);

                stageIndex = 0;

                yield return new WaitForSeconds(3);
            }
            else
            {
                stageIndex = 0;

                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void Start()
    {
        StartCoroutine(pathAnimator());
    }

    private void Update()
    {
        switch (stageIndex)
        {
            case 1:
                this.transform.position = this.transform.position + new Vector3(0, 0, movementSpeed * Time.deltaTime);
                break;
            case 2:
                this.transform.position = this.transform.position + new Vector3(movementSpeed * Time.deltaTime * (-1), 0, 0);
                break;
            case 3:
                this.transform.position = this.transform.position + new Vector3(0, 0, movementSpeed * Time.deltaTime * (-1));
                break;
            case 4:
                this.transform.position = this.transform.position + new Vector3(movementSpeed * Time.deltaTime, 0, 0);
                break;
            default:
                break;
        }
        
    }
}
