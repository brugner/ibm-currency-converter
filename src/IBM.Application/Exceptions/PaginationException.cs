using System.Runtime.Serialization;

namespace IBM.Application.Exceptions;

[Serializable]
public class PaginationException : Exception
{
	public PaginationException()
	{

	}

	public PaginationException(string message) : base(message)
	{

	}

	public PaginationException(string message, Exception inner) : base(message, inner)
	{

	}

	protected PaginationException(SerializationInfo info, StreamingContext context) : base(info, context)
	{

	}
}