using System;
using System.Collections.Generic;
using System.Linq;
using Trns.Data;
using Trns.Data.Models;
using Trns.Engine.Models;

namespace Trns.Engine
{
    public class TranslationService: ITranslationService
    {
        private readonly ITranslationContext _repo;

        public TranslationService(ITranslationContext translationContext)
        {
            _repo = translationContext;
        }
        public IEnumerable<Translation> GetTopToTranslate(int top)
        {
            var translations = _repo.Translations
                .Where(t => t.Spanish == null && t.Edition == "2019-07")
                .Take(top)
                .OrderBy(t => t.Id)
                .ToList();
            translations.Where(t => t.BlockedTime < DateTime.Now.AddMinutes(-10)).ToList().ForEach(t => {
                t.BlockedBy = string.Empty; t.BlockedTime = null;
            });
            return translations;
        }

        public Translation GetPhrase(int id)
        {
            return _repo.Translations.Find(id);
        }

        public Stats GetStats()
        {
            var tot = _repo.Translations.Where(t => t.Edition == "2019-07").Count();
            double translated = _repo.Translations.Where(t => t.Edition == "2019-07").Count(t => !string.IsNullOrEmpty(t.Spanish));
            double transChecked = _repo.Translations.Where(t => t.Edition == "2019-07").Count(t => !string.IsNullOrEmpty(t.CheckedBy));
            return new Stats
            {
                Checked = Convert.ToInt32(transChecked),
                TotalTranslations = tot,
                Translated = Convert.ToInt32(translated),
                CheckedPercent = (transChecked / tot) * 100,
                TranslationsPercent = (translated / tot) * 100
            };
        }

        public string BlockPhrase(int id, string user)
        {
            var phrase = _repo.Translations.Find(id);
            if (!string.IsNullOrEmpty(phrase.BlockedBy)) return phrase.BlockedBy;
            if (!string.IsNullOrEmpty(phrase.Spanish)) return phrase.TransBy;
            phrase.BlockedBy = user;
            phrase.BlockedTime = DateTime.Now;
            _repo.Translations.Update(phrase);
            _repo.SaveChanges();
            return "";
        }

        public bool SaveTranslation(Translation translation)
        {
            var phrase = _repo.Translations.Find(translation.Id);
            if (phrase != null)
            {
                phrase.Spanish = translation.Spanish;
                phrase.TransBy = translation.TransBy;
                if (translation.TransBy != null) phrase.TransDate = DateTime.Now;
                phrase.BlockedBy = null;
                phrase.BlockedTime = null;
                phrase.CheckedBy = translation.CheckedBy;
                if (translation.CheckedBy != null) phrase.CheckedTime = DateTime.Now;
                phrase.EditedBy = translation.EditedBy;
                if (translation.EditedBy != null) phrase.EditedTime = DateTime.Now;
                phrase.Comment = translation.Comment;
            }
            _repo.Translations.Update(phrase);
            _repo.SaveChanges();
            return true;
        }

        public IEnumerable<Translation> SearchByWord(string words)
        {
            var Translations =
                _repo.Translations.Where(t => t.Text.ToLower().Contains(words.ToLower()) && !string.IsNullOrEmpty(t.Spanish))
                .Take(50)
                .ToList();
            return Translations;
        }

        public IEnumerable<Translation> GetUnchecked()
        {
            return _repo.Translations.ToList()
                .Where(t => String.IsNullOrEmpty(t.CheckedBy) 
                    && t.Spanish != null
                    && t.Edition == "2019-07")
                .Take(20);
        }

    }
}
