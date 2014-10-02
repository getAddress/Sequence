namespace getAddress.Sequence
{
    public interface ISequence
    {
        long StartAt { get;  }
        int Increment { get;  }
        long MaxValue { get;  }
        long MinValue { get;  }
        bool Cycle { get;  }
        long CurrentValue { get; set; }
    }
}