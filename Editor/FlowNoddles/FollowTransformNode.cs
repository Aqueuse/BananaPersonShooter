using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class FollowTransformNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<string>("aiTargetUuid").Build();
            context.AddInputPort<string>("aiAgentUuid").Build();
            context.AddInputPort<bool>("isFollowing").Build();
            
            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }
        
        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<FollowTransformBlockExecutor>();

            executor.executorUuid = uuid;
            executor.name = typeof(FollowTransformBlockExecutor).ToString();
            
            var outputPort = GetOutputPortByName("TriggerOut");
            var outputTriggerNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (outputTriggerNode != null)
                executor.outputUuidTrigger = outputTriggerNode.uuid;

            var aiTargetUuidNode = (StringVariableNode)GetInputPortByName("aiTargetUuid").firstConnectedPort.GetNode();
            var aiAgentUuidNode = (StringVariableNode)GetInputPortByName("aiAgentUuid").firstConnectedPort.GetNode();
            var isFollowingNode = (BoolVariableNode)GetInputPortByName("isFollowing").firstConnectedPort.GetNode();
            
            if (aiTargetUuidNode == null || aiAgentUuidNode == null || isFollowingNode == null) {
                Debug.Log("an uuid was not set");
                return null;
            }
            
            aiTargetUuidNode.GetNodeOptionByName("uuid").TryGetValue(value: out string aiTargetUuid);
            aiTargetUuidNode.GetNodeOptionByName("value").TryGetValue(value: out string aiTargetUuidValue);
            
            aiAgentUuidNode.GetNodeOptionByName("uuid").TryGetValue(value: out string aiAgentUuid);
            aiAgentUuidNode.GetNodeOptionByName("value").TryGetValue(value: out string aiAgentUuidValue);

            isFollowingNode.GetNodeOptionByName("uuid").TryGetValue(value: out string isFollowingBoolUuid);
            isFollowingNode.GetNodeOptionByName("value").TryGetValue(value: out bool isFollowingValue);
            
            executor.aiTargetUuid = aiTargetUuid;
            executor.aiAgentUuid = aiAgentUuid;
            executor.isFollowingBoolUuid = isFollowingBoolUuid;

            aiRuntimeGraph.stringData.TryAdd(aiTargetUuid, aiTargetUuidValue);
            aiRuntimeGraph.stringData.TryAdd(aiAgentUuid, aiAgentUuidValue);
            aiRuntimeGraph.boolData.TryAdd(isFollowingBoolUuid, isFollowingValue);
            
            return executor;
        }
    }
}