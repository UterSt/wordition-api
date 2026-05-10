namespace Wordition.Domain.Exceptions;

public class NotFoundException(string entity, object id) : Exception($"{entity} with id {id} not found");