using ByteBank_v01.Model;
using Newtonsoft.Json;

namespace ByteBank_v01;
public class JsonIO
{
    static string repositoryPath = "./repository.json";

    public static bool JsonSerialize(List<Account> lista)
    {
        string strJson = JsonConvert.SerializeObject(lista, Formatting.Indented);
        return SaveTxtFile(strJson);
    }

    public static List<Account> JsonDesserialize()
    {
        string strJson = OpenTxtFile();
        return JsonConvert.DeserializeObject<List<Account>>(strJson);
    }

    private static bool SaveTxtFile(string strJson)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(repositoryPath))
                sw.WriteLine(strJson);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private static string OpenTxtFile()
    {
        string strJson = "";
        
        try
        {         
            using (StreamReader sr = new StreamReader(repositoryPath))
                strJson = sr.ReadToEnd();
        }
        catch (Exception)
        {
            strJson = "[]";
            using (StreamWriter sw = new StreamWriter(repositoryPath))
                sw.WriteLine(strJson);
        }

        return strJson;
    }
}
