using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IViewStrain.Repository.Interfaces;
using Ncto.Luk.Drawing;
using Ncto.Luk.iViewDB;

namespace IViewStrain.Repository.Tables
{
    public sealed class TableProvider : ITableProvider
    {
        /// <summary>
        /// Дополнительный отступ
        /// </summary>
        private const double EXTINDENT = 50;
        private bool _disposed;
        private SpanRange _range;
        private readonly ViewSQLiteTable _table;
        public SpanRange Range => _range;
        public ViewSQLiteTable Table => _table;
        public void Allocate(double distanceMin, double distanceSpan)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _table.Dispose();

            _disposed = true;
        }
    }
}
