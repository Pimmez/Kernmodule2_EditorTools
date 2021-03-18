using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Example : MonoBehaviour
{
    private ReflectionExample reflectionExample;
    private BindingFlags bindingFlag;
    // Start is called before the first frame update
    void Start()
    {
        MethodInfo[] methods = typeof(ReflectionExample).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (MethodInfo method in methods)
        {
            Debug.Log(method);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}