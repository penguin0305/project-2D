public interface IEnhanceService
{
    void Enhance(int collectionId, CollectionState state, System.Action<CollectionState> onComplete);
}