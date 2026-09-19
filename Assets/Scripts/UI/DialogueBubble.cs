using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
        /*pienso hacer el sistema de dialogos,
        como en animal crossing, hay dos tipos de burbuja de dialogo,
        una para seleccionar opciones y una para el mero dialogo.
        Deberia hacer dos clases distintas para ello?
        o crear la clase dialogo donde las burbujas de opciones
        son hijas de las burbujas de mero dialogo???*/
}
