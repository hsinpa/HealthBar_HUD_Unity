# HealthBar_HUD_Unity
Plugin: health bar behave like DNF 

![Demo Effect](https://github.com/hsinpa/HealthBar_HUD_Unity/blob/main/Readme_asset/demo.gif?raw=true)

## How to use
  1. Drop Prefab[BossHealthBar] to scene
  2. You are free to change the layout on your style.
  
  3. Scriptable Object
  Change the color of health bar over here
  ![Demo Effect](https://github.com/hsinpa/HealthBar_HUD_Unity/blob/main/Readme_asset/HealthBar_ScriptableObj.jpg?raw=true)
  
  4. Set up
```
var healthBarView = this.GetComponent<HealthBarView>();
healthBarView.SetUp(target_id, currentHealth: 100, totalHealth: 100, blockNum: 5);

//Update healthbar
//Deduct 10 hp 
healthBarView.UpdateHPValue(-10);

//Resume 5 hp 
healthBarView.UpdateHPValue(5);
```
