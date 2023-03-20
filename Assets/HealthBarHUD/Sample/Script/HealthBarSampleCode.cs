using Hsinpa.Healthbar.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.Healthbar.Sample
{
    public class HealthBarSampleCode : MonoBehaviour
    {
        [SerializeField]
        private HealthBarView healthBarView;

        [SerializeField]
        private Button executeBtn;

        [SerializeField]
        private InputField damageInputField;

        [SerializeField]
        private Text hpText;

        private void Start()
        {
            string target_id = "fake_object_01";
         
            healthBarView.SetUp(target_id, 100, 100, blockNum: 5);
            UpdateHPText();

            executeBtn.onClick.AddListener(OnExectueBtnClick);
        }

        private void OnExectueBtnClick() {
            string damageString = damageInputField.text;

            if (int.TryParse(damageString, out int damageInt)) {

                if (damageInt > 0) damageInt = damageInt * -1;

                int newHP = healthBarView.UpdateHPValue(damageInt);

                UpdateHPText();
            }

            damageInputField.text = "";
        }

        private void UpdateHPText() {
            hpText.text = "HP : " + healthBarView.CurrentHP;
        }




    }
}
