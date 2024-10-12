using System;
namespace PostService.Models
{
	public abstract class Result
	{
		public bool IsSuccess { get; set; } = true;
	}
}

