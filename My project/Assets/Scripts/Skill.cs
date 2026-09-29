using System;
using UnityEngine;

[Serializable]
public class Skill
{
    public string nombre;
    public int costo;
    public string triggerAnimacion;

    
    private bool desbloqueada;

   
    public Skill(string nombre, int costo, string triggerAnimacion)
    {
        this.nombre = nombre;
        this.costo = costo;
        this.triggerAnimacion = triggerAnimacion;
        this.desbloqueada = false;
    }

    public bool EstaDesbloqueada()
    {
        return desbloqueada;
    }

    public void Desbloquear()
    {
        desbloqueada = true;
    }
}