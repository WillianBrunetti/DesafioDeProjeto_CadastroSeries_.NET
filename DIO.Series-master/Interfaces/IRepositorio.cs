using System;
using System.Collections.Generic;


namespace DIO.Series.Interfaces
{
    public interface IRepositorio<T>
    {
         List<T> Lista();
         void  Insere(T entidade);
         void Exclui(int Id);
        void Atualiza(int id, T entidade);
        int ProximoId();
        T? BuscarPorId(int id);
    }
}