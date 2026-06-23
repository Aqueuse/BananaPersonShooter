using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class DecisionNode : BaseNode {
        private const string optionId = "portCount";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<int>(optionId).WithDefaultValue(2);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddInputPort<int>("choiceIndex").Build();

            var option = GetNodeOptionByName(optionId);
            option.TryGetValue(out int portCount);

            for (int i = 0; i < portCount; i++) {
                context.AddOutputPort<bool>("TriggerOut" + i).Build();
            }
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<DecisionBlockExecutor>();
            executor.executorUuid = uuid;
            executor.name = typeof(DecisionBlockExecutor).ToString();

            var choiceIndexPort = GetInputPortByName("choiceIndex");

            if (choiceIndexPort != null) {
                var variableNode = (IntVariableNode)choiceIndexPort .firstConnectedPort.GetNode();

                if (variableNode != null) {
                    variableNode.GetNodeOptionByName("uuid").TryGetValue(value: out string intUuid);
                    variableNode.GetNodeOptionByName("value").TryGetValue(value: out int intValue);
                    
                    aiRuntimeGraph.intData.TryAdd(intUuid, intValue);
                    executor.choiceIndexVariableUuid = intUuid;
                }
                else {
                    Debug.Log("choice index variable not set");
                }
            }
            
            var outputPorts = GetOutputPorts();

            List<string> choicesUuid = new List<string>();
            
            foreach (var outputPort in outputPorts) {
                var linkedPortUuid = ((BaseNode)outputPort.firstConnectedPort.GetNode()).uuid;
                choicesUuid.Add(linkedPortUuid);
            }

            aiRuntimeGraph.branchData.Add(uuid, choicesUuid);
            
            return executor;
       }
    }
}