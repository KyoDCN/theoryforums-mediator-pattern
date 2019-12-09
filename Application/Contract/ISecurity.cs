using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contract
{
    public interface ISecurity
    {
        Task<string> GenerateLoginTokenAsync(User user);
    }
}
