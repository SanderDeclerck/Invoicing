using System.Globalization;

public class CultureScope : IDisposable
{
    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUICulture;

    public CultureScope(CultureInfo newCulture)
    {
        // Store the current culture and UI culture
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUICulture = Thread.CurrentThread.CurrentUICulture;

        // Set the new culture and UI culture
        Thread.CurrentThread.CurrentCulture = newCulture;
        Thread.CurrentThread.CurrentUICulture = newCulture;
    }

    public void Dispose()
    {
        // Revert to the original culture and UI culture
        Thread.CurrentThread.CurrentCulture = _originalCulture;
        Thread.CurrentThread.CurrentUICulture = _originalUICulture;
    }
}