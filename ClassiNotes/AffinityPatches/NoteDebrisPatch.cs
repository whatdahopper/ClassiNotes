using SiraUtil.Affinity;
using UnityEngine;

namespace ClassiNotes.AffinityPatches;

internal class NoteDebrisPatch : IAffinity
{
    [AffinityPatch(typeof(NoteDebris), "Init"), AffinityPrefix]
    void InitPrefix(Vector3 cutPoint, Vector3 cutNormal, ref Vector3 force, ref Vector3 torque, ref float lifeTime)
    {
        // From 0.11.2.
        float num = Vector3.Dot(cutNormal, Vector3.up);
        float d = (num + 1f) * 0.5f * 7f + 1f;
        float d3 = 4.5f;

        force = (cutNormal + Random.onUnitSphere * 0.2f) * d;
        torque = Random.insideUnitSphere * d3;
        lifeTime = 1f;
    }
}
