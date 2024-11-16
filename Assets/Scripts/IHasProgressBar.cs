using System;

public interface IHasProgressBar
{
    public event EventHandler<ProgressEventArgs> OnProgressChanged;
}

public class ProgressEventArgs : EventArgs
{
    public float ProgressNormalized;
}
