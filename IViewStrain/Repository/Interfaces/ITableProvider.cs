using IViewStrain.Repository.Interfaces.Tables;
using Ncto.Luk.Drawing;
using Ncto.Luk.iViewDB;

namespace IViewStrain.Repository.Interfaces
{
    public interface ITableProvider : IDataViewProvider, ITableColumns, ITableUnit
    {
        /// <summary>
        /// Диапазон видимых данных
        /// </summary>
        public SpanRange Range { get; }
        /// <summary>
        /// Таблица особенностей
        /// </summary>
        public ViewSQLiteTable Table { get; }
        /// <summary>
        /// Загружает особенности в представления данных (DataView)
        /// <see cref="IDataViewProvider"/>
        /// </summary>
        /// <param name="distanceMin"></param>
        /// <param name="distanceSpan"></param>
        public void Allocate(double distanceMin, double distanceSpan);
    }
}
