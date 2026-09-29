using UnityEngine;

public class Possessable : MonoBehaviour
{
    private EnemigoMelee_IA iaCerebro;
    private EnemigoBase enemigoFisico;

    void Awake()
    {
        iaCerebro = GetComponent<EnemigoMelee_IA>();
        enemigoFisico = GetComponent<EnemigoBase>();
    }

    public void SerPoseido()
    {
        if (iaCerebro != null) iaCerebro.iaActiva = false;
    }

    public void RestaurarIA()
    {
        if (iaCerebro != null) iaCerebro.iaActiva = true;
        if (enemigoFisico != null) enemigoFisico.SetInput(Vector2.zero);
    }

    public void MoverEnemigo(Vector2 input, bool corriendo)
    {
        if (enemigoFisico != null)
        {
            enemigoFisico.SetInput(input);
            enemigoFisico.Move(corriendo);
        }
    }
}