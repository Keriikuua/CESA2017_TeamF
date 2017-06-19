using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// コルーチンを一時停止させるクラス
/// </summary>
public class PausableCoroutine : MonoBehaviour
{

    public static List<PausableCoroutine> grobalPausables = new List<PausableCoroutine>();

    public static void PauseAllPausableCoroutinesGlobal()
    {
        grobalPausables.RemoveAll((obj) => obj == null);

        foreach (var obj in grobalPausables)
        {
            obj.PauseAllPausableCoroutines();
            Debug.Log(obj.name);
        }
    }

    public static void ResumeAllPausableCoroutinesGlobal()
    {
        grobalPausables.RemoveAll((obj) => obj == null);

        foreach (var obj in grobalPausables)
        {
            obj.ResumeAllPausableCoroutines();
        }
    }

    public static void StopAllPausableCoroutinesGlobal()
    {
        grobalPausables.RemoveAll((obj) => obj == null);

        foreach (var obj in grobalPausables)
        {
            obj.StopAllPausableCoroutines();
            Debug.Log(obj.name);
        }
    }

    public static void RegistarForGrobalPausables(PausableCoroutine pausable)
    {
        grobalPausables.Add(pausable);
        Debug.Log(grobalPausables[0].name);
    }

    private Dictionary<string, IEnumerator> enumerators = new Dictionary<string, IEnumerator>();
    private bool isPausing = false;

    public void StartPausableCoroutine(string methodName, object[] args = null)
    {
        if (isPausing)
        {
            Debug.Log("return");
            return;
        }

        // Coroutine "methodName" previously invoked is running,
        // Stop it and, Restart
        if (enumerators.ContainsKey(methodName))
        {
            StopPausableCoroutine(methodName);
            Debug.Log(methodName);
        }
        Debug.Log(methodName);

        MethodInfo method = this.GetType().GetMethod(methodName);

        // Store reference of IEnumerator and start coroutine
        if (method != null)
        {
            var enumerator = (IEnumerator)this.GetType().GetMethod(methodName).Invoke(this, args);

            enumerators.Add(methodName, enumerator);
            StartCoroutine(enumerator);
        }
    }

    public void StopPausableCoroutine(string methodName)
    {
        if (enumerators.ContainsKey(methodName))
        {
            StopCoroutine(enumerators[methodName]);
            enumerators.Remove(methodName);
        }
    }

    public void StopAllPausableCoroutines()
    {
        foreach (var e in enumerators)
        {
            StopCoroutine(e.Value);
        }

        enumerators.Clear();
    }

    public void PauseAllPausableCoroutines()
    {
        if (!isPausing)
        {
            isPausing = true;
            Debug.Log("hey:"+ enumerators);

            foreach (var e in enumerators)
            {
                StopCoroutine(e.Value);

                Debug.Log("value:"+e.Value);
            }
        }
    }

    public void ResumeAllPausableCoroutines()
    {
        if (isPausing)
        {
            isPausing = false;

            foreach (var e in enumerators)
            {
                StartCoroutine(e.Value);
            }
        }
    }
}