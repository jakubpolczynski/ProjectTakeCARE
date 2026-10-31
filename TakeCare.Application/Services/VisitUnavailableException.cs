namespace TakeCare.Application.Services;

public sealed class VisitUnavailableException : Exception
{
    public VisitUnavailableException() : base("The visit is no longer available for an examination.") { }
}
