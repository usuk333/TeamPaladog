using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pride : MonoBehaviour
{
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    public bool isIn = false;
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }

    
}
