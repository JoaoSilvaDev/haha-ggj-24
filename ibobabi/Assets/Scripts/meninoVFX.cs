using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public void ScaleHit(float amount, bool increaseX, bool increaseY, float duration, bool reset = true)
    {
        if(reset)
            transform.localScale = Vector3.one;

        Vector3 newScale = 
            new Vector3(
                transform.localScale.x + (increaseX ? amount : -amount),
                transform.localScale.y + (increaseY ? amount : -amount),
                1f);

        DOVirtual.Vector3(newScale, transform.localScale, duration, (value) =>
        {
            transform.localScale = value;
        });
    }
}
