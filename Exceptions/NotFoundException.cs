namespace MyApp.Exceptions;

public class NotFoundException(string message) : Exception(message);