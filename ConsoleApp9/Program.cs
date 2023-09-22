using System.Numerics;
using System.Xml.Linq;

internal class Program
{
    static string database = "dwb.txt";
    static (string name, string surname, string phone, DateTime birth)[] contacts;

    static void Main(string[] args)
    {
       
        string[] records = ReadDatabaseAllTextLines(database);
        contacts = ConvertStringsToContacts(records);

        while (true)
        {
            UserInteraction();
        }
    }

    static void UserInteraction()
    {
        Console.WriteLine("1. All contacts");
        Console.WriteLine("2. Add contact");
        Console.WriteLine("3. Edit contact");
        Console.WriteLine("4. Search");
        Console.WriteLine("6. Save");
        Console.WriteLine("So? ");

        uint input = uint.Parse(Console.ReadLine());
        switch (input)
        {
            case 1:
                WriteAllContactsToConsole();
                break;
            case 2:
                AddNewContact();
                break;
            case 3:
                EditContact();
                break;
            case 4:
                SearchContact();
                break;
            case 6:
                SaveContactsToFile();
                break;
            default:
                Console.WriteLine("I don`t know your variant!");
                break;
        }
    }

    static void AddNewContact()
    {
        Console.WriteLine("Name: ");

        string first = Console.ReadLine();

        Console.WriteLine("Surname: ");

        string second = Console.ReadLine();

        Console.WriteLine("Phone number: ");
        string third = Console.ReadLine();
        
        DateTime date = DateTime.Now;
        
        Console.Write("Please, your date of birth: ");
        date = DateTime.Parse(Console.ReadLine()); 
        
        Array.Resize(ref contacts, contacts.Length + 1);
        contacts[^1] = (first, second, third, date);
    }
    static int SearchContact()
    {
        Console.Write("Enter name/surname/phone number (only 1 variant!): ");
        string searchQuery = Console.ReadLine().Trim(); 

        for (int i = 0; i < contacts.Length; ++i)
        {
            if (contacts[i].name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || 
                contacts[i].surname.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                contacts[i].phone.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"#{i + 1}: {contacts[i].name}, {contacts[i].surname}, {contacts[i].phone}, {contacts[i].birth}");
                return i;
            }
        }
        return -1;
    }

    static void EditContact()
    {
        int id = SearchContact();
        if (id == -1)
        {
            Console.WriteLine("We don`t have this contact!");
            return;
        }
        Console.WriteLine("Name: ");

        string first = Console.ReadLine();

        Console.WriteLine("Surname: ");

        string third = Console.ReadLine();

        Console.WriteLine("Phone: ");
        string second = Console.ReadLine();

        DateTime date = DateTime.Now;
        Console.Write("Date of burth: ");
        date = DateTime.Parse(Console.ReadLine());
        
        Console.WriteLine("What? I don`t know this format.");
        

        contacts[id] = (first, third, second, date);
    }

   
    static void WriteAllContactsToConsole()
    {

        Array.Sort(contacts, (contact1, contact2) => contact1.name.CompareTo(contact2.name));

        for (int i = 0; i < contacts.Length; i++)
        {
            
            Console.WriteLine($"#{i + 1}: Name: {contacts[i].Item1}, Surname: {contacts[i].Item2}, Phone: {contacts[i].Item3}");
        }
    }

    static (string name, string surname, string phone, DateTime date)[] ConvertStringsToContacts(string[] records)
    {
        
        var contacts = new (string name, string surname, string phone, DateTime date)[records.Length];
        for (int i = 0; i < records.Length; ++i)
        {
            string[] array = records[i].Split(','); 
            if (array.Length != 4)
            {
                Console.WriteLine($"Line #{i + 1}: '{records[i]}' cannot be parsed");
                continue;
            }
            contacts[i].name = array[0];
            contacts[i].surname = array[1];
            contacts[i].phone = array[2];
            contacts[i].date = DateTime.Parse(array[3]);
        }
        return contacts;
    }

    static void SaveContactsToFile()
    {
        string[] lines = new string[contacts.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = $"{contacts[i].Item1},{contacts[i].Item2},{contacts[i].Item3}, {contacts[i].Item4}";
        }
        File.WriteAllLines(database, lines);
    }

    static string[] ReadDatabaseAllTextLines(string file)
    {
        if (!File.Exists(file))
        {
            File.WriteAllText(file, "");
        }
        return File.ReadAllLines(file);
    }
}