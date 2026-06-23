using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class DebugNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<string>("message").Delayed().Build();
            
            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }
        
        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<DebugBlockExecutor>();
            executor.executorUuid = uuid;
            executor.name = typeof(DebugBlockExecutor).ToString();
            
            var outputPort = GetOutputPortByName("TriggerOut");
            
            var connectedNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (connectedNode != null)
                executor.outputUuidTrigger = connectedNode.uuid;
            
            var input1Port = GetInputPortByName("message");
            
            if (input1Port != null) {
                var variableNode = (StringVariableNode)input1Port.firstConnectedPort.GetNode();
            
                if (variableNode != null) {
                    variableNode.GetNodeOptionByName("value").TryGetValue(value: out string stringValue);
                    variableNode.GetNodeOptionByName("uuid").TryGetValue(value: out string stringUuid);
                    
                    aiRuntimeGraph.stringData.TryAdd(stringUuid, stringValue);
                    executor.messageVariableUuid = stringUuid;
                }
                else {
                    Debug.Log("debug message not set");
                }
            }
            
            return executor;
       }
    }
}