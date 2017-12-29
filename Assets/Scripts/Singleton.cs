using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generic Singleton class. 
//A singleton is something we only want one of, and we always want to know what it is. (e.g. the FightManager. We only want one, so to refer to it we can always just say FightManager.instance)
//To make a class a singleton just have it inherit
//public class MyClass : Singleton<MyClass> {
public class Singleton<T> : MonoBehaviour
where T: Component{

    public static T instance;

    public virtual void Awake()
    {

        if (instance == null)
        {
            instance = this as T;
            
        }
        else
        {
            if (this as T != instance)
                Destroy(this.gameObject);
        }
    }

}
