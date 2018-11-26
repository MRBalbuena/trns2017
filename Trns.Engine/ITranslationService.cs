using System;
using System.Collections.Generic;
using System.Text;
using Trns.Data.Models;
using Trns.Engine.Models;

namespace Trns.Engine
{
    public interface ITranslationService
    {
        IEnumerable<Translation> GetTopToTranslate(int top);
        Translation GetPhrase(int id);
        Stats GetStats();
        string BlockPhrase(int id, string user);
        bool SaveTranslation(Translation translation);
        IEnumerable<Translation> SearchByWord(string words);
        IEnumerable<Translation> GetUnchecked();
    }
}
