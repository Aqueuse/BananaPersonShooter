using Unity.GraphToolkit.Editor;

namespace Editor.FlowNoddles.variables {
    public class FloatVariableNode : Node {
        protected override void OnDefinePorts(IPortDefinitionContext context) {
            context.AddOutputPort<float>("variable").Build();
        }
        
        protected override void OnDefineOptions(IOptionDefinitionContext context) {
            context.AddOption<string>(name: "uuid").ShowInInspectorOnly().Build();
            context.AddOption<float>(name: "value").ShowInInspectorOnly().Build();
        }
    }
}