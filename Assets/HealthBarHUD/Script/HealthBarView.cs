using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.SRP;

namespace Hsinpa.Healthbar.View{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private Image healthbarImage;

        [SerializeField]
        private Transform healthBlockHolder;

        [SerializeField]
        private GameObject healthBlockPrefab;

        [SerializeField]
        private Material healthBarMaterial;

        [SerializeField]
        private HealthbarSRP healthBarSRP;

        private List<Image> blocks = new List<Image>();
        private int _totalBlocks;
        private int _totalHealth;
        private int _currentBlockIndex;

        public int CurrentHP => Mathf.RoundToInt(_currentHP);
        private float _currentHP;
        private float _currentResidual;
        private float _threshold = 0.01f;

        private float blockHP;
        private string _objectID = "";

        private Color mainBarColor {
            get {
                return healthBarSRP.barColor[_currentBlockIndex];
            }
        }

        public System.Action<int> OnHealthBlockChange;
        public System.Action OnHealthEmptyOrNegative;

        #region Public API
        public bool CheckIfTargetIsSet(string objectID)
        {
            return _objectID == objectID;
        }

        public void Show(bool isShow)
        {
            this.gameObject.SetActive(isShow);
        }

        public void SetUp(string objectID, int currentHealth, int totalHealth, int blockNum)
        {
            Show(true);

            //Filter block count;
            _totalBlocks = Mathf.Clamp(blockNum, 0, healthBarSRP.barCount);

            _totalHealth = totalHealth;
            _currentHP = currentHealth;
            _currentResidual = totalHealth;

            blockHP = (float)totalHealth / blockNum;

            if (objectID != this._objectID)
                GenerateBlock(_totalBlocks);

            float blockRatio = _currentHP / blockHP;

            _currentBlockIndex = Mathf.FloorToInt(blockRatio);

            UpdateHPBarEffect(_currentBlockIndex, blockRatio, blockRatio);

            this._objectID = objectID;
        }

        public int UpdateHPValue(float editValue)
        {
            float newHP = Mathf.Clamp(_currentHP + editValue, 0, this._totalHealth);

            float newBlockRatio = newHP / blockHP;
            float previousBlockRatio = _currentHP / blockHP;
            float previousBlockIndex = _currentBlockIndex;

            _currentBlockIndex = Mathf.FloorToInt(newBlockRatio);

            //Debug.Log($"_currentBlockIndex {_currentBlockIndex}, previousBlockRatio {previousBlockRatio}, newBlockRatio {newBlockRatio}");
            
            UpdateHPBarEffect(_currentBlockIndex, newBlockRatio, previousBlockRatio);

            if (previousBlockIndex != _currentBlockIndex && OnHealthBlockChange != null)
            {
                OnHealthBlockChange(_currentBlockIndex);
            }

            if ( newHP <= 0 && _currentHP > 0 && OnHealthEmptyOrNegative != null)
            {
               OnHealthEmptyOrNegative();
            }

            _currentHP = newHP;

            return CurrentHP;
        }

        public void Dispose()
        {
            _objectID = "";
            ClearUp();
            Show(false);
        }
        #endregion


        #region Private API
        private void UpdateHPBarEffect(int blockIndex, float newHpBlockRatio, float previousBlockRatio)
        {
            //Should be range between 0 - 1
            float ratio = newHpBlockRatio - blockIndex;
            float residualRatio = GetRatio(_currentResidual);

            UpdateMaterial(ratio, residualRatio, mainBarColor, GetNextBarColor());

            //if (blockIndex < Mathf.FloorToInt(previousBlockRatio))
            //{
                //Change Bottom Block Color
                UpdateBlockColor(blockIndex);
            //}
        }

        private void UpdateMaterial(float barHP, float residual, Color currentColor, Color nextColor)
        {
            healthBarMaterial.SetColor(ParameterFlag.HealthBarShader.BackgroundColor, nextColor);
            healthBarMaterial.SetColor(ParameterFlag.HealthBarShader.MainColor, currentColor);

            healthBarMaterial.SetFloat(ParameterFlag.HealthBarShader.Health, barHP);
            healthBarMaterial.SetFloat(ParameterFlag.HealthBarShader.Residual, residual);
        }

        private void GenerateBlock(int blockNum)
        {
            ClearUp();

            for (int i = blockNum - 1; i >= 0; i--)
            {
                Color c = healthBarSRP.barColor[i];
                var block = Utility.UtilityMethod.CreateObjectToParent<Image>(healthBlockHolder, healthBlockPrefab);
                block.color = c;

                blocks.Add(block);
            }
        }

        //Between 0 - 1
        private float GetRatio(float x)
        {
            return (x / blockHP) - _currentBlockIndex;
        }

        private void UpdateBlockColor(int index)
        {
            for (int i = 0; i < _totalBlocks; i++)
            {
                Color c = blocks[_totalBlocks - i - 1].color;
                c.a = (index >= i) ? 1 : 0;

                blocks[_totalBlocks - i -1].color = c;
            }
        }

        private Color GetNextBarColor()
        {
            if (_currentBlockIndex <= 0) return healthBarSRP.emptyColor;

            return healthBarSRP.barColor[_currentBlockIndex - 1];
        }

        private void ClearUp()
        {
            blocks.Clear();
            Utility.UtilityMethod.ClearChildObject(healthBlockHolder);
        }
        #endregion

        #region Monobehavior
        private void Update()
        {
            if (_currentResidual - _currentHP < _threshold)
            {
                _currentResidual = _currentHP;
                healthBarMaterial.SetFloat(ParameterFlag.HealthBarShader.Residual, GetRatio(_currentResidual));
                return;
            }

            _currentResidual = Mathf.Lerp(_currentResidual, _currentHP, 0.02f);

            healthBarMaterial.SetFloat(ParameterFlag.HealthBarShader.Residual, GetRatio(_currentResidual));
        }
        #endregion
    }
}

