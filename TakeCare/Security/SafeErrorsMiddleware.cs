namespace TakeCare.Security;

public sealed class SafeErrorsMiddleware(RequestDelegate next, ILogger<SafeErrorsMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested) { }
        catch (Exception exception)
        {
            // Do not log the exception: provider errors can contain SQL values or medical data.
            logger.LogError("Request failed. ErrorType={ErrorType} TraceId={TraceId}",
                exception.GetType().Name, context.TraceIdentifier);
            if (context.Response.HasStarted) { context.Abort(); return; }
            context.Response.Clear();
            context.Response.Headers.CacheControl = "no-store";
            await Results.Problem(statusCode: 500, title: "Request could not be completed.",
                extensions: new Dictionary<string, object?> { ["traceId"] = context.TraceIdentifier }).ExecuteAsync(context);
        }
    }
}
