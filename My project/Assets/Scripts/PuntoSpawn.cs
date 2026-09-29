using System;
using UnityEngine;

[Serializable]
public class PuntoSpawn
{
    public Transform pivote;
    public GameObject enemigoPrefab;

    // Constructor
    public PuntoSpawn(Transform pivote, GameObject enemigoPrefab)
    {
        this.pivote = pivote;
        this.enemigoPrefab = enemigoPrefab;
    }
}