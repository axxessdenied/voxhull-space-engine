/*
/ Author    : Nick Slusarczyk
/ Github    : https://github.com/axxessdenied
*/

namespace Chernobog.Studio.Common
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using Newtonsoft.Json;

	public class RandomNameGenerator
    {
        #region Fields & Properties
        
	    class NameList
	    {
	    	public string[] first {get; set;}
	    	public string[] middle {get; set;}
	    	public string[] last {get; set;}
	    	
	    	public NameList()
	    	{
	    		first = new string[] {};
	    		middle = new string[] {};
	    		last = new string[] {};
	    	}
	    }
	    
	    private System.Random random;
	    private List<string> first;
	    private List<string> middle;
	    private List<string> last;
        
        #endregion

        #region Initialization
        
	    public RandomNameGenerator(System.Random rand, string fileName = "names.json")
	    {
	    	random = rand;
	    	var nameList = new NameList();
	    	
	    	var serializer = new JsonSerializer();
	    	
	    	using (var reader = new StreamReader(fileName))
		    	using (JsonReader jReader = new JsonTextReader(reader))
		    	{
		    		nameList = serializer.Deserialize<NameList>(jReader);
		    	}
		    first = new List<string>(nameList.first);
		    middle = new List<string>(nameList.middle);
		    last = new List<string>(nameList.last);
	    }
	    
        #endregion

        #region Methods
        
	    public string Generate(bool genMiddle = false)
	    {
		    var firstName = first[random.Next(first.Count)];
		    string lastName;
		    lastName = last[random.Next(last.Count)];

		    var name = new StringBuilder();
		    name.Append(firstName + " ");
		    if (genMiddle)
		    {
		    	var middleName = middle[random.Next(middle.Count)];
		    	name.Append(middleName + " ");
		    }
		    name.Append(lastName);
		    
		    return name.ToString();
	    }
	    
	    public List<string> GenerateMultiple(int number, bool genMiddle = false)
	    {
	    	var names = new List<string>();
	    	
	    	for (var i = 0; i < number; i++)
	    	{
	    		names.Add(Generate(genMiddle));
	    	}
	    	
	    	return names;
	    }
        #endregion
    }
}