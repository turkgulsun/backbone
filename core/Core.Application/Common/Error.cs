using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Common;

public class Error
{
	public List<ErrorInfo> ErrorInfos { get; set; } = new List<ErrorInfo>();
}

public class ErrorInfo
{
	public string Message { get; set; }

	public string? StackTrace { get; set; }

	public string? Source { get; set; }
}
