using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class PlayAnimationNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<string>("animationTargetUuid").Build();
            context.AddInputPort<string>("animationTriggerName").Build();
            
            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<PlayAnimationBlockExecutor>();

            executor.executorUuid = uuid;
            executor.name = typeof(PlayAnimationBlockExecutor).ToString();

            var outputPort = GetOutputPortByName("TriggerOut");

            var connectedNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (connectedNode != null)
                executor.outputUuidTrigger = connectedNode.uuid;

            var animationTargetUuidPort = GetInputPortByName("animationTargetUuid");
            var animationTriggerUuidPort = GetInputPortByName("animationTriggerName");

            var targetNode = (StringVariableNode)animationTargetUuidPort.firstConnectedPort.GetNode();
            var animationNameNode = (StringVariableNode)animationTriggerUuidPort.firstConnectedPort.GetNode();

            if (targetNode == null || animationNameNode == null) {
                Debug.Log("an uuid was not set");
                return null;
            }

            targetNode.GetNodeOptionByName("uuid").TryGetValue(value: out string stringTargetUuid);
            targetNode.GetNodeOptionByName("value").TryGetValue(value: out string stringTargetUuidValue);
            
            animationNameNode.GetNodeOptionByName("uuid").TryGetValue(value: out string stringAnimationUuid);
            animationNameNode.GetNodeOptionByName("value").TryGetValue(value: out string stringAnimationValue);

            aiRuntimeGraph.stringData.TryAdd(stringTargetUuid, stringTargetUuidValue);
            aiRuntimeGraph.stringData.TryAdd(stringAnimationUuid, stringAnimationValue);
            
            executor.animationTarget = stringTargetUuid;
            executor.animationName = stringAnimationValue;

            return executor;
        }
    }
}