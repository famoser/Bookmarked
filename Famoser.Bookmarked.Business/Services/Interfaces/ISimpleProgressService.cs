namespace Famoser.LectureSync.Business.Services.Interfaces
{
    public interface ISimpleProgressService
    {
        void InitializeProgressBar(int total);
        void IncrementProgress();
        void HideProgress();
    }
}
