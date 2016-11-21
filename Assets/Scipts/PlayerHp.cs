using UnityEngine;
using System.Collections;

public class PlayerHp : MonoBehaviour {
    public int max_HP;
    public int current_HP;
    // Use this for initialization
    void Awake () {
        current_HP = max_HP;
    }
	
	public void Hitted()
    {
        current_HP -= 1;
        if (current_HP == 0)
        {
            Application.LoadLevel(Application.loadedLevel);
            //Destroy(gameObject);
        }
    }
}
