namespace Wordition.Domain.Exceptions;

public class NotUniqueLoginException(string userName) : Exception($"This {userName} already exists");