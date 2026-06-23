using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class WaitForTriggerNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<bool>("trigger").Build();

            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<WaitForTriggerBlockExecutor>();

            executor.executorUuid = uuid;
            executor.name = typeof(WaitForTriggerBlockExecutor).ToString();

            var outputPort = GetOutputPortByName("TriggerOut");

            var connectedNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (connectedNode != null)
                executor.outputUuidTrigger = connectedNode.uuid;
                        
            var input1Port = GetInputPortByName("trigger");

            if (input1Port != null) {
                var variableNode = (BoolVariableNode)input1Port.firstConnectedPort.GetNode();
                
                if (variableNode != null) {
                    variableNode.GetNodeOptionByName("uuid").TryGetValue(value: out string stringTriggerUuid);
                    variableNode.GetNodeOptionByName("value").TryGetValue(value: out bool stringTriggerValue);
                    
                    aiRuntimeGraph.boolData.TryAdd(stringTriggerUuid, stringTriggerValue);
                    executor.triggeringBoolUuid = stringTriggerUuid;
                }
                else {
                    Debug.Log("trigger not set");
                }
            }

            return executor;
        }
    }
}