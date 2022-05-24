using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHealth : MonoBehaviour
{
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;

    public SpriteRenderer HealthSpriteRenderer1;
    public SpriteRenderer HealthSpriteRenderer2;
    public SpriteRenderer HealthSpriteRenderer3;
    public SpriteRenderer HealthSpriteRenderer4;
    public SpriteRenderer HealthSpriteRenderer5;

    // Start is called before the first frame update
    void Start()
    {
        GameController.controller.onUpdateHealth += UpdateHealth;
    }

    // Update is called once per frame
    void UpdateHealth(int Health)
    {
        print(Health);

        
        if (HealthSpriteRenderer1 != null)
        {
            if (Health <= 19)
            {
                HealthSpriteRenderer1.sprite = num3;
                HealthSpriteRenderer2.sprite = num3;
                HealthSpriteRenderer3.sprite = num3;
                HealthSpriteRenderer4.sprite = num3;
                HealthSpriteRenderer5.sprite = num3;
            }
            else if (Health <=39)
            {
                HealthSpriteRenderer1.sprite = num3;
                HealthSpriteRenderer2.sprite = num3;
                HealthSpriteRenderer3.sprite = num3;
                HealthSpriteRenderer4.sprite = num3;
                HealthSpriteRenderer5.sprite = num1;
            }
            else if (Health <=59)
            {
                HealthSpriteRenderer1.sprite = num3;
                HealthSpriteRenderer2.sprite = num3;
                HealthSpriteRenderer3.sprite = num3;
                HealthSpriteRenderer4.sprite = num1;
                HealthSpriteRenderer5.sprite = num1;
            }
            else if (Health <=79)
            {
                HealthSpriteRenderer1.sprite = num3;
                HealthSpriteRenderer2.sprite = num3;
                HealthSpriteRenderer3.sprite = num1;
                HealthSpriteRenderer4.sprite = num1;
                HealthSpriteRenderer5.sprite = num1;
            }
            else if(Health <=99)
            {
                HealthSpriteRenderer1.sprite = num3;
                HealthSpriteRenderer2.sprite = num1;
                HealthSpriteRenderer3.sprite = num1;
                HealthSpriteRenderer4.sprite = num1;
                HealthSpriteRenderer5.sprite = num1;
            }
            else if (Health == 100)
            {
                HealthSpriteRenderer1.sprite = num1;
                HealthSpriteRenderer2.sprite = num1;
                HealthSpriteRenderer3.sprite = num1;
                HealthSpriteRenderer4.sprite = num1;
                HealthSpriteRenderer5.sprite = num1;
            }
          
        }
    }

}