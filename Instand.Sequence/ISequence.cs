namespace Instanda.Sequence
{
    public interface ISequence
    {
        long StartAt { get; set; }
        int Increment { get; set; }
        long MaxValue { get; set; }
        long MinValue { get; set; }
        bool Cycle { get; set; }
        long CurrentValue { get; set; }
    }
}