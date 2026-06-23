using System.Collections.Generic;
using Behaviours;
using UnityEngine;

public class AiRuntimeGraph : ScriptableObject {
    [SerializeReference] public GenericDictionary<string, bool> boolData = new();
    [SerializeReference] public GenericDictionary<string, float> floatData = new();
    [SerializeReference] public GenericDictionary<string, int> intData = new();
    [SerializeReference] public GenericDictionary<string, string> stringData = new();
    [SerializeReference] public GenericDictionary<string, List<string>> branchData = new();
    
    [SerializeReference] public GenericDictionary<string, AiBlockExecutor> executors = new();
}