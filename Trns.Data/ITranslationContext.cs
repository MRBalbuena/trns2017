using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trns.Data.Models;
using Trns.Data.Trs.Data;

namespace Trns.Data
{
    public interface ITranslationContext: IModel, IDisposable
    {
        DbSet<Translation> Translations { get; set; }
    }
}
