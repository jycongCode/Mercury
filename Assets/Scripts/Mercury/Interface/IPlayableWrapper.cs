using UnityEngine.Playables;
public interface IPlayableWrapper
{
    Playable PlayableHandle { get; set; }
    IPlayableWrapper Parent { get; set; }
    float Speed { get;set; }
    float NormalizedTime {  get; set; }
    int Index {  get; set; }  
    bool IsPlaying { get; set; }    
    MercuryPlayable Root { get; }
    void RemoveFromGraph(int index);
    void AddToGraph(int index, MercuryState state);
    void SetInputWeight(int index,float weight);
}