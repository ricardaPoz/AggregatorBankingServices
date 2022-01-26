namespace AggregatorBankingServices.ParsingBankingServices.Helpers;
 public static class ParserExtension
{
    /// <summary>
    /// Метод для сортировки коллекции по имени банка
    /// </summary>
    /// <param name="credits_name_references">Слоаварь, содержащий наименование банка и ссылки на кредитные продукты</param>
    /// <param name="name_bank">Массив, содержащий названия банков для сортировки</param>
    /// <returns>Возвращает отсортированный словарь</returns>
    public static Dictionary<string, List<string>> SortBankName(this Dictionary<string, List<string>> credits_name_references, params string[] name_bank)
    {
        Dictionary<string, List<string>> _result_credits_name_references = new Dictionary<string, List<string>>();
        foreach (var item in credits_name_references)
        {
            foreach (var name in name_bank)
            {
                if (item.Key == name)
                {
                    item.Value.RemoveAt(0);
                    _result_credits_name_references.Add(item.Key, item.Value);
                }
            }
        }
        return _result_credits_name_references;
    }
}