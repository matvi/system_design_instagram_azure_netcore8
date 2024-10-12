using System;
using Domain.EventMessages;

namespace PostProcessor.Services
{
	public interface IProcessPostCreatedService
	{
		Task Process(PostCreated postCreated);
	}
}

