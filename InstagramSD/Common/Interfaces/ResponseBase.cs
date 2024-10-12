using System;
namespace Common.Interfaces
{
	public abstract class ResponseBase
	{
		public bool IsSuccess { get; set; } = true;
		public string ErrorMessage { get; set; }
	}
}

