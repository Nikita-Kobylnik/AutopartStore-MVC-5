using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutopartStore.Domain.Entities;

namespace AutopartStore.Domain.Abstract
{
    public interface IAutopartRepository
    {
        IEnumerable<Autopart> Autoparts { get; }
        void SaveAutopart(Autopart autopart);
        Autopart DeleteAutopart(int autopartId);
    }
}
