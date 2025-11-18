using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Common.Behaviors
{
	internal class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _valiators;
		public ValidationBehavior(IEnumerable<IValidator<TRequest>> valiators)
		{
			_valiators = valiators;
		}

		public Task<TResponse> Handle(TRequest request , RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
		{
			var context = new ValidationContext<TRequest>(request); 
			var failures = _valiators.Select(v => v.Validate(context)).SelectMany(result => result.Errors).Where(failure => failure != null).ToList();
			if(failures.Count != 0)
			{
				throw new FluentValidation.ValidationException(failures); 
			}
			return next(); 
		}
	}
}
