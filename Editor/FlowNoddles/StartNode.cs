using System;
using Behaviours;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace Editor.FlowNoddles {
    [Serializable]
    public class StartNode : BaseNode {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort<bool>("TriggerOut").Build();
        }

        public override AiBlockExecutor ConvertToExecutor(AiRuntimeGraph aiRuntimeGraph) {
            var executor = ScriptableObject.CreateInstance<StartBlockExecutor>();
            executor.executorUuid = uuid;
            executor.name = typeof(StartBlockExecutor).ToString();

            var outputPort = GetOutputPortByName("TriggerOut");
            var connectedNode = (BaseNode)outputPort.firstConnectedPort.GetNode();

            if (connectedNode != null)
                executor.outputUuidTrigger = connectedNode.uuid;

            return executor;
        }
    }
}