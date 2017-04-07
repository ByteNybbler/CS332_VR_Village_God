// Author(s): Paul Calande
// A parent class for late initialization functionality.
// Inherit from this if you need to pass parameters to an instantiated object
// that needs to perform initialization after receiving the parameters.
// All of this initialization should be contained in the Init() function,
// which you should override in your own classes.
// Use Init() instead of Start().

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LateInit : MonoBehaviour
{
    // Whether Init() has been called yet. One call to Init() will make this true.
    protected bool isInitialized = false;

    // This function should be overrided to handle all of the initialization.
    // In derived classes, call base.Init() at the VERY END of the overrided function.
    public virtual void Init()
    {
        EventsSubscribe();
        isInitialized = true;
    }

    // In derived classes, this function should subscribe to the events of other classes.
    protected virtual void EventsSubscribe()
    {
        // Override this in your derived classes.
    }

    // In derived classes, this function should unsubscribe from the events of other classes.
    protected virtual void EventsUnsubscribe()
    {
        // Override this in your derived classes.
    }

    // OnEnable() usually gets called before assignments can be made to the properties of an
    // instantiated object. Because of this, we need to check if the properties have been
    // initialized before the event subscriptions can be populated (since this uses
    // references to other GameObject instances which might be null before the Init() call).
    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        if (isInitialized)
        {
            EventsUnsubscribe();
        }
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        EventsUnsubscribe();
    }

    protected void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("SceneManager_SceneLoaded called!");
        Init();
    }
}