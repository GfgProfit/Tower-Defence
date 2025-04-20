using UnityEngine;

public interface IRocket
{
    bool IsRocketReadyToLaunch { get; }
    void PrepareForLaunch(float appearanceDuration);
    void Launch(Vector3 targetPosition, float flyDuration, float damage, ParticleSystem explosionPrefab);
}