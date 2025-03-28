public class UFEService
{
    private static readonly object _lock = new object();
    private static UFEService? _instance;
    private decimal _currentFee = 1.0m;
    private DateTime _lastUpdate = DateTime.MinValue;

    private UFEService()
    {
        UpdateFee();
    }

    public static UFEService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UFEService();
                    }
                }
            }
            return _instance;
        }
    }

    public decimal GetCurrentFee()
    {
        if (DateTime.Now.Subtract(_lastUpdate).TotalHours >= 1)
        {
            UpdateFee();
        }
        return _currentFee;
    }

    private void UpdateFee()
    {
        lock (_lock)
        {
            if (DateTime.Now.Subtract(_lastUpdate).TotalHours >= 1)
            {
                Random random = new Random();
                decimal randomMultiplier = (decimal)(random.NextDouble() * 2); // Genera decimal entre 0 y 2
                _currentFee *= randomMultiplier; // Multiplica la tarifa actual por el decimal aleatorio
                _lastUpdate = DateTime.Now;
            }
        }
    }
}