using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneDrawer : MonoBehaviour
{
    private List<Transform> _roadInstances = new List<Transform>();
    private List<Transform> _barrierInstances = new List<Transform>();

    public int roadLength = 10000;

    public float roadWidth = 7;

    public Transform roadPrefab;
    public Transform barrierPrefab;

    private float laneWidth;
    private float laneLength;

    private Slider slider;

    public int middledir = 1;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        Vector3 laneSize = roadPrefab.localScale;
        laneWidth = laneSize.x;
        laneLength = laneSize.z;
        roadWidth = laneWidth * 2f;
        drawLanes();
        drawBarrier();
    }
    
    public void updateLanes()
    {
        roadWidth = slider.value * laneWidth;
        clearLanes();
        drawLanes();
        clearBarrier();
        drawBarrier();
    }

    void clearLanes()
    {
        for (int i = 0; i < _roadInstances.Count; i++)
        {
            Destroy(_roadInstances[i].gameObject);
        }
        _roadInstances.Clear();
    }

    void clearBarrier()
    {
        for (int i = 0; i < _barrierInstances.Count; i++)
        {
            Destroy(_barrierInstances[i].gameObject);
        }
        _barrierInstances.Clear();
    }

    void drawLanes()
    {
        Debug.Log(laneWidth);
        for (int i = 0; i < roadLength / 10f; i++)
        {
            var instance = Instantiate(roadPrefab) as Transform;
            _roadInstances.Add(instance);
            instance.position = new Vector3(0, 0, i * 10f);
            instance.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < roadLength / 10f - 1; i++)
        {
            var instance = _roadInstances[i];
            var nextinstance = _roadInstances[i + 1];
        }
    }

    void drawBarrier()
    {
        for (int i = 0; i < roadLength / 5f; i++)
        {
            var instance = Instantiate(barrierPrefab) as Transform;
            _barrierInstances.Add(instance);

            float middle = middledir * 5f;

            instance.position = new Vector3(middle, 0, i * 5f);
        }
    }
}
