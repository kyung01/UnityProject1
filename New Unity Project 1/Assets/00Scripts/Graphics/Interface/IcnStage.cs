using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class IcnStage : EasyGameObject
{
    public int id;
    public EasyTextMesh textId;
    public void display(int id, string name, DataLevel data)
    {
        this.id = id;
        textId.display(name);
        //do something with the Data
    }
    public void Update()
    {
        
    }
    void OnMouseDown()
    {
        Debug.Log("StageDown");
        SceneManager.EVENT_TranslationSceneInit2Game();
    }
}