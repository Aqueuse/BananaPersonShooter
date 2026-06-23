using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class WaitNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<int>("seconds").Build();

            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<WaitForSecondsBlockExecutor>();

            executor.executorUuid = uuid;
            executor.name = typeof(WaitForSecondsBlockExecutor).ToString();

            var outputPort = GetOutputPortByName("TriggerOut");

            var connectedNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (connectedNode != null)
                executor.outputUuidTrigger = connectedNode.uuid;
                        
            var input1Port = GetInputPortByName("seconds");

            if (input1Port != null) {
                var variableNode = (IntVariableNode)input1Port.firstConnectedPort.GetNode();

                if (variableNode != null) {
                    variableNode.GetNodeOptionByName("uuid").TryGetValue(value: out string stringSecondsUuid);
                    variableNode.GetNodeOptionByName("value").TryGetValue(value: out int stringSecondsValue);

                    aiRuntimeGraph.intData.TryAdd(stringSecondsUuid, stringSecondsValue);
                    executor.secondsVariableUuid = stringSecondsUuid;
                }
                else {
                    Debug.Log("seconds not set");
                }
            }

            return executor;
        }
    }
}