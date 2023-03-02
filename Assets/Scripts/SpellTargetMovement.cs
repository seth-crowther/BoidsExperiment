using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargetMovement : MonoBehaviour
{
    private LayerMask playerMask;
    public RaycastHit lookHit;

    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.GetMask("Ignore Raycast");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~playerMask)) //Doesn't collide with player, so target can go beneath player
        {
            lookHit = hit;
        }
    }
}
