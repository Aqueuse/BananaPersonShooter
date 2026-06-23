using System;
using System.Collections.Generic;
using Behaviours;
using Editor.FlowNoddles.variables;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class DestroyTargetNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddInputPort<string>("TargetUuid").Build();
            
            context.AddInputPort<bool>("TriggerIn").Build();
            context.AddOutputPort<bool>("TriggerOut").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<DestroyTargetBlockExecutor>();
            executor.executorUuid = uuid;
            executor.name = typeof(DestroyTargetBlockExecutor).ToString();
            
            var targetUuidNode = (StringVariableNode)GetInputPortByName("TargetUuid").firstConnectedPort.GetNode();
            
            targetUuidNode.GetNodeOptionByName("uuid").TryGetValue(value: out string targetUuid);
            targetUuidNode.GetNodeOptionByName("value").TryGetValue(value: out string targetUuidValue);
            
            executor.targetUuid = targetUuid;

            aiRuntimeGraph.stringData.TryAdd(executor.targetUuid, targetUuidValue);
            
            return executor;
        }
    }
}