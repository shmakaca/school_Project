using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class mainMenu : MonoBehaviour
{
    public Button con;
    public Button setting;
    public Button quit;

    public GameObject setMenu;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            menu.SetActive(true);
            Cursor.visible = true;
        }
        
        con.GetComponent<Button>().onClick.AddListener(exit);
    }
    public void exit()
    {
        menu.SetActive(false);

    }
}
