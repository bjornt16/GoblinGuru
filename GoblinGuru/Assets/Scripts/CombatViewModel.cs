using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatViewModel : MonoBehaviour {

    public UnityEngine.UI.RawImage head;
    public UnityEngine.UI.RawImage torso;
    public UnityEngine.UI.RawImage legs;
    public UnityEngine.UI.RawImage feet;

    public void Init(Color headColor, Color torsoColor, Color legsColor, Color feetColor)
    {
        head.color = headColor;
        torso.color = torsoColor;
        legs.color = legsColor;
        feet.color = feetColor;
    }
}
