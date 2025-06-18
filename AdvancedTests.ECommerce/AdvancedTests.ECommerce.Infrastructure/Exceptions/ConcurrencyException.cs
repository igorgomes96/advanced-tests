namespace AdvancedTests.ECommerce.Infrastructure.Exceptions;

public class ConcurrencyException(string message): Exception(message);