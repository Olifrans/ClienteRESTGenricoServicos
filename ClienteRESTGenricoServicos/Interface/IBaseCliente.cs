using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ClienteRESTGenricoServicos.Interface
{
    public interface IBaseCliente<T> : IDisposable
    {
        Task<List<T>> GetAll();

        Task<T> Get(int? id);

        Task Create(T item);

        Task Update(int? id, T item);

        Task Delete(int id);
    }
}
