using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;

public interface IDictionaryGateway<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(Guid id);
}