using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField] private GameObject spine;
    private float spineSightRadius = 5f;
    private float spineAvoidRadius = 0.5f;
    private float spineSpeed = 10f;
    private float spineTurningCircle = 100f;

    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;
    [SerializeField] private float avoidanceWeight;
    [SerializeField] private float targetWeight;

    private static int numSpines = 16;
    private SpellTargetMovement spellTargetScript;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spellTargetScript = GetComponent<SpellTargetMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < numSpines; i++)
            {
                var g = Instantiate(spine, player.transform.position, Quaternion.LookRotation(Vector3.up));
                g.GetComponent<myDest>().myTarget = spellTargetScript.lookHit.point;
            }
        }
        UpdateAllSpines();
    }

    void UpdateAllSpines()
    {
        GameObject[] allSpines = GameObject.FindGameObjectsWithTag("Spine");

        foreach (GameObject spine in allSpines)
        {
            Collider[] nearColliders = Physics.OverlapSphere(spine.transform.position, spineSightRadius);
            GameObject[] nearSpines = new GameObject[nearColliders.Length];
            for (int i = 0; i < nearColliders.Length; i++)
            {
                nearSpines[i] = nearColliders[i].gameObject;
            }

            Vector3 cohesion = DoCohesion(spine, nearSpines);
            Vector3 alignment = DoAlignment(spine, nearSpines);
            Vector3 avoidance = DoAvoidance(spine);
            Vector3 target = DoTarget(spine, spine.GetComponent<myDest>().myTarget);

            Vector3 newDirection = ((cohesion * cohesionWeight) + (alignment * alignmentWeight) + (avoidance * avoidanceWeight) + (target * targetWeight)) / 4;
            newDirection.Normalize();

            Quaternion rotationAmount = Quaternion.LookRotation(newDirection);

            float rotationStep = spineTurningCircle * Time.deltaTime;

            spine.transform.rotation = Quaternion.RotateTowards(spine.transform.rotation, rotationAmount, rotationStep);
            //Rigidbody r = spine.GetComponent<Rigidbody>();
            //r.MovePosition(spine.transform.position + (spine.transform.forward * spineSpeed * Time.deltaTime));
            spine.transform.Translate(spine.transform.forward * spineSpeed * Time.deltaTime, Space.World);
        }
    }

    //Pulls each spine towards nearby spines
    Vector3 DoCohesion(GameObject spine, GameObject[] nearSpines)
    {
        //If there are no other spines nearby, cohesion shouldn't affect the current spine at all
        if (nearSpines.Length == 1)
        {
            return Vector3.zero;
        }

        Vector3 total = Vector3.zero;
        foreach (GameObject s in nearSpines)
        {
            if (s.transform.root != spine.transform) //Omit the current spine
            {
                total += s.transform.position;
            }
        }

        //Move spine to average position of nearby spines
        Vector3 averagePos = total / nearSpines.Length;
        Vector3 moveDir = averagePos - spine.transform.position;
        moveDir.Normalize();

        return moveDir;
    }

    //Aligns each spine with its neighbouring spines
    Vector3 DoAlignment(GameObject spine, GameObject[] nearSpines)
    {
        //If there are no other spines nearby, alignment shouldn't affect the current spine at all
        if (nearSpines.Length == 1)
        {
            return Vector3.zero;
        }

        Vector3 total = Vector3.zero;

        foreach(GameObject s in nearSpines)
        {
            if (s.transform.root != spine.transform) //Omit the current spine
            {
                total += s.transform.forward;
            }
        }

        //Return average direction of all nearby spines
        Vector3 averageDir = total / nearSpines.Length;
        averageDir.Normalize(); //This might not be needed but might as well
        return averageDir;
    }

    //Moves spines away from spines that are too close
    Vector3 DoAvoidance(GameObject spine)
    {
        Collider[] tooCloseColliders = Physics.OverlapSphere(spine.transform.position, spineAvoidRadius);
        List<GameObject> tooCloseSpines = new List<GameObject>();
        for (int i = 0; i < tooCloseColliders.Length; i++)
        {
            if (tooCloseColliders[i].gameObject.tag == "Spine")
            {
                tooCloseSpines.Add(tooCloseColliders[i].gameObject);
            }
        }

        Vector3 total = Vector3.zero;
        foreach(GameObject s in tooCloseSpines)
        {
            total += s.transform.position;
        }

        Vector3 averagePos = total / tooCloseSpines.Count;
        Vector3 direction = spine.transform.position - averagePos; //Doesn't work if they're at the exact same position...
        direction.Normalize();

        return direction;
    }

    //Moves spines towards a common target
    Vector3 DoTarget(GameObject spine, Vector3 targetPos)
    {
        Vector3 direction = targetPos - spine.transform.position;
        direction.Normalize();
        return direction;
    }
}
