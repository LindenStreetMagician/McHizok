using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHizok.Entities.Exceptions;

public class RegistrationTokenNotProvidedBadRequestException : BadRequestException
{
	public RegistrationTokenNotProvidedBadRequestException()
		: base("The provided token was invalid")
	{
	}
}
