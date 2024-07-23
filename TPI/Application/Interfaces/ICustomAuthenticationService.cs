using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces
{
    public interface ICustomAuthenticationService
    {
        string Autenticar(AuthenticationRequest authenticationRequest);
    }
}
