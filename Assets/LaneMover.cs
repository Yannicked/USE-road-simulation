using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneMover : MonoBehaviour
{
    Toggle _toggle;

    public float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        _toggle.onValueChanged.AddListener(delegate {
            switchLanes(_toggle.isOn);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchLanes(bool state)
    {
        if (state)
        {
            moveBarrierLeft();
        } else
        {
            moveBarrierRight();
        }
    }

    void moveBarrierLeft()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        Array.Sort<GameObject>(barriers, new Comparison<GameObject>(
            (i1, i2) => (i2.transform.position.z.CompareTo(i1.transform.position.z))));
        StartCoroutine(moveBarriers(barriers, -10));
    }

    void moveBarrierRight()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        Array.Sort<GameObject>(barriers, new Comparison<GameObject>(
            (i1, i2) => (i1.transform.position.z.CompareTo(i2.transform.position.z))));
        StartCoroutine(moveBarriers(barriers, 10));
    }

    IEnumerator moveBarriers(GameObject[] barriers, float xdir)
    {
        foreach (GameObject barrier in barriers)
        {
            StartCoroutine(moveBarrier(barrier, xdir));
            yield return new WaitForSeconds(0.5f / speed);
        }
    }

    IEnumerator moveBarrier(GameObject barrier, float xdir)
    {
        Vector3 startingPos = barrier.transform.position;
        Vector3 endingPos = barrier.transform.position + new Vector3(xdir, 0, 0);

        while (Vector3.Distance(endingPos, barrier.transform.position) > 0.01f)
        {
            barrier.transform.position = Vector3.MoveTowards(barrier.transform.position, endingPos, 1f * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
