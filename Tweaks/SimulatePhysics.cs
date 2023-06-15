using UnityEngine;

public class SimulatePhysics : MonoBehaviour {
    public bool isSimulatingPhysic;

    void FixedUpdate() {
        if (isSimulatingPhysic) {
            Physics.Simulate(Time.fixedDeltaTime);
        }
    }
}
