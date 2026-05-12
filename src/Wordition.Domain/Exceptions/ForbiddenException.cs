namespace Wordition.Domain.Exceptions;

public class ForbiddenException(Guid userId, string resourceType, string resourceId) : Exception($"User with Id: {userId} is trying to access resource: {resourceType }with Id: {resourceId}") {}