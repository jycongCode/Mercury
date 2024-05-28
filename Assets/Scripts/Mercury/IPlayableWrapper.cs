using UnityEngine.Playables;
public interface IPlayableWrapper
{
    Playable Parent { get; set; }
    MercuryPlayable Root { get; set; }
}