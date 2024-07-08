using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        private const string STEP = "_Steps";
        private const string RATIO = "_HSRatio";
        private const string WIDTH = "_Width";
        private const string THICKNESS = "_Thickness";

        private static readonly int floatSteps = Shader.PropertyToID(STEP);
        private static readonly int floatRatio = Shader.PropertyToID(RATIO);
        private static readonly int floatWidth = Shader.PropertyToID(WIDTH);
        private static readonly int floatThickness = Shader.PropertyToID(THICKNESS);

        public float Hp = 100f;
        public float MaxHp = 100f;
        public float Sp = 0f;
        public float speed = 3f;

        public float hpShieldRatio;
        public float RectWidth = 100f;
        [Range(0, 5f)] public float Thickness = 2f;
        public float hpPerUnit = 10f;

        public Image hp;
        public Image damaged;
        public Image sp;
        public Image separator;

        [ContextMenu("Create Material")]
        private void CreateMaterial()
        {
            // if (separator.material == null)
            {
                separator.material = new Material(Shader.Find("ABS/UI/Health Separator"));
            }
        }

        private void Start()
        {
            Hp = 150;
            MaxHp = 150;

            //yield return new WaitForSeconds(2.0f);


            //Sp = 40;

            //while (Sp > 0)
            //{
            //    Sp -= 28 * Time.deltaTime;
            //    yield return null;
            //}

            //Sp = 0;

            //yield return new WaitForSeconds(2f);

            //for (int i = 0; i < 8; i++)
            //{
            //    Hp -= 12;
            //    yield return new WaitForSeconds(1f);
            //}

            //for (int i = 0; i < 8; i++)
            //{
            //    MaxHp += 20;
            //    Hp = MaxHp;

            //    yield return new WaitForSeconds(1f);
            //}
         }


        private void Update()
        {
            if (MaxHp < Hp)
            {
                MaxHp = Hp;
            }

            float step;

            // 쉴드가 존재 할 때
            if (Sp > 0)
            {
                // 현재체력 + 쉴드 > 최대 체력
                if (Hp + Sp > MaxHp)
                {
                    hpShieldRatio = Hp / (Hp + Sp);
                    sp.fillAmount = 1f;
                    step = (Hp) / hpPerUnit;
                    hp.fillAmount = Hp / (Hp + Sp);
                }
                else
                {
                    sp.fillAmount = (Hp + Sp) / MaxHp;
                    hpShieldRatio = Hp / MaxHp;
                    step = Hp / hpPerUnit;

                    hp.fillAmount = Hp / MaxHp;
                }
            }
            else
            {
                sp.fillAmount = 0f;
                step = MaxHp / hpPerUnit;
                hpShieldRatio = 1f;

                hp.fillAmount = Hp / MaxHp;
            }

            // sp.fillAmount = 1 - hpShieldRatio;

            damaged.fillAmount = Mathf.Lerp(damaged.fillAmount, hp.fillAmount, Time.deltaTime * speed);

            separator.material.SetFloat(floatSteps, step);
            separator.material.SetFloat(floatRatio, hpShieldRatio);
            separator.material.SetFloat(floatWidth, RectWidth);
            separator.material.SetFloat(floatThickness, Thickness);
        }

        public void UpdateHp(float HP, float MaxHP, float SP)
        {
            Hp = HP;
            MaxHp = MaxHP;
            Sp = SP;    
        }
    }