using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public int direction = 1;
    public float maxSpeed = 100.0f;
    public GameObject next = null;
    private float speed = 0;

    private float brakespeed;
    private float accelerationSpeed;

    private Rigidbody rb;
    private MeshCollider meshCollider;

    bool moving = false;

    int changingLane = 0;

    public LayerMask layermask;

    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
        maxSpeed = maxSpeed * Random.Range(0.9f, 1.1f);
        speed = maxSpeed;
        brakespeed = maxSpeed / 50.0f;
        accelerationSpeed = maxSpeed / 100.0f;
    }

    void MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
    }

    void Update()
    {
        next = nextNode();
        if (next == null || Vector3.Distance(next.transform.position, transform.position) < 1)
        {
            Destroy(gameObject);
        } else if (!moving)
        {
            var nextPosition = new Vector3(next.transform.position.x, transform.position.y, next.transform.position.z);
            MoveOverSpeed(gameObject, nextPosition, speed);
        }
    }

    bool checkAtNode(Vector3 node) 
    {
        Vector3 position = node;

        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(5, 5, 5), Quaternion.identity);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Car" && collider.transform.position != transform.position)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void regainSpeed()
    {
        speed = Mathf.Min(maxSpeed, speed + 1);
    }

    void reduceSpeed()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, 0, direction), out hit, 40, layermask))
        {
            Debug.DrawRay(transform.position, new Vector3(0, 0, direction) * hit.distance, Color.yellow);
            speed = hit.transform.gameObject.GetComponent<CarAI>().speed * .95f;
        } else
        {
            Debug.DrawRay(transform.position, new Vector3(0, 0, direction) * 40, Color.white);
        }
        Debug.Log("Reducing speed");
    }
    
    GameObject nextNode()
    {
        var nextNode = getNodeFrontRight();
        if (nextNode && changingLane != -1) 
        {
            var nextPos = nextNode.transform.position;
            var car = checkAtNode(nextPos) || checkAtNode(posOffset(nextPos, 0, 10)) || checkAtNode(posOffset(nextPos, 0, 20)) || checkAtNode(posOffset(nextPos, 0, -10)) || checkAtNode(posOffset(nextPos, 0, -20)) || checkAtNode(posOffset(nextPos, 0, -30));
            if (!car)
            {
                changingLane = 1;
                regainSpeed();
                return nextNode;
            }
        }

        nextNode = getNodeFront();

        if (nextNode != null)
        {
            var nextPos = nextNode.transform.position;
            var car = checkAtNode(nextPos) || checkAtNode(posOffset(nextPos, 0, 10)) || checkAtNode(posOffset(nextPos, 0, 20)) || checkAtNode(posOffset(nextPos, 0, 30)) || checkAtNode(posOffset(nextPos, 0, 40));
            if (!car)
            {
                regainSpeed();
                return nextNode;
            }
            else
            {
                var takeOverNode = getNodeFrontLeft();
                if (takeOverNode != null && changingLane != 1)
                {
                    var takeOverNodePos = takeOverNode.transform.position;
                    var car2 = checkAtNode(takeOverNodePos) || checkAtNode(posOffset(takeOverNodePos, 0, 10)) || checkAtNode(posOffset(takeOverNodePos, 0, 20)) || checkAtNode(posOffset(takeOverNodePos, 0, -10)) || checkAtNode(posOffset(takeOverNodePos, 0, -20)) || checkAtNode(posOffset(takeOverNodePos, 0, -30));
                    if (!car2)
                    {
                        changingLane = -1;
                        regainSpeed();
                        return takeOverNode;
                    }
                }
                changingLane = 0;
                reduceSpeed();
                return nextNode;
            }
        }

        return null;
    }

    Vector3 posOffset(Vector3 pos, int offsetX = 0, int offsetZ = 0)
    {
        return new Vector3(pos.x + offsetX * direction, pos.y, pos.z + offsetZ * direction);
    }

    GameObject getNode(Vector3 pos, int offsetX = 0, int offsetZ = 0)
    {
        Vector3 position = pos + new Vector3(offsetX * direction, 0, offsetZ * direction);
        GameObject node = null;

        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(5, 5, 5), Quaternion.identity, layermask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Node")
                {
                    node = collider.gameObject;
                }
            }
        }
        return node;
    }

    GameObject getNodeFront()
    {
        Vector3 position = transform.position + new Vector3(0, 0, direction * 10);
        GameObject furthest = null;

        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(5, 5, 10), Quaternion.identity, layermask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Node")
                {
                    if (furthest == null)
                    {
                        furthest = collider.gameObject;
                    } else
                    {
                        var nodepos = collider.transform.position;
                        var mypos = transform.position;
                        var furthestpos = furthest.transform.position;
                        if (furthestpos.z * direction < nodepos.z * direction)
                        {
                            if (Mathf.Abs(nodepos.x * direction - mypos.x * direction) < 3)
                            {
                                furthest = collider.gameObject;
                            }
                        }
                    }
                }
            }
        }
        return furthest;
    }

    GameObject getNodeFrontRight()
    {
        Vector3 position = transform.position + new Vector3(10 * direction, 0, direction * 10);
        GameObject furthest = null;

        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(5, 5, 10), Quaternion.identity, layermask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Node")
                {
                    if (furthest == null)
                    {
                        furthest = collider.gameObject;
                    }
                    else
                    {
                        var nodepos = collider.transform.position;
                        var mypos = transform.position;
                        var furthestpos = furthest.transform.position;
                        if (furthestpos.z * direction < nodepos.z * direction)
                        {
                            if (nodepos.x * direction - mypos.x * direction >= 0)
                            {
                                furthest = collider.gameObject;
                            }
                        }
                    }
                }
            }
        }
        return furthest;
    }

    GameObject getNodeFrontLeft()
    {
        Vector3 position = transform.position + new Vector3(-10 * direction, 0, direction * 10);
        GameObject furthest = null;

        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(5, 5, 10), Quaternion.identity, layermask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Node")
                {
                    if (furthest == null)
                    {
                        furthest = collider.gameObject;
                    }
                    else
                    {
                        var nodepos = collider.transform.position;
                        var mypos = transform.position;
                        var furthestpos = furthest.transform.position;
                        if (furthestpos.z * direction < nodepos.z * direction)
                        {
                            if (nodepos.x * direction - mypos.x * direction <= 0)
                            {
                                furthest = collider.gameObject;
                            }
                        }
                    }
                }
            }
        }
        if (furthest != null)
        {
            furthest = checkForBarrier(furthest) ? null : furthest;
        }
        return furthest;
    }

    bool checkForBarrier(GameObject node)
    {
        Collider[] hitColliders = Physics.OverlapBox(node.transform.position, new Vector3(5, 5, 5), Quaternion.identity, layermask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.tag == "Barrier")
                {
                    if (collider.transform.position.x * direction > node.transform.position.x * direction)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
