using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class EndNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<bool>("DestroyYourself").Build();
            context.AddInputPort<bool>("IsLooping").Build();

            context.AddInputPort<bool>("TriggerIn").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<EndBlockExecutor>();
            executor.executorUuid = uuid;
            executor.name = typeof(EndBlockExecutor).ToString();
            
            var destroyUuidNode = (BoolVariableNode)GetInputPortByName("DestroyYourself").firstConnectedPort.GetNode();
            var isLoopingUuidNode = (BoolVariableNode)GetInputPortByName("IsLooping").firstConnectedPort.GetNode();
            
            destroyUuidNode.GetNodeOptionByName("uuid").TryGetValue(value: out string destroyUuid);
            destroyUuidNode.GetNodeOptionByName("value").TryGetValue(value: out bool destroyUuidValue);

            isLoopingUuidNode.GetNodeOptionByName("uuid").TryGetValue(value: out string isLoopingUuid);
            isLoopingUuidNode.GetNodeOptionByName("value").TryGetValue(value: out bool isLoopingValue);
            
            executor.destroyYourselfUuid = destroyUuid;
            executor.isLoopingUuid = isLoopingUuid;
            
            aiRuntimeGraph.boolData.TryAdd(destroyUuid, destroyUuidValue);
            aiRuntimeGraph.boolData.TryAdd(isLoopingUuid, isLoopingValue);
            
            return executor;
        }
    }
}