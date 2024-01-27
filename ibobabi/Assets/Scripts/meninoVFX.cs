using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meninoVFX : MonoBehaviour
{
    public SpriteRenderer rend;

    public void FlashHit(Color color, float duration)
    {
        DOVirtual.Color(color, Color.black, duration, (value) =>
        {
            rend.material.SetColor("_FlashColor", value);
        });
    }
}
