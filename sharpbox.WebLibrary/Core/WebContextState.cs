namespace sharpbox.WebLibrary.Core
{
    public enum WebContextState
    {
        Waiting,
        ProcessingRequest,
        ResponseSet,
        ResponseProcessed,
        Faulted

    }
}