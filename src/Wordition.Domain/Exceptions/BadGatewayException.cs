namespace Wordition.Domain.Exceptions;

public class BadGatewayException(string serviceName) : Exception ($"Service {serviceName} sent incorrect data") {}