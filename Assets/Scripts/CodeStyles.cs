using System;
using UnityEngine;

// CodeStyles For this C# Project
// Make sure your IDE using this CodeStyle 
public class CodeStyles
{
    // Constants: UPPERCASE with Snake_Case
    public const int CONSTANT_FIELD = 56;
    
    // Properties: PascalCase
    public static CodeStyles Instance { get; private set; }
    
    // Events: PascalCase
    public event EventHandler OnSomethingHappened;
    
    // Private Field: _camelCase
    private float _memberVariable;

    // Function Name: PascalCase 
    private void Awake()
    {
        Instance = this;
        DoSomething(10f);
    }
    
    // Function Params: camelCase
    private void DoSomething(float time)
    {
        // Do Something
        _memberVariable = time + Time.deltaTime;
        if (_memberVariable > 0)
        {
            // Do Something else
        }
    }
}
