namespace AACore.Web.Utils;

public static class ExceptionUtils
{
    /// <summary>
    /// Ignore exceptions thrown by the operation. Return true if the operation completes successfully, false otherwise.
    /// </summary>
    /// <param name="operation">An operation.</param>
    /// <returns>Return true if the operation completes successfully, false otherwise.</returns>
    public static bool IgnoreException(Action operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        try
        {
            operation();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Ignore exceptions thrown by the operation, and return a default value if there's an exception.
    /// </summary>
    /// <param name="operation">An operation.</param>
    /// <param name="defaultValue">The value to be returned if there's an exception.</param>
    /// <typeparam name="T">The return type of the function</typeparam>
    /// <returns>Return the result of the operation, or a default value if there's an exception.</returns>
    public static T IgnoreException<T>(Func<T> operation, T defaultValue = default(T))
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            return operation();
        }
        catch
        {
            return defaultValue;
        }
    }
}