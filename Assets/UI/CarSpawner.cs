using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CarSpawner : MonoBehaviour
{
    double t = 0;

    public List<Transform> cars;

    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        for (; ;)
        {
            for (int i = 0; i < 4; i++)
            {
                if (UnityEngine.Random.Range(0.0f, 4.0f) > 3.2f)
                {
                    spawnInLane(i);
                }
            }
            for (int i = 4; i < 7; i++)
            {
                if (UnityEngine.Random.Range(0.0f, 4.0f) > 3.2f)
                {
                    spawnInLane(i);
                }
            }
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void spawnInLane(int lanenr)
    {
        int direction = getLaneDirection(lanenr);
        if (direction == 1)
        {
            var car = getLaneCar(lanenr);
            Vector3 lanePos = new Vector3(-30 + lanenr * 10, 0, 0);
            spawnCar(lanePos, direction, car);
        } else if (direction == -1)
        {
            var car = getLaneCar(lanenr);
            Vector3 lanePos = new Vector3(-30 + lanenr * 10, 0, 5000);
            spawnCar(lanePos, direction, car);
        } else if (direction == 0)
        {
            /*Vector3 lanePos = new Vector3(-30 + lanenr * 10, 0, 0);
            Vector3 lanePos2 = new Vector3(-30 + lanenr * 10, 0, 1000);
            spawnCar(lanePos, 1);
            spawnCar(lanePos2, -1);*/
        }
    }

    void spawnCar(Vector3 pos, int direction, Transform car)
    {
        if (checkSpawnPoint(pos))
        {
            var instance = Instantiate(car) as Transform;
            instance.position = pos + new Vector3(0, 0.65f, 0);
            instance.GetComponent<CarAI>().direction = direction;
            instance.rotation = Quaternion.Euler(0, 90 * direction, 0);
        }
    }

    Transform getLaneCar(int lanenr)
    {
        var r = UnityEngine.Random.value;
        switch (lanenr)
        {
            case 0:
                if (r < 0.6)
                {
                    return cars[2];
                } else if (r < 0.9)
                {
                    return cars[1];
                }
                break;
            case 1:
                if (r < 0.2)
                {
                    return cars[2];
                }
                else if (r < 0.6)
                {
                    return cars[1];
                }
                break;
            case 2:
                if (r < 0.35)
                {
                    return cars[1];
                }
                break;
            case 6:
                if (r < 0.6)
                {
                    return cars[2];
                }
                else if (r < 0.9)
                {
                    return cars[1];
                }
                break;
            case 5:
                if (r < 0.2)
                {
                    return cars[2];
                }
                else if (r < 0.6)
                {
                    return cars[1];
                }
                break;
            case 4:
                if (r < 0.35)
                {
                    return cars[1];
                }
                break;
        }
        return cars[0];
    }

    int getLaneDirection(int lanenr)
    {
        switch (lanenr)
        {
            case 0:
                return -1;
            case 1:
                return -1;
            case 2:
                return -1;
            case 3:
                return 0;
            case 4:
                return 1;
            case 5:
                return 1;
            case 6:
                return 1;
        }
        return 0;
    }

    bool checkSpawnPoint(Vector3 pos)
    {
        return true;
    }
}